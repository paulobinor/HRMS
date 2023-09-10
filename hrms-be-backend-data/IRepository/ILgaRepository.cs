using hrms_be_backend_data.RepoPayload;
using System.Collections;

namespace hrms_be_backend_data.IRepository
{
    public interface ILgaRepository
    {
         Task<IEnumerable<LgaDTO>> GetAllLga(long StateID);
        Task<IEnumerable> GetLgaByStateId(long StateID);
        Task<LgaDTO> GetLgaByName(string LGA_Name);
    }
}
