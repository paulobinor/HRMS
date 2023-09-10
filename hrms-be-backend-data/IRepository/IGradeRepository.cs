using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface IGradeRepository
    {
        Task<dynamic> CreateGrade(CreateGradeDTO create, string createdbyUserEmail);
        Task<dynamic> UpdateGrade(UpdateGradeDTO update, string updatedbyUserEmail);
        Task<dynamic> DeleteGrade(DeleteGradeDTO delete, string deletedbyUserEmail);
        Task<IEnumerable<GradeDTO>> GetAllActiveGrade();
        Task<IEnumerable<GradeDTO>> GetAllGrade();
        Task<GradeDTO> GetGradeById(long GradeID);
        Task<GradeDTO> GetGradeByName(string GradeName);
        Task<GradeDTO> GetGradeByCompany(string GradeName, int companyId);
        Task<IEnumerable<GradeDTO>> GetAllGradeCompanyId(long CompanyId);




    }
}
