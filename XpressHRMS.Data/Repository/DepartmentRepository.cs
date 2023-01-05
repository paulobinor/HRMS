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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ILogger<DepartmentRepository> _logger;
        private readonly IDapperGeneric _dapper;

        public DepartmentRepository(ILogger<DepartmentRepository> logger)
        {
            _logger = logger;

        }

        public async Task<int> CreateDepartment(DepartmentDTO payload)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", ACTION.INSERT);
                param.Add("@DepartmentName", payload.DepartmentName);
                param.Add("@HODEmployeeID", payload.HODEmployeeID);
                param.Add("@CreatedByUserID", payload.CreatedByUserID);
                param.Add("@isActive", true);
                param.Add("@CompanyID", payload.CompanyID);
                return await _dapper.Get<int>("", param, commandType: CommandType.StoredProcedure);


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
                var param = new DynamicParameters();
                param.Add("@Status", ACTION.DELETE);
                param.Add("@DepartmentID", DepartmentID);
                param.Add("@CompanyID", CompanyID);
                return await _dapper.Get<int>("", param, commandType: CommandType.StoredProcedure);


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
                var param = new DynamicParameters();
                param.Add("@Status", ACTION.DELETE);
                param.Add("@DepartmentID", DepartmentID);
                param.Add("@CompanyID", CompanyID);
                return await _dapper.Get<int>("", param, commandType: CommandType.StoredProcedure);

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
                var param = new DynamicParameters();
                param.Add("@Status", ACTION.DELETE);
                param.Add("@DepartmentID", DepartmentID);
                param.Add("@CompanyID", CompanyID);
                return await _dapper.Get<int>("", param, commandType: CommandType.StoredProcedure);

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
                var param = new DynamicParameters();
                param.Add("@Status", ACTION.UPDATE);
                param.Add("@DepartmentName", payload.DepartmentName);
                param.Add("@HODEmployeeID", payload.HODEmployeeID);
                param.Add("@DepartmentID", payload.DepartmentID);
                param.Add("@CompanyID", payload.CompanyID);
                return await _dapper.Get<int>("", param, commandType: CommandType.StoredProcedure);


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
                param.Add("@Status", ACTION.SELECTALL);
                param.Add("@CompanyID", CompanyID);
                return await _dapper.GetAll<GetDepartmentDTO>("", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }

        }

        public async Task<List<GetDepartmentDTO>> GetAllDepartmentByID(int DepartmentID, int CompanyID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", ACTION.SELECTBYID);
                param.Add("@DepartmentID", DepartmentID);
                param.Add("@CompanyID", CompanyID);
                return await _dapper.GetAll<GetDepartmentDTO>("", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }

        }


    }
}
