﻿using Dapper;
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
    public class EmployeeTypeRepository : IEmployeeTypeRepository
    {
        private readonly ILogger<EmployeeTypeRepository> _logger;
        //private readonly IDapperGeneric _dapper;
        private readonly IDapperGeneric _dapperr;

        private readonly string _connectionString;

        public EmployeeTypeRepository(ILogger<EmployeeTypeRepository> logger, IConfiguration configuration, IDapperGeneric dapperr)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("HRMSConnectionString");
            _dapperr = dapperr;


        }

        public async Task<int> CreateEmployeeType(CreateEmployeeTypeDTO createEmployeeType)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.INSERT);
                    param.Add("@CompanyID", createEmployeeType.CompanyID);
                    param.Add("@EmployeeTypeName", createEmployeeType.EmployeeTypeName);
                    
                    dynamic response = await _dapper.ExecuteAsync("sp_EmployeeType", param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        public async Task<int> UpdateEmployeeType(UpdateEmployeeTypeDTO UpdateEmployeeType)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.UPDATE);
                    param.Add("@CompanyIDUpd", UpdateEmployeeType.CompanyID);
                    param.Add("@EmployeeTypeIDUpd", UpdateEmployeeType.EmployeeTypeID);
                    param.Add("@EmployeeTypeNameUpd", UpdateEmployeeType.EmployeeTypeName);

                    dynamic response = await _dapper.ExecuteAsync("sp_EmployeeType", param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        public async Task<int> DeleteEmployeeType(DelEmployeeTypeDTO deleteEmployeeType)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DELETE);
                    param.Add("@CompanyID", deleteEmployeeType.CompanyID);
                    param.Add("@EmployeeTypeIDDel", deleteEmployeeType.EmployeeTypeID);
                    dynamic response = await _dapper.ExecuteAsync("sp_EmployeeType", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        public async Task<int> DisableEmployeeType(int EmployeeTypeID , int CompanyIDDis)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DISABLE);
                    param.Add("@EmployeeTypeID", EmployeeTypeID);
                    param.Add("@CompanyIDDis", CompanyIDDis);
                    dynamic response = await _dapper.ExecuteAsync("sp_EmployeeType", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        public async Task<int> ActivateEmployeeType(int EmployeeTypeID, int CompanyIDEna)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.ACTIVATE);
                    param.Add("@EmployeeTypeID", EmployeeTypeID);
                    param.Add("@CompanyIDEna", CompanyIDEna);
                    dynamic response = await _dapper.ExecuteAsync("sp_EmployeeType", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }

        public async Task<List<EmployeeTypeDTO>> GetAllEmployeeType(int CompanyID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    int d = (int)GetAllDefault.GetAll;
                    param.Add("@Status", ACTION.SELECTALL);
                    param.Add("@CompanyIDGet", CompanyID);
                    //var response = await _dapper.QueryAsync<EmployeeTypeDTO>("sp_EmployeeType", param: param, commandType: CommandType.StoredProcedure);
                    var respone = await _dapperr.GetAll<EmployeeTypeDTO>("sp_EmployeeType", param, commandType: CommandType.StoredProcedure);
                    return respone;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<EmployeeTypeDTO> GetEmployeeTypeByID(int CompanyID, int EmployeeTypeID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    int d = (int)GetAllDefault.GetAll;
                    param.Add("@Status", ACTION.SELECTBYID);
                    param.Add("@CompanyIDGet", CompanyID);
                    param.Add("@EmployeeTypeIDGet", EmployeeTypeID);
                    var response = await _dapperr.Get<EmployeeTypeDTO>("sp_EmployeeType", param, commandType: CommandType.StoredProcedure);
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