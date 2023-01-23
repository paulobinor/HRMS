using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Data.IRepository
{
    public interface IPositionRepository 
    {
        Task<int> CreatePosition(CreatePositionDTO createposition);
        Task<int> UpdatePosition(UPdatePositionDTO Updateposition);
        Task<int> DeletePosition(DeletePositionDTO deletePosition);
        Task<int> DisablePosition(int PositionID, int CompanyIDDis);
        Task<int> ActivatePosition(int PositionID, int CompanyIDEna);
<<<<<<< HEAD
        Task<IEnumerable<PositionDTO>> GetAllPositions(int CompanyID);
=======
        Task<IEnumerable<PositionDTO>> GetAllPositions();
>>>>>>> e2edf564460ff757ff7e79041bfc7a224d357bef
        Task<IEnumerable<PositionDTO>> GetPositionByID(int CompanyID, int PositionID);
    }
}
