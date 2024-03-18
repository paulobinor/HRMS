

using hrms_be_backend_data.RepoPayload.OnBoardingDTO.DTOs;

namespace hrms_be_backend_data.IRepository
{
    public  interface IReviwerRepository
    {
        Task<dynamic> CreateReviwer(CreateReviwerDTO CreateReviwer, string createdbyUserEmail);
        Task<dynamic> DeleteReviwer(DeleteReviwerDTO del, string deletedbyUserEmail);
        Task<ReviwerDTO> GetReviwerById(long ReviwerID);
        Task<ReviwerDTO> GetReviwerByName(long UserId);
        Task<IEnumerable<ReviwerDTO>> GetAllReviwerCompanyId(long companyId);
        Task<ReviwerDTO> GetReviwerByCompany(long UserId, long companyId);
    }
}
