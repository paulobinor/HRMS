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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ILogger<DepartmentRepository> _logger;
        private readonly IDapperGeneric _dapperr;
        private readonly string _connectionString;


        public DepartmentRepository(ILogger<DepartmentRepository> logger, IConfiguration configuration, IDapperGeneric dapper)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("HRMSConnectionString");
            _dapperr = dapper;

        }

        public async Task<int> CreateDepartment(CreateDepartmentDTO payload)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                { 
                       var param = new DynamicParameters();
                        param.Add("@Status", ACTION.INSERT);
                        param.Add("@DepartmentName", payload.DepartmentName);
                        param.Add("@HODEmployeeID", payload.HODEmployeeID);
                        param.Add("@isActive", true);
                        param.Add("@CompanyID", payload.CompanyID);
                    dynamic response = await _dapper.ExecuteAsync("Sp_Department", param: param, commandType: CommandType.StoredProcedure);
                    return response;

                }




            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }
        public async Task<int> DeleteDepartment(int DepartmentID, int CompanyID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DELETE);
                    param.Add("@DepartmentID", DepartmentID);
                    param.Add("@CompanyID", CompanyID);
                    dynamic response = await _dapper.ExecuteAsync("Sp_Department", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
                


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }

        public async Task<int> DisableDepartment(int DepartmentID, int CompanyID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DELETE);
                    param.Add("@DepartmentID", DepartmentID);
                    param.Add("@CompanyID", CompanyID);
                    dynamic response = await _dapper.ExecuteAsync("Sp_Department", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
              

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }

        public async Task<int> ActivateDepartment(int DepartmentID, int CompanyID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.DELETE);
                    param.Add("@DepartmentID", DepartmentID);
                    param.Add("@CompanyID", CompanyID);
                    dynamic response = await _dapper.ExecuteAsync("Sp_Department", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
              

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }

        public async Task<int> UpdateDepartment(UpdateDepartmentDTO payload)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.UPDATE);
                    param.Add("@DepartmentName", payload.DepartmentName);
                    param.Add("@HODEmployeeID", payload.HODEmployeeID);
                    param.Add("@DepartmentID", payload.DepartmentID);
                    param.Add("@CompanyID", payload.CompanyID);

                    int response = await _dapper.ExecuteAsync("Sp_Department", param: param, commandType: CommandType.StoredProcedure);
                    return response;
                }
              


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }

        }
        public async Task<List<GetDepartmentDTO>> GetAllDepartment(int CompanyID)
        {
            try
            {
                
                    var param = new DynamicParameters();
                int d = (int)GetAllDefault.GetAll;
                param.Add("@Status", ACTION.SELECTALL);
                    param.Add("@CompanyID", CompanyID);
                   // var response = await _dapper.QueryAsync<GetDepartmentDTO>("Sp_Department", param: param, commandType: CommandType.StoredProcedure);
                    return await _dapperr.GetAll<GetDepartmentDTO>("Sp_Department", param, commandType: CommandType.StoredProcedure);
                    //return responeD;
                
               

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }

        }

        public async Task<IEnumerable<GetDepartmentDTO>> GetAllDepartmentByID(int DepartmentID, int CompanyID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", ACTION.SELECTBYID);
                    param.Add("@DepartmentID", DepartmentID);
                    param.Add("@CompanyID", CompanyID);
                    var response = await _dapper.QueryAsync<GetDepartmentDTO>("Sp_Department", param: param, commandType: CommandType.StoredProcedure);
                    return response.ToList();

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
