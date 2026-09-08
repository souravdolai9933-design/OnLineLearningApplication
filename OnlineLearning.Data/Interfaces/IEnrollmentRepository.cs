using OnlineLearning.Core.Common;
using OnlineLearning.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineLearning.Data.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<DbResult> EnrollStudent(EnrollStudentRequest request);
        Task<IEnumerable<EnrollmentDto>> GetStudentCourses(int userId);
        Task<EnrollmentDto?> GetEnrollmentById(int enrollmentId);
        Task<DbResult> CancelEnrollment(int enrollmentId);
    }
}
