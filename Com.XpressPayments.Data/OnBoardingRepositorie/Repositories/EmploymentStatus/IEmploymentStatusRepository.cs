using Com.XpressPayments.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.EmploymentStatus
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
        Task<EmploymentStatusDTO> GetEmpLoymentStatusByName(string EmploymentStatusName, int companyId);
        Task<IEnumerable<EmploymentStatusDTO>> GetAllEmploymentStatusCompanyId(long companyId);
    }
}
