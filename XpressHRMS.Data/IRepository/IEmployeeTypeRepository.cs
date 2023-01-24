using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Data.IRepository
{
    public interface IEmployeeTypeRepository
    {
        Task<int> CreateEmployeeType(CreateEmployeeTypeDTO createEmployeeType);
        Task<int> UpdateEmployeeType(UpdateEmployeeTypeDTO UpdateEmployeeType);
        Task<int> DeleteEmployeeType(DelEmployeeTypeDTO deleteEmployeeType);
        Task<int> DisableEmployeeType(int EmployeeTypeID, int CompanyIDDis);
        Task<int> ActivateEmployeeType(int EmployeeTypeID, int CompanyIDEna);
<<<<<<< HEAD
        Task<IEnumerable<EmployeeTypeDTO>> GetAllEmployeeType(int CompanyID);
=======
        Task<IEnumerable<EmployeeTypeDTO>> GetAllEmployeeType();
>>>>>>> e2edf564460ff757ff7e79041bfc7a224d357bef
        Task<IEnumerable<EmployeeTypeDTO>> GetEmployeeTypeByID(int CompanyID, int EmployeeTypeID);
    }
}
