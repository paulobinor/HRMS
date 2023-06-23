using Com.XpressPayments.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.EmployeeType
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
