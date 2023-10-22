using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public interface IGradeRepository
    {
        Task<string> ProcessGrade(ProcessGradeReq payload);
        Task<string> DeleteGrade(DeleteGradeReq payload);
        Task<GradeWithTotalVm> GetGrades(long CompanyId, int PageNumber, int RowsOfPage);
        Task<GradeWithTotalVm> GetGradesDeleted(long CompanyId, int PageNumber, int RowsOfPage);
        Task<GradeVm> GetGradeById(long Id);
        Task<GradeVm> GetGradeByName(string GradeName, long CompanyId);
    }
}
