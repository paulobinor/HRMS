using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_data.RepoPayload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.IRepository
{
    public interface IExitClearanceSetupRepository
    {
        Task<dynamic> CreateExitClearanceSetup(ExitClearanceSetupDTO request);
        Task<dynamic> UpdateExitClearanceSetup(UpdateExitClearanceSetupDTO request);
        Task<ExitClearanceSetupDTO> GetExitClearanceSetupByID(long ExitClearanceSetupID);
        Task<ExitClearanceSetupDTO> GetExitClearanceSetupByHodEmployeeID(long HodEmployeeID);
        Task<ExitClearanceSetupDTO> GetExitClearanceSetupByDepartmentID(long departmentID);
        Task<IEnumerable<ExitClearanceSetupDTO>> GetAllExitClearanceSetupByCompanyID(long companyID);
        Task<dynamic> DeleteExitClearanceSetup(long ExitClearanceSetupID, long deletedByUserID);
        Task<ExitClearanceSetupDTO> GetDepartmentThatIsFinalApprroval(long companyID);
        Task<IEnumerable<ExitClearanceSetupDTO>> GetDepartmentsThatAreNotFinalApproval(long companyID);
    }
}
