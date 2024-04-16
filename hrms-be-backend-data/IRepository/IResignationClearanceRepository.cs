using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface IResignationClearanceRepository
    {
        Task<dynamic> CreateResignationClearance(ResignationClearanceDTO resignation);
        Task<ResignationClearanceDTO> GetResignationClearanceByID(long ID);
        Task<ResignationClearanceDTO> GetResignationClearanceByEmployeeID(long UserID);
        Task<IEnumerable<ResignationClearanceDTO>> GetAllResignationClearanceByCompany(long companyID, int PageNumber, int RowsOfPage, string SearchVal);

        //Task<List<ResignationClearanceDTO>> GetPendingResignationClearanceByUserID(long userID);
        Task<dynamic> ApprovePendingResignationClearance(long userID, long ID);
        Task<dynamic> DisapprovePendingResignationClearance(long userID, long ID, string reason);
    }
}
