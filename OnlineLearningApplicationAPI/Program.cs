using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineLearning.Data.Interfaces;
using OnlineLearning.Data.Repository;
using OnlineLearning.Utilites.Connections;
using OnlineLearning.Utilites.Helper;
using Serilog;
using System;
using System.Linq;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting Online Learning Application Web API...");

    var builder = WebApplication.CreateBuilder(args);

    // 1. Serilog Configuration
    builder.Host.UseSerilog((context, services, config) =>
        config.ReadFrom.Configuration(context.Configuration)
              .ReadFrom.Services(services)
              .Enrich.FromLogContext());

    // 2. Controllers & Validation Response
    builder.Services.AddControllers()
        .ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .ToDictionary(
                        e => e.Key,
                        e => e.Value!.Errors.Select(x => x.ErrorMessage).ToArray()
                    );

                return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(new
                {
                    Errors = errors
                });
            };
        });

    builder.Services.AddEndpointsApiExplorer();

    // 3. Swagger Gen with JWT Bearer Support
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Online Learning Application API",
            Version = "v1",
            Description = "Enterprise RESTful Web API for Online Learning Platform"
        });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter JWT Bearer token format: Bearer {token}"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    // 4. JWT Authentication
    var jwtConfig = builder.Configuration.GetSection("Jwt");
    var jwtKey = Encoding.UTF8.GetBytes(jwtConfig["Key"] ?? "OnlineLearningApp_SuperSecretKey_2026_Must_Be_32_Chars!!");

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig["Issuer"] ?? "OnlineLearningAPI",
            ValidAudience = jwtConfig["Audience"] ?? "OnlineLearningClients",
            IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Log.Warning("JWT Authentication failed: {Error}", context.Exception.Message);
                return Task.CompletedTask;
            }
        };
    });

    // 5. Authorization Policies
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
        options.AddPolicy("InstructorOnly", policy => policy.RequireRole("Instructor"));
        options.AddPolicy("StudentOnly", policy => policy.RequireRole("Student"));
        options.AddPolicy("AllUsers", policy => policy.RequireRole("Admin", "Instructor", "Student"));
    });

    // 6. CORS Policy
    var allowedOrigins = builder.Configuration["AllowedOrigins"]?.Split(",") ?? Array.Empty<string>();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("OnlineLearningCors", policy =>
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials());
    });

    // 7. Dependency Injection Registration
    builder.Services.AddSingleton<DbConnectionFactory>();
    builder.Services.AddScoped<JwtHelper>();

    builder.Services.AddScoped<IAuthRepository, AuthRepository>();
    builder.Services.AddScoped<ICourseRepository, CourseRepository>();
    builder.Services.AddScoped<ISectionLessonRepository, SectionLessonRepository>();
    builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
    builder.Services.AddScoped<ICartWishlistRepository, CartWishlistRepository>();
    builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
    builder.Services.AddScoped<IQuizRepository, QuizRepository>();
    builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
    builder.Services.AddScoped<ICertificateNotificationRepository, CertificateNotificationRepository>();
    builder.Services.AddScoped<IBannerSupportRepository, BannerSupportRepository>();
    builder.Services.AddScoped<IAdminRepository, AdminRepository>();

    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

    var app = builder.Build();

    // 8. Middleware Pipeline
    app.UseSerilogRequestLogging();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Online Learning Application API v1");
        options.RoutePrefix = string.Empty;
    });

    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();

            if (error != null)
            {
                Log.Error(error.Error, "Unhandled API Exception");
                await context.Response.WriteAsJsonAsync(new
                {
                    Message = "An unexpected error occurred on the server."
                });
            }
        });
    });

    if (!app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }
    app.UseCors("OnlineLearningCors");
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
