﻿using System;
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
        Task<int> DisablePosition(int PositionID);
        Task<int> ActivatePosition(int PositionID);
        Task<IEnumerable<PositionDTO>> GetAllPositions();
        Task<IEnumerable<PositionDTO>> GetPositionByID(int CompanyID, int PositionID);
    }
}
