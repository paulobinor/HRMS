using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface IPositionRepository
    {
        Task<dynamic> CreatePosition(CreatePositionDTO create, string createdbyUserEmail);
        Task<dynamic> UpdatePosition(UpadtePositionDTO update, string updatedbyUserEmail);
        Task<dynamic> Deleteposition(DeletePositionDTO delete, string deletedbyUserEmail);
        Task<IEnumerable<PositionDTO>> GetAllActivePosition();
        Task<IEnumerable<PositionDTO>> GetAllPosition();
        Task<PositionDTO> GetPositionById(long PositionID);
        Task<PositionDTO> GetPositionByName(string PositionName);
        Task<PositionDTO> GetPositionByCompany(string PositionName, int companyId);
        Task<IEnumerable<PositionDTO>> GetAllPositionCompanyId(long PositionID);

    }
}
