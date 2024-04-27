using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public  interface ILeaveTypeRepository
    {
        Task<CreateLeaveTypeDTO> CreateLeaveType(CreateLeaveTypeDTO create);
        Task<LeaveTypeDTO> UpdateLeaveType(UpdateLeaveTypeDTO update);
        Task<LeaveTypeDTO> DeleteLeaveType(DeleteLeaveTypeDTO delete);
        Task<IEnumerable<LeaveTypeDTO>> GetAllActiveLeaveType();
        Task<IEnumerable<LeaveTypeDTO>> GetAllLeaveType();
        Task<LeaveTypeDTO> GetLeaveTypeById(long LeaveTypeId);
        Task<LeaveTypeDTO> GetLeaveTypeByName(string LeaveTypeName);
        Task<LeaveTypeDTO> GetLeaveTypeByCompany(string LeaveTypeName, long companyId);
        Task<IEnumerable<LeaveTypeDTO>> GetAllLeaveTypeCompanyId(long CompanyId);
    }
}
