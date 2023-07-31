using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        private readonly IDapperGeneric _dapperr;
        private readonly string _connectionString;

        public UnitRepository(ILogger<UnitRepository> logger, IConfiguration configuration, IDapperGeneric dapperr)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("HRMSConnectionString");
            _dapperr = dapperr;


        }

        public async Task<int> CreateUnit(CreateUnitDTO payload)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.INSERT);
                    param.Add("@UnitName", payload.UnitName);
                    param.Add("@HODEmployeeID", payload.HODEmployeeID);
<<<<<<< HEAD:HRMS/XpressHRMS.Data/Repository/UnitRepository.cs
                    param.Add("@CreatedBy", payload.CreatedByUserID);
=======
                    //param.Add("@CreatedBy", payload.CreatedByUserID);
>>>>>>> parent of 55b359c (commit):XpressHRMS.Data/Repository/UnitRepository.cs
                    param.Add("@CompanyID", payload.CompanyID);
                    dynamic response = await _dapper.ExecuteAsync("Sp_Unit", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
                   


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }
        public async Task<int> DeleteUnit(int UnitID, int CompanyID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DELETE);
                    param.Add("@UnitID", UnitID);
                    param.Add("@CompanyID", CompanyID);
                    dynamic response = await _dapper.ExecuteAsync("Sp_Unit", param: param, commandType: CommandType.StoredProcedure);
                    return response;

                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }

        public async Task<int> DisableUnit(int UnitID, int CompanyID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DISABLE);
                    param.Add("@UnitID", UnitID);
                    param.Add("@CompanyID", CompanyID);
                    dynamic response = await _dapper.ExecuteAsync("Sp_Unit", param: param, commandType: CommandType.StoredProcedure);
                    return response;

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }
        public async Task<int> ActivateUnit(int UnitID, int CompanyID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {

                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.ACTIVATE);
                    param.Add("@UnitID", UnitID);
                    param.Add("@UnitID", CompanyID);
                    dynamic response = await _dapper.ExecuteAsync("Sp_Unit", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }

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
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.UPDATE);
                    param.Add("@UnitName", payload.UnitName);
                    param.Add("@HODEmployeeID", payload.HODEmployeeID);
                    param.Add("@UnitID", payload.UnitID);
                    param.Add("@DepartmentID", payload.DepartmentID);
                    param.Add("@CompanyID", payload.CompanyID);
                    dynamic response = await _dapper.ExecuteAsync("Sp_Unit", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }

                  


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }
        public async Task<List<UnitDTO>> GetAllUnits(int CompanyID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.SELECTALL);
                    param.Add("@CompanyID", CompanyID);
                    var response = await _dapperr.GetAll<UnitDTO>("Sp_Unit",param, commandType: CommandType.StoredProcedure);
                    return response;
                }
                   

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }

        }

        public async Task<UnitDTO> GetUnitByID(int UnitID, int CompanyID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.SELECTBYID);
                    param.Add("@UnitID", UnitID);
                    param.Add("@CompanyID", CompanyID);
                    var response = await _dapperr.Get<UnitDTO>("Sp_Unit", param, commandType: CommandType.StoredProcedure);
                    return response;
                }
               

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }

        }

    }
}
