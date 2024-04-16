using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_business.ILogic
{
    public interface IExitClearanceSetupService
    {
        Task<ExecutedResult<string>> CreateExitClearanceSetup(CreateExitClearanceSetupVm request, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> UpdateExitClearanceSetup(ExitClearanceSetupDTO updateDTO, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> DeleteExitClearanceSetup(ExitClearanceSetupDTO request, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<ExitClearanceSetupDTO>> GetExitClearanceSetupByID(long exitClearanceSetupID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<ExitClearanceSetupDTO>>> GetExitClearanceSetupByCompanyID(long companyID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<ExitClearanceSetupDTO>> GetExitClearanceSetupByHodEmployeeID(long HodEmployeeID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<ExitClearanceSetupDTO>> GetDepartmentThatIsFinalApprroval(long companyID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<ExitClearanceSetupDTO>>> GetDepartmentsThatAreNotFinalApproval(long companyID, string AccessKey, string RemoteIpAddress);
    }
}
