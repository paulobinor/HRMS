using Com.XpressPayments.Common.ViewModels;
using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace hrms_be_backend_data.Repository
{
    public class ResignationInterviewRepository : IResignationInterviewRepository
    {
        private string _connectionString;
        private readonly IDapperGenericRepository _dapper;
        private readonly ILogger<ResignationInterviewRepository> _logger;
        private readonly IConfiguration _configuration;
        public ResignationInterviewRepository(ILogger<ResignationInterviewRepository> logger, IConfiguration configuration, IDapperGenericRepository repository)
        {
            _logger = logger;
            _dapper = repository;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<dynamic> CreateResignationInterview(ResignationInterviewDTO resignation, DataTable sectionOne, DataTable sectionTwo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("EmployeeID", resignation.EmployeeId);
                param.Add("CompanyID", resignation.CompanyId);
                param.Add("@ResignationID", resignation.ResignationId);
                param.Add("DateCreated", resignation.DateCreated);
                param.Add("ResumptionDate", resignation.ResumptionDate);
                param.Add("ExitDate", resignation.ExitDate);
                param.Add("CreatedByUserId", resignation.CreatedByUserId);
                //param.Add("ReasonForResignation", resignation.ReasonForResignation);
                param.Add("DateCreated", resignation.DateCreated);
                param.Add("OtherRemarks", resignation.OtherRemarks);
                param.Add("Date", resignation.Date);
                param.Add("Signature", resignation.Signature);
                param.Add("WhatDidYouLikeMostAboutTheCompanyAndYourJob", resignation.WhatDidYouLikeMostAboutTheCompanyAndYourJob);
                param.Add("WhatDidYouLeastLikeAboutTheCompanyAndYourJob", resignation.WhatDidYouLeastLikeAboutTheCompanyAndYourJob);
                param.Add("DoYouFeelYouWerePlacedInAPositionCompatibleWithYourSkillSet", resignation.DoYouFeelYouWerePlacedInAPositionCompatibleWithYourSkillSet);
                param.Add("IfYouAreTakingAnotherJob_WhatKindOfJobWillYouBeTaking", resignation.IfYouAreTakingAnotherJob_WhatKindOfJobWillYouBeTaking);
                param.Add("CouldOurCompanyHaveMadeAnyImprovementsThatMightHaveMadeYouStay", resignation.CouldOurCompanyHaveMadeAnyImprovementsThatMightHaveMadeYouStay);
                //param.Add("OfficialEmail", resignation.OfficialEmail);
                //param.Add("HrEmployeeId", resignation.HrEmployeeId);
                param.Add("SectionOne", sectionOne.AsTableValuedParameter("InterviewDetailsSectionType"));
                param.Add("SectionTwo", sectionTwo.AsTableValuedParameter("InterviewDetailsSectionType"));

                var resp = await _dapper.BulkInsert<dynamic>(param, "Sp_SubmitResignationInterview");

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateResignationInterview(ResignationInterviewDTO resignation) => {ex.Message}");
                throw;
            }
        }

        public async Task<ResignationInterviewDTO> GetResignationInterviewById(long ResignationInterviewID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("ResignationInterviewID", ResignationInterviewID);

                var response = await _dapper.Get<ResignationInterviewDTO>("Sp_get_resignation_interview", param, commandType: CommandType.StoredProcedure);

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Resignation by ID - {ResignationInterviewID}", ex);
                throw;
            }
        }
        public async Task<List<InterviewScaleValue>> GetResignationInterviewDetails(long InterviewID)
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("InterviewID", InterviewID);

                var response = (await _dapper.GetAll<InterviewScaleValue>("Sp_get_resignation_interview_details", param, commandType: CommandType.StoredProcedure));

                return response;

            }
            catch (Exception ex)
            {

                _logger.LogError($"Error Getting Resignation by ID - {InterviewID}", ex);
                throw;
            }
        }

        public async Task<IEnumerable<ResignationInterviewDTO>> GetAllResignationInterviewsByCompany(long companyID, int PageNumber, int RowsOfPage, string SearchVal, DateTime? startDate, DateTime? endDate)
        {
            try
            {

                {
                    var param = new DynamicParameters();
                    param.Add("CompanyID", companyID);
                    param.Add("@PageNumber", PageNumber);
                    param.Add("@RowsOfPage", RowsOfPage);
                    param.Add("@StartDate", startDate);
                    param.Add("@EndDate", endDate);
                    param.Add("@SearchVal", SearchVal.ToLower());


                    var response = await _dapper.GetAll<ResignationInterviewDTO>("Sp_get_all_resignation_interview", param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Resignation Interviews", ex);
                throw;
            }
        }

        public async Task<ResignationInterviewDTO> GetResignationInterviewByEmployeeID(long employeeID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("employeeID", employeeID);

                var response = await _dapper.Get<ResignationInterviewDTO>("Sp_get_resignation_interview_by_user", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Resignation by UserID - {employeeID}", ex);
                throw;
            }
        }


        public async Task<IEnumerable<InterviewScaleDetailsDTO>> GetInterviewScaleDetails()
        {
            try
            {
                var resp = await _dapper.GetAll<InterviewScaleDetailsDTO>("Sp_get_resignation_interview_rate_scale_details", null, CommandType.Text);
                return resp;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting ResignationInterview", ex);
                throw;
            }
        }

        //public async Task<dynamic> ApprovePendingResignationInterview(long userID, long ID, bool isApproved)
        //{
        //    try
        //    {
        //        var param = new DynamicParameters();
        //        param.Add("ApprovedByUserId", userID);
        //        param.Add("ID", ID);
        //        param.Add("isApproved", isApproved);
        //        param.Add("DateApproved", DateTime.Now);
        //        param.Add("Resp", dbType: DbType.Int32, direction: ParameterDirection.Output);

        //        var response = await _repository.Execute<int>("Sp_ApprovePendingResignationInterview", param, commandType: CommandType.StoredProcedure);
        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        var err = ex.Message;
        //        _logger.LogError($"MethodName: ApprovePendingResignationAsync(long userID, long ID) => {ex.Message}");
        //        throw;
        //    }
        //}

        //public async Task<dynamic> DisapprovePendingResignationInterview(long userID, long ID, bool isDisapproved, string DisapprovedComment)
        //{
        //    try
        //    {
        //        var param = new DynamicParameters();
        //        param.Add("DisapprovedByUserId", userID);
        //        param.Add("ID", ID);
        //        param.Add("isDisapproved", isDisapproved);
        //        param.Add("DisapprovedComment", DisapprovedComment);
        //        param.Add("DateDisapproved", DateTime.Now);
        //        param.Add("Resp", dbType: DbType.Int32, direction: ParameterDirection.Output);

        //        var response = await _repository.Execute<int>("Sp_DisapprovePendingResignationIntyerview", param, commandType: CommandType.StoredProcedure);
        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        var err = ex.Message;
        //        _logger.LogError($"MethodName: DisapprovePendingResignationInterview(long userID, long ID) => {ex.Message}");
        //        throw;
        //    }
        //}

    }
}
