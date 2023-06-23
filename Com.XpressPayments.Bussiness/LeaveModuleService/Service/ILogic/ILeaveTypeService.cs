using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.LeaveModuleDTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.LeaveModuleService.Service.ILogic
{
    public interface ILeaveTypeService
    {
        Task<BaseResponse> CreateLeaveType(CreateLeaveTypeDTO creatDto, RequesterInfo requester);
        Task<BaseResponse> UpdateLeaveType(UpdateLeaveTypeDTO updateDto, RequesterInfo requester);
        Task<BaseResponse> DeleteLeaveType(DeleteLeaveTypeDTO deleteDto, RequesterInfo requester);
        Task<BaseResponse> GetAllActiveLeaveType(RequesterInfo requester);
        Task<BaseResponse> GetAllLeaveType(RequesterInfo requester);
        Task<BaseResponse> GetLeaveTypeById(long LeaveTypeId, RequesterInfo requester);
        Task<BaseResponse> GetLeavebyCompanyId(long companyId, RequesterInfo requester);
    }
}
