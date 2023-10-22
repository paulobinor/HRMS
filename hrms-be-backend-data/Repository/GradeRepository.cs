using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace hrms_be_backend_data.Repository
{
    public class GradeRepository : IGradeRepository
    {
        private readonly ILogger<GradeRepository> _logger;
        private readonly IDapperGenericRepository _dapper;

        public GradeRepository(IConfiguration configuration, ILogger<GradeRepository> logger, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _dapper = dapper;
        }

        public async Task<string> ProcessGrade(ProcessGradeReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@GradeId", payload.GradeId);
                param.Add("@GradeName", payload.GradeName);              
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                param.Add("@IsModifield", payload.IsModifield);

                return await _dapper.Get<string>("sp_process_grade", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"GradeRepository -> ProcessGrade => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<string> DeleteGrade(DeleteGradeReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@GradeId", payload.GradeId);
                param.Add("@DeletedComment", payload.Comment);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                return await _dapper.Get<string>("sp_delete_grade", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyRepository -> DeleteGrade => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<GradeWithTotalVm> GetGrades(long CompanyId, int PageNumber, int RowsOfPage)
        {
            var returnData = new GradeWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@PageNumber", PageNumber);
                param.Add("@CompanyId", CompanyId);
                var result = await _dapper.GetMultiple("sp_get_grades", param, gr => gr.Read<long>(), gr => gr.Read<GradeVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<GradeVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"GradeRepository -> GetGradees => {ex}");
                return returnData;
            }

        }
        public async Task<GradeWithTotalVm> GetGradesDeleted(long CompanyId, int PageNumber, int RowsOfPage)
        {
            var returnData = new GradeWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@PageNumber", PageNumber);
                param.Add("@CompanyId", CompanyId);
                var result = await _dapper.GetMultiple("sp_get_grades_deleted", param, gr => gr.Read<long>(), gr => gr.Read<GradeVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<GradeVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"GradeRepository -> GetGradeesDeleted => {ex}");
                return returnData;
            }

        }
        public async Task<GradeVm> GetGradeById(long Id)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                return await _dapper.Get<GradeVm>("sp_get_grade_by_id", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GradeRepository -> GetGradeById => {ex}");
                return new GradeVm();
            }
        }
        public async Task<GradeVm> GetGradeByName(string GradeName, long CompanyId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@GradeName", GradeName);
                param.Add("@CompanyId", CompanyId);
                return await _dapper.Get<GradeVm>("sp_get_grade_by_name", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GradeRepository -> GetGradeByName => {ex}");
                return new GradeVm();
            }
        }
    }
}
