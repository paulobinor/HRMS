using hrms_be_backend_data.RepoPayload;
using System.Collections;

namespace hrms_be_backend_data.IRepository
{
    public  interface IStateRepository
    {
        Task<IEnumerable<StateDTO>> GetAllState(long CountryID);
        Task<IEnumerable> GetStateByCountryId(long CountryID);
        Task<StateDTO> GetStateByName(string StateName);
    }
}
