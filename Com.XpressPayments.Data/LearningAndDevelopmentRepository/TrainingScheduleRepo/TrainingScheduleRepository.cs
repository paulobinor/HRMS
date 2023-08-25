using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DapperGeneric;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.LearningAndDevelopmentDTO.DTOs;
using Com.XpressPayments.Data.LearningAndDevelopmentRepository.TrainingPlanRepo;
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

namespace Com.XpressPayments.Data.LearningAndDevelopmentRepository.TrainingScheduleRepo
{
    internal class TrainingScheduleRepository : ITrainingScheduleRepository
    {
        private string _connectionString;
        private readonly ILogger<TrainingScheduleRepository> _logger;
        private readonly IDapperGenericRepository _dapperGeneric;
        private readonly IConfiguration _configuration;

        public TrainingScheduleRepository(IConfiguration configuration, ILogger<TrainingScheduleRepository> logger, IDapperGenericRepository dapperGeneric)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
            _dapperGeneric = dapperGeneric;
        }
        public async Task<string> CreateTrainingSchedule(TrainingScheduleCreate TrainingSchedule)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", TrainingScheduleEnum.CREATE);
                param.Add("@UserId", TrainingSchedule.UserId);
                param.Add("@StaffName", TrainingSchedule.StaffName);
                param.Add("@Department", TrainingSchedule.Department);
                param.Add("@TrainingTopic", TrainingSchedule.TrainingTopic);
                param.Add("@TrainingTime", TrainingSchedule.TrainingTime);
                param.Add("@TrainingOrganizer", TrainingSchedule.TrainingOrganizer);
                param.Add("@TrainingMode", TrainingSchedule.TrainingMode);
                param.Add("@TrainingVenue", TrainingSchedule.TrainingVenue);
                param.Add("@StartDate", TrainingSchedule.StartDate);
                param.Add("@EndDate", TrainingSchedule.EndDate);
                param.Add("@Created_By_User_Email", TrainingSchedule.Created_By_User_Email.Trim());

                return await _dapperGeneric.Get<string>(ApplicationConstant.Sp_TrainingSchedule, param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: CreateTrainingSchedule ===>{ex}");
                throw;
            }
        }

        public async Task<dynamic> DeleteTrainingSchedule(TrainingScheduleDelete delete, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingScheduleEnum.DELETE);
                    param.Add("@TrainingScheduleIDDelete", Convert.ToInt32(delete.TrainingScheduleID));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Delete", delete.Reasons_For_Delete == null ? "" : delete.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_TrainingSchedule, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: Task<dynamic> DeleteTrainingSchedule(TrainingScheduleDelete delete, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TrainingScheduleDTO>> GetAllTrainingSchedule()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingScheduleEnum.GETALL);

                    var TrainingSchedules = await _dapper.QueryAsync<TrainingScheduleDTO>(ApplicationConstant.Sp_TrainingSchedule, param: param, commandType: CommandType.StoredProcedure);

                    return TrainingSchedules;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: GetAllTrainingSchedule() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TrainingScheduleDTO>> GetTrainingScheduleByCompany(long companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingScheduleEnum.GETBYCOMPANYID);
                    param.Add("@CompanyIdGet", companyId);

                    var TrainingScheduleDetails = await _dapper.QueryAsync<TrainingScheduleDTO>(ApplicationConstant.Sp_TrainingSchedule, param: param, commandType: CommandType.StoredProcedure);

                    return TrainingScheduleDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetTrainingScheduleByCompany(int companyId) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<TrainingScheduleDTO> GetTrainingScheduleById(long TrainingScheduleID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", TrainingScheduleEnum.GETBYID);
                    param.Add("@TrainingScheduleIDGet", TrainingScheduleID);


                    var TrainingScheduleDetails = await _dapper.QueryFirstOrDefaultAsync<TrainingScheduleDTO>(ApplicationConstant.Sp_TrainingSchedule, param: param, commandType: CommandType.StoredProcedure);

                    return TrainingScheduleDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: Task<TrainingScheuleDTO> GetTrainingScheduleById(long TrainingScheduletID) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<string> ApproveTrainingSchedule(long TrainingScheduleID, long ApprovedByUserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", 10);
                param.Add("@TrainingPlanID", TrainingScheduleID);
                param.Add("@ApprovedByUserId", ApprovedByUserId);
                param.Add("@DateApproved", DateTime.Now);
                return await _dapperGeneric.Get<string>(ApplicationConstant.Sp_TrainingSchedule, param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: ApproveTrainingSchedule ===>{ex}");
                throw;
            }
        }

        public async Task<string> DisaproveTrainingSchedule(long TrainingScheduleID, long DisapprovedByUserId, string DisapprovedComment)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Status", 11);
                param.Add("@TrainingPlanID", TrainingScheduleID);
                param.Add("@DisapprovedByUserId", DisapprovedByUserId);
                param.Add("@DisapprovedComment", DisapprovedComment);
                param.Add("@DateDisapproved", DateTime.Now);
                return await _dapperGeneric.Get<string>(ApplicationConstant.Sp_TrainingSchedule, param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"MethodName: DisaproveTrainingPlan ===>{ex}");
                throw;
            }
        }
    }
}
