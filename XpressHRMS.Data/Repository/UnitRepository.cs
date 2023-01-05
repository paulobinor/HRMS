using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;
using XpressHRMS.Data.Enums;
using XpressHRMS.Data.IRepository;
using XpressHRMS.IRepository;

namespace XpressHRMS.Data.Repository
{
    public class UnitRepository : IUnitRepository
    {
        private readonly ILogger<UnitRepository> _logger;
        private readonly IDapperGeneric _dapper;

        public UnitRepository(ILogger<UnitRepository> logger)
        {
            _logger = logger;

        }

        public async Task<int> CreateUnit(UnitDTO payload)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", ACTION.INSERT);
                param.Add("@UnitName", payload.UnitName);
                param.Add("@HODEmployeeID", payload.HODEmployeeID);
                param.Add("@CreatedByUserID", payload.CreatedByUserID);
                param.Add("@isActive", true);
                return await _dapper.Get<int>("", param, commandType: CommandType.StoredProcedure);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }
        public async Task<int> DeleteUnit(int UnitID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", ACTION.DELETE);
                param.Add("@UnitID", UnitID);
                return await _dapper.Get<int>("", param, commandType: CommandType.StoredProcedure);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }

        public async Task<int> DisableUnit(int UnitID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", ACTION.DISABLE);
                param.Add("@UnitID", UnitID);
                return await _dapper.Get<int>("", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }
        public async Task<int> ActivateUnit(int UnitID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", ACTION.ACTIVATE);
                param.Add("@UnitID", UnitID);
                return await _dapper.Get<int>("", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }

        public async Task<int> UpdateUnit(UpdateUnitDTO payload)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", ACTION.UPDATE);
                param.Add("@UnitName", payload.UnitName);
                param.Add("@HODEmployeeID", payload.HODEmployeeID);
                param.Add("@UnitID", payload.UnitID);
                param.Add("@DepartmentID", payload.DepartmentID);
                return await _dapper.Get<int>("", param, commandType: CommandType.StoredProcedure);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }
        public async Task<List<UnitDTO>> GetAllUnits(UnitDTO payload)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", ACTION.SELECTALL);
                return await _dapper.GetAll<UnitDTO>("", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }

        }

        public async Task<List<UnitDTO>> GetUnitByID(int UnitID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", ACTION.SELECTBYID);
                param.Add("@UnitID", UnitID);
                return await _dapper.GetAll<UnitDTO>("", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }

        }

    }
}
