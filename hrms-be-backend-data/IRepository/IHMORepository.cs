using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface IHMORepository
    {
        Task<dynamic> CreateHMO(CreateHMODTO create, string createdbyUserEmail);
        Task<dynamic> UpdateHMO(UpdateHMODTO HMO, string updatedbyUserEmail);
        Task<dynamic> DeleteHMO(DeleteHMODTO DEL, string deletedbyUserEmail);
        Task<IEnumerable<HMODTO>> GetAllActiveHMO();
        Task<IEnumerable<HMODTO>> GetAllHMO();
        Task<HMODTO> GetHMOById(long ID);
        Task<HMODTO> GetHMOByName(string HMONumber);
        Task<IEnumerable<HMODTO>> GetAllHMOCompanyId(long companyId);
    }
}
