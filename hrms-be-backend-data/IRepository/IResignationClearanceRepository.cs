using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface IResignationClearanceRepository
    {
        Task<int> CreateResignationClearance(ResignationClearanceDTO resignation);
        Task<ResignationClearanceDTO> GetResignationClearanceByID(long ID);
        Task<ResignationClearanceDTO> GetResignationClearanceByUserID(long UserID);
        Task<List<ResignationClearanceDTO>> GetPendingResignationClearanceByUserID(long userID);
        Task<int> ApprovePendingResignationClearance(long userID, long ID);
        Task<int> DisapprovePendingResignationClearance(long userID, long ID, string reason);
    }
}
