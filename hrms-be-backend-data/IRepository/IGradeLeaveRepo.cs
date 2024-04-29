using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public  interface IGradeLeaveRepo
    {
        Task<string> CreateGradeLeave(CreateGradeLeaveDTO create);
        Task<GradeLeaveDTO> UpdateGradeLeave(UpdateGradeLeaveDTO update);
        Task<GradeLeaveDTO> DeleteGradeLeave(DeleteGradeLeaveDTO delete);
        Task<IEnumerable<GradeLeaveDTO>> GetAllActiveGradeLeave();
        Task<IEnumerable<GradeLeaveDTO>> GetAllGradeLeave();
        Task<GradeLeaveDTO> GetGradeLeaveById(long GradeLeaveID);
        Task<IEnumerable<GradeLeaveDTO>> GetAllGradeLeaveCompanyId(long CompanyId);
        Task<IEnumerable<GradeLeaveDTO>> GetEmployeeGradeLeaveTypes(long companyID, long employeeID);
    }
}
