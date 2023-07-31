using Com.XpressPayments.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.Position
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
