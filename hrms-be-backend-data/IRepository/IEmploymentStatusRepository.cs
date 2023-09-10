using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public  interface IEmploymentStatusRepository
    {
        Task<dynamic> CreateEmploymentStatus(CreateEmploymentStatusDTO create, string createdbyUserEmail);
        Task<dynamic> UpdateEmploymentStatus(UpdateEmploymentStatusDTO Update, string updatedbyUserEmail);
        Task<dynamic> DeleteEmploymentStatus(DeleteEmploymentStatusDTO DelEmpStatus, string deletedbyUserEmail);
        Task<IEnumerable<EmploymentStatusDTO>> GetAllActiveEmploymentStatus();
        Task<IEnumerable<EmploymentStatusDTO>> GetAllEmpLoymentStatus();
        Task<EmploymentStatusDTO> GetEmpLoymentStatusById(long EmploymentStatusID);
        Task<EmploymentStatusDTO> GetEmpLoymentStatusByName(string EmploymentStatusName);
        Task<EmploymentStatusDTO> GetEmpLoymentStatusByCompany(string EmploymentStatusName, int companyId);
        Task<IEnumerable<EmploymentStatusDTO>> GetAllEmploymentStatusCompanyId(long companyId);
    }
}
