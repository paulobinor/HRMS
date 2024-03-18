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
        Task<dynamic> UpdateExitClearanceSetup(ExitClearanceSetupDTO request);
        Task<ExitClearanceSetupDTO> GetExitClearanceSetupByID(long ExitClearanceSetupID);
        Task<IEnumerable<ExitClearanceSetupDTO>> GetAllExitClearanceSetupByCompanyID(long companyID, int PageNumber, int RowsOfPage, string SearchVal);
        Task<dynamic> DeleteExitClearanceSetup(long ExitClearanceSetupID, string deletedBy);
    }
}
