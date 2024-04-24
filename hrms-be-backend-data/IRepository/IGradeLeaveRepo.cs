using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public  interface IGradeLeaveRepo
    {
        Task<string> CreateGradeLeave(CreateGradeLeaveDTO create);
        Task<dynamic> UpdateGradeLeave(UpdateGradeLeaveDTO update, string updatedbyUserEmail);
        Task<dynamic> DeleteGradeLeave(DeleteGradeLeaveDTO delete, string deletedbyUserEmail);
        Task<IEnumerable<GradeLeaveDTO>> GetAllActiveGradeLeave();
        Task<IEnumerable<GradeLeaveDTO>> GetAllGradeLeave();
        Task<GradeLeaveDTO> GetGradeLeaveById(long GradeLeaveID);
        Task<IEnumerable<GradeLeaveDTO>> GetAllGradeLeaveCompanyId(long CompanyId);
    }
}
