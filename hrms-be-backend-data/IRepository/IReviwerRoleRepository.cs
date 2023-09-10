using hrms_be_backend_data.RepoPayload.OnBoardingDTO.DTOs;

namespace hrms_be_backend_data.IRepositoryRole
{
    public interface IReviwerRoleRepository
    {
        Task<ReviwerRoleDTO> GetReviwerRoleById(long ReviwerRoleID);
        Task<IEnumerable<ReviwerRoleDTO>> GetAllReviwerRoleCompanyId(long companyId);
        
    }
}
