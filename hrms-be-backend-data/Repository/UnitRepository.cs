using Dapper;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace hrms_be_backend_data.Repository
{
    public class UnitRepository : IUnitRepository
    {     
        private readonly ILogger<UnitRepository> _logger;
        private readonly IDapperGenericRepository _dapper;

        public UnitRepository(IConfiguration configuration, ILogger<UnitRepository> logger, IDapperGenericRepository dapper)
        {          
            _logger = logger;           
            _dapper = dapper;
        }

        public async Task<string> ProcessUnit(ProcessUnitReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@UnitId", payload.UnitId);
                param.Add("@UnitName", payload.UnitName);
                param.Add("@DepartmentId", payload.DepartmentId);
                param.Add("@UnitHeadEmployeeId", payload.UnitHeadEmployeeId);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                param.Add("@IsModifield", payload.IsModifield);

                return await _dapper.Get<string>("sp_process_unit", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"UnitRepository -> ProcessUnit => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<string> DeleteUnit(DeleteUnitReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@UnitId", payload.UnitId);
                param.Add("@DeletedComment", payload.Comment);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                return await _dapper.Get<string>("sp_delete_unit", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyRepository -> DeleteUnit => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<UnitWithTotalVm> GetUnites(long CompanyId, int PageNumber, int RowsOfPage)
        {
            var returnData = new UnitWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@PageNumber", PageNumber);
                param.Add("@CompanyId", CompanyId);
                var result = await _dapper.GetMultiple("sp_get_units", param, gr => gr.Read<long>(), gr => gr.Read<UnitVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<UnitVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"UnitRepository -> GetUnites => {ex}");
                return returnData;
            }

        }
        public async Task<UnitWithTotalVm> GetUnitesDeleted(long CompanyId, int PageNumber, int RowsOfPage)
        {
            var returnData = new UnitWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@PageNumber", PageNumber);
                param.Add("@CompanyId", CompanyId);
                var result = await _dapper.GetMultiple("sp_get_units_deleted", param, gr => gr.Read<long>(), gr => gr.Read<UnitVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<UnitVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"UnitRepository -> GetUnitesDeleted => {ex}");
                return returnData;
            }

        }
        public async Task<UnitVm> GetUnitById(long Id)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                return await _dapper.Get<UnitVm>("sp_get_unit_by_id", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"UnitRepository -> GetUnitById => {ex}");
                return new UnitVm();
            }
        }
        public async Task<UnitVm> GetUnitByName(string UnitName, long CompanyId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UnitName", UnitName);
                param.Add("@CompanyId", CompanyId);
                return await _dapper.Get<UnitVm>("sp_get_unit_by_name", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"UnitRepository -> GetUnitByName => {ex}");
                return new UnitVm();
            }
        }
    }
}
