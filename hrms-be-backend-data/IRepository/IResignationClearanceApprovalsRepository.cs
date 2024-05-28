using hrms_be_backend_data.RepoPayload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.IRepository
{
    public interface IResignationClearanceApprovalsRepository
    {
        Task<dynamic> CreateResignationClearanceApprovals(long CompanyId, long resignationClearanceID, long exitClearanceSetupID, long UserID,string departmentName);

        Task<IEnumerable<ResignationClearanceApprovalsDTO>> GetAllResignationClearanceApprovalsByResignationClearanceID(long companyID);

    }
}
