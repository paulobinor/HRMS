using Com.XpressPayments.Data.LeaveModuleDTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.LeaveModuleRepository.LeaveType
{
    public  interface ILeaveTypeRepository
    {
        Task<dynamic> CreateLeaveType(CreateLeaveTypeDTO create, string createdbyUserEmail);
        Task<dynamic> UpdateLeaveType(UpdateLeaveTypeDTO update, string updatedbyUserEmail);
        Task<dynamic> DeleteLeaveType(DeleteLeaveTypeDTO delete, string deletedbyUserEmail);
        Task<IEnumerable<LeaveTypeDTO>> GetAllActiveLeaveType();
        Task<IEnumerable<LeaveTypeDTO>> GetAllLeaveType();
        Task<LeaveTypeDTO> GetLeaveTypeById(long LeaveTypeId);
        Task<LeaveTypeDTO> GetLeaveTypeByName(string LeaveTypeName);
        Task<LeaveTypeDTO> GetLeaveTypeByCompany(string LeaveTypeName, int companyId);
        Task<IEnumerable<LeaveTypeDTO>> GetAllLeaveTypeCompanyId(long CompanyId);
    }
}
