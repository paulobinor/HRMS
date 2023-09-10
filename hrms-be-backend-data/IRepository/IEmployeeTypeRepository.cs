using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface IEmployeeTypeRepository
    {
        Task<dynamic> CreateEmployeeType(CraeteEmployeeTypeDTO create, string createdbyUserEmail);
        Task<dynamic> UpdateEmployeeType(UpdateEmployeeTypeDTO update, string updatedbyUserEmail);
        Task<dynamic> DeleteEmployeeType(DeleteEmployeeTypeDTO delete, string deletedbyUserEmail);
        Task<IEnumerable<EmployeeTypeDTO>> GetAllActiveEmployeeType();
        Task<IEnumerable<EmployeeTypeDTO>> GetAllEmployeeType();
        Task<EmployeeTypeDTO> GetEmployeeTypeById(long EmployeeTypeID);
        Task<EmployeeTypeDTO> GetEmployeeTypeByName(string EmployeeTypeName);
        Task<EmployeeTypeDTO> GetEmployeeTypeByCompany(string EmployeeTypeName, int companyId);
        Task<IEnumerable<EmployeeTypeDTO>> GetAllEmployeeTypeCompanyId(long EmployeeTypeID);
    }
}
