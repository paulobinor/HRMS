using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public  interface IGradeLeaveRepo
    {
        Task<dynamic> CreateGradeLeave(CreateGradeLeaveDTO create, string Created_By_User_Email);
        Task<dynamic> UpdateGradeLeave(UpdateGradeLeaveDTO update, string updatedbyUserEmail);
        Task<dynamic> DeleteGradeLeave(DeleteGradeLeaveDTO delete, string deletedbyUserEmail);
        Task<IEnumerable<GradeLeaveDTO>> GetAllActiveGradeLeave();
        Task<IEnumerable<GradeLeaveDTO>> GetAllGradeLeave();
        Task<GradeLeaveDTO> GetGradeLeaveById(long GradeLeaveID);
        Task<IEnumerable<GradeLeaveDTO>> GetAllGradeLeaveCompanyId(long CompanyId);
    }
}
