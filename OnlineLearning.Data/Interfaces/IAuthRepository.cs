using OnlineLearning.Core.Common;
using OnlineLearning.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineLearning.Data.Interfaces
{
    public interface IAuthRepository
    {
        Task<DbResult> RegisterUser(RegisterRequest request, string passwordHash);
        Task<LoginResponse> LoginUser(LoginRequest request);
        Task<UserProfileDto?> GetUserById(int userId);
        Task<DbResult> UpdateUserProfile(UpdateProfileRequest request);
        Task<DbResult> ChangePassword(int userId, string oldPasswordHash, string newPasswordHash);
        Task<DbResult> ForgotPassword(string email, string resetToken, System.DateTime expiry);
        Task<DbResult> ResetPassword(string email, string token, string newPasswordHash);
        Task<DbResult> SaveRefreshToken(int userId, string refreshToken, System.DateTime expiryDate);
        Task<IEnumerable<RoleDto>> GetRoles();
        Task<RoleDto?> GetRoleById(int roleId);
        Task<DbResult> CreateRole(CreateRoleRequest request);
        Task<DbResult> UpdateRole(UpdateRoleRequest request);
        Task<DbResult> DeleteRole(int roleId);
    }
}
