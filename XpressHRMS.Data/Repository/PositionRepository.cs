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
    public class PositionRepository : IPositionRepository
    {
        private readonly ILogger<PositionRepository> _logger;
        private readonly IDapperGeneric _dapper;
        private readonly string _connectionString;

        public PositionRepository(ILogger<PositionRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("HRMSConnectionString");


        }


        public async Task<int> CreatePosition(CreatePositionDTO createposition)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.INSERT);
                    param.Add("@CompanyID", createposition.CompanyID);
                    param.Add("@PositionName", createposition.PositionName);
                    param.Add("@CreatedBy", createposition.CreatedBy);


                    dynamic response = await _dapper.ExecuteAsync("sp_Position", param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }

        public async Task<int> UpdatePosition(UPdatePositionDTO Updateposition)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.UPDATE);
                    param.Add("@PositionIDUpd", Updateposition.PositionID);
                    param.Add("@CompanyIDUpd", Updateposition.CompanyID);
                    param.Add("@PositionNameUpd", Updateposition.PositionName);
                    param.Add("@CreatedByUpd", Updateposition.CreatedBy);

                    dynamic response = await _dapper.ExecuteAsync("sp_Position", param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }

            }

            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
      
            }
        }

        public async Task<int> DeletePosition(DeletePositionDTO deletePosition)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DELETE);
                    param.Add("@PositionIDDel", deletePosition.PositionID);
                    param.Add("@CompanyIDDel", deletePosition.CompanyID);
                    //param.Add("@CompanyIDUpd", deletePosition.CompanyID);
                    dynamic response = await _dapper.ExecuteAsync("sp_Position", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        public async Task<int> DisablePosition(int PositionID, int CompanyIDDis)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DISABLE);
                    param.Add("@PositionID", PositionID);
                    param.Add("@CompanyIDDis", CompanyIDDis);
                    dynamic response = await _dapper.ExecuteAsync("sp_Position", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        public async Task<int> ActivatePosition(int PositionID, int CompanyIDEna)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.ACTIVATE);
                    param.Add("@PositionID", PositionID);
                    param.Add("@CompanyIDEna", CompanyIDEna);
                    dynamic response = await _dapper.ExecuteAsync("sp_Position", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        public async Task<IEnumerable<PositionDTO>> GetAllPositions()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    int d = (int)GetAllDefault.GetAll;
                    param.Add("@Status", ACTION.SELECTALL);
                    var response = await _dapper.QueryAsync<PositionDTO>("sp_Position", param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<IEnumerable<PositionDTO>> GetPositionByID(int CompanyID, int PositionID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    int d = (int)GetAllDefault.GetAll;
                    param.Add("@Status", ACTION.SELECTBYID);
                    param.Add("@CompanyIDGet", CompanyID);
                    param.Add("@PositionIDGet", PositionID);
                    var response = await _dapper.QueryAsync<PositionDTO>("sp_Position", param: param, commandType: CommandType.StoredProcedure);
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
