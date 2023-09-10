using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface IUnitHeadRepository
    {
        Task<dynamic> CreateUnitHead(CreateUnitHeadDTO create, string createdbyUserEmail);
        Task<dynamic> UpdateUnitHead(UpdateUnitHeadDTO update, string updatedbyUserEmail);
        Task<dynamic> DeleteUnitHead(DeleteUnitHeadDTO delete, string deletedbyUserEmail);
        Task<IEnumerable<UnitHeadDTO>> GetAllActiveUnitHead();
        Task<IEnumerable<UnitHeadDTO>> GetAllUnitHead();
        Task<UnitHeadDTO> GetUnitHeadById(long UnitHeadID);
        Task<UnitHeadDTO> GetUnitHeadByUserID(long UserID);
        Task<UnitHeadDTO> GetUnitHeadByUnitHeadName(string UnitHeadName);
        Task<UnitHeadDTO> GetUnitHeadByCompany(long UserID, long companyId);
        Task<IEnumerable<UnitHeadDTO>> GetAllUnitHeadCompanyId(long UnitHeadID);
    }
}
