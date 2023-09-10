using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface IUnitRepository
    {
        Task<dynamic> CreateUnit(CreateUnitDTO unit, string createdbyUserEmail);
        Task<dynamic> UpdateUnit(UpdateUnitDTO unit, string updatedbyUserEmail);
        Task<dynamic> DeleteUnit(DeleteUnitDTO unit, string deletedbyUserEmail);
        Task<IEnumerable<UnitDTO>> GetAllActiveUnit();
        Task<IEnumerable<UnitDTO>> GetAllUnit();
        Task<UnitDTO> GetUnitById(long UnitID);
        Task<UnitDTO> GetUnitByName(string UnitName);
        Task<UnitDTO> GetUnitByCompany(string UnitName, int companyId);
        Task<IEnumerable<UnitDTO>> GetAllUnitCompanyId(long CompanyId);
    }
}
