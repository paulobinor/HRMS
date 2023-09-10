using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface IResignationRepository
    {
        Task<int> CreateResignation(ResignationDTO request);
        Task<ResignationDTO> GetResignationByID(long ID);
        Task<ResignationFormDTO> GetResignationByUserID(long UserID);
        Task<List<ResignationDTO>> GetResignationByCompanyID(long companyID, bool isApproved);
        Task<int> DeleteResignation(long ID, string deletedBy, string deleteReason);
        Task<List<ResignationDTO>> GetPendingResignationByUserID(long userID);
        Task<int> ApprovePendingResignation(long userID, long SRFID);
        Task<int> DisapprovePendingResignation(long userID, long SRFID, string reason);
        Task<int> UpdateResignation(ResignationDTO resignation);

    }
}
