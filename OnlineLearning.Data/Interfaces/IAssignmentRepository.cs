using OnlineLearning.Core.Common;
using OnlineLearning.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineLearning.Data.Interfaces
{
    public interface IAssignmentRepository
    {
        Task<IEnumerable<AssignmentDto>> GetAssignments();
        Task<AssignmentDto?> GetAssignmentById(int assignmentId);
        Task<IEnumerable<AssignmentDto>> GetAssignmentsByCourse(int courseId);
        Task<DbResult> CreateAssignment(CreateAssignmentRequest request);
        Task<DbResult> UpdateAssignment(UpdateAssignmentRequest request);
        Task<DbResult> DeleteAssignment(int assignmentId);
        Task<DbResult> SubmitAssignment(SubmitAssignmentRequest request);
        Task<IEnumerable<AssignmentSubmissionDto>> GetAssignmentSubmissions(int assignmentId);
        Task<AssignmentSubmissionDto?> GetSubmissionById(int submissionId);
        Task<DbResult> GradeAssignment(GradeAssignmentRequest request);
    }
}
