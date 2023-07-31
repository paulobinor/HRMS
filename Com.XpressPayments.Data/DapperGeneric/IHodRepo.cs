using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Data.IRepository
{
    public interface IHodRepo
    {
        Task<int> CreateHOD(CreateHodDTO createHOD);
        Task<int> UpdateHOD(UpdateHodDTO UpdateHOD);
        Task<int> DeleteHOD(DelHodDTO deleteHOD);
        Task<int> DisableHOD(DisableHodDTO disable);
        Task<int> ActivateHOD(EnableHodDTO enable);
        Task<List<HodDTO>> GetAllHOD(string CompanyID);
        Task<List<HodDTO>> GetHODByID(string CompanyID, int HodID, int DepartmentID);
    }
}
