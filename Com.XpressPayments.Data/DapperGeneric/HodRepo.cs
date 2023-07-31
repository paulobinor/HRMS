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
    public class HodRepo : IHodRepo
    {
        private readonly ILogger<HodRepo> _logger;
        private readonly IDapperGeneric _dapperr;
        private readonly string _connectionString;

        public HodRepo(ILogger<HodRepo> logger, IConfiguration configuration, IDapperGeneric dapper)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("HRMSConnectionString");
            _dapperr = dapper;
        }

        public async Task<int> CreateHOD(CreateHodDTO createHOD)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.INSERT);
                    param.Add("@HODName", createHOD.HODName);
                    param.Add("@DepartmentID", createHOD.DepartmentID);
                    param.Add("@CompanyID", createHOD.CompanyID);
                    param.Add("@CreatedBy", createHOD.CreatedBy);
                    //param.Add("@IsActive", createHOD.IsActive);


                    dynamic response = await _dapper.ExecuteAsync("sp_HOD", param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }

        public async Task<int> UpdateHOD(UpdateHodDTO UpdateHOD)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.UPDATE);
                    param.Add("@HodID", UpdateHOD.HodID);
                    param.Add("@HODNameUpd", UpdateHOD.HODName);
                    param.Add("@DepartmentIDUpd", UpdateHOD.DepartmentID);
                    param.Add("@CompanyIDUpd", UpdateHOD.CompanyID);
                    param.Add("@CreatedByUpd", UpdateHOD.DepartmentID);
                    param.Add("@UpdatedBy", UpdateHOD.UpdatedBy);

                    dynamic response = await _dapper.ExecuteAsync("sp_HOD", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        public async Task<int> DeleteHOD(DelHodDTO deleteHOD)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DELETE);
                    param.Add("@HodID", deleteHOD.HodID);
                    param.Add("@CompanyIDDel", deleteHOD.CompanyID);
                    param.Add("@DepartmentID", deleteHOD.DepartmentID);
                    param.Add("@DeletedBy", deleteHOD.DeletedBy);

                    dynamic response = await _dapper.ExecuteAsync("sp_HOD", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        public async Task<int> DisableHOD(DisableHodDTO disable)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DISABLE);
                    param.Add("@HodIDDis", disable.HodID);
                    param.Add("@CompanyID", disable.CompanyID);
                    param.Add("@DepartmentID", disable.DepartmentID);
                    dynamic response = await _dapper.ExecuteAsync("sp_HOD", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        public async Task<int> ActivateHOD(EnableHodDTO enable)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.ACTIVATE);
                    param.Add("@HodIDEna", enable.HodID);
                    param.Add("@CompanyIDEna", enable.CompanyID);
                    dynamic response = await _dapper.ExecuteAsync("sp_HOD", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        public async Task<List<HodDTO>> GetAllHOD(string CompanyID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    int d = (int)GetAllDefault.GetAll;
                    param.Add("@Status", ACTION.SELECTALL);
                    param.Add("@CompanyIDGet", CompanyID);
                    var response = await _dapperr.GetAll<HodDTO>("sp_HOD", param, commandType: CommandType.StoredProcedure);
                    return response;


                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<List<HodDTO>> GetHODByID(string CompanyID, int HodID, int DepartmentID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    int d = (int)GetAllDefault.GetAll;
                    param.Add("@Status", ACTION.SELECTBYID);
                    param.Add("@CompanyIDGet", CompanyID);
                    param.Add("@HodID", HodID);
                    param.Add("@DepartmentID", DepartmentID);
                    var response = await _dapperr.GetAll<HodDTO>("sp_HOD", param, commandType: CommandType.StoredProcedure);
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
