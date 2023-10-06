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
        private readonly IDapperGenericRepository _repository;
        private readonly ILogger<ResignationInterviewRepository> _logger;
        private readonly IConfiguration _configuration;
        public ResignationInterviewRepository(ILogger<ResignationInterviewRepository> logger, IConfiguration configuration, IDapperGenericRepository repository)
        {
            _logger = logger;
            _repository = repository;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<int> CreateResignationInterview(ResignationInterviewDTO resignation, DataTable sectionOne, DataTable sectionTwo)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("UserId", resignation.UserID);
                param.Add("SRFID", resignation.SRFID);
                param.Add("FirstName", resignation.FirstName);
                param.Add("LastName", resignation.LastName);
                param.Add("MiddleName", resignation.MiddleName);
                param.Add("DateCreated", resignation.DateCreated);
                param.Add("LastDayOfWork", resignation.LastDayOfWork);
                param.Add("Created_By_User_Email", resignation.Created_By_User_Email);
                param.Add("Status", 0);//Create enum for status one approval stages is known
                param.Add("ReasonForResignation", resignation.ReasonForResignation);
                param.Add("DateCreated", resignation.DateCreated);
                param.Add("QuestionOne", resignation.QuestionOne);
                param.Add("QuestionTwo", resignation.QuestionTwo);
                param.Add("QuestionThree", resignation.QuestionThree);
                param.Add("QuestionFour", resignation.QuestionFour);
                param.Add("QuestionFive", resignation.QuestionFive);
                param.Add("Comment", resignation.Comment);
                param.Add("SectionOne", sectionOne.AsTableValuedParameter("InterviewDetailsSectionType"));
                param.Add("SectionTwo", sectionTwo.AsTableValuedParameter("InterviewDetailsSectionType"));
                param.Add("Resp", dbType: DbType.Int32, direction: ParameterDirection.Output);

                //int response =  _dapper.ExecuteReader("Sp_SubmitResignation", param: param, commandType: CommandType.StoredProcedure);

                await _repository.BulkInsert<int>(param, "Sp_SubmitResignationInterview");

                int resp = param.Get<int>("Resp");

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateResignationInterview(ResignationInterviewDTO resignation) => {ex.Message}");
                throw;
            }
        }

        public async Task<List<InterviewScaleDetailsDTO>> GetInterviewScaleDetails()
        {
            try
            {
                string sqlQuery = @"select * from InterviewRateScaleDetails";
                var resp = await _repository.GetAll<InterviewScaleDetailsDTO>(sqlQuery, null, CommandType.Text);
                return resp;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting ResignationInterview", ex);
                throw;
            }
        }

        public async Task<ResignationInterviewDTO> GetResignationInterview(long SRFID)
        {
            try
            {

                string query = "select * from ResignationInterview where SRFID = @SRFID";
                var param = new DynamicParameters();
                param.Add("SRFID", SRFID);

                var response = (await _repository.Get<ResignationInterviewDTO>(query, param, commandType: CommandType.Text));

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Resignation by ID - {SRFID}", ex);
                throw;
            }
        }
        public async Task<List<InterviewScaleValue>> GetResignationInterviewDetails(long InterviewID)
        {
            try
            {

                string query = "select IRSV.*,IRSD.Name,IRSD.Section from InterviewRateScaleValue IRSV JOIN InterviewRateScaleDetails IRSD ON IRSV.ResignationDetailID = IRSD.ID WHERE IRSV.InterviewID = 1 ORDER BY IRSD.ID , IRSD.Section";
                var param = new DynamicParameters();
                param.Add("SRFID", InterviewID);

                var response = (await _repository.GetAll<InterviewScaleValue>(query, param, commandType: CommandType.Text));

                return response;

            }
            catch (Exception ex)
            {

                _logger.LogError($"Error Getting Resignation by ID - {InterviewID}", ex);
                throw;
            }
        }

        public async Task<int> ApprovePendingResignationInterview(long userID, long ID, bool isApproved)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("ApprovedByUserId", userID);
                param.Add("ID", ID);
                param.Add("isApproved", isApproved);
                param.Add("DateApproved", DateTime.Now);
                param.Add("Resp", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var response = await _repository.Execute<int>("Sp_ApprovePendingResignationInterview", param, commandType: CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: ApprovePendingResignationAsync(long userID, long ID) => {ex.Message}");
                throw;
            }
        }

        public async Task<int> DisapprovePendingResignationInterview(long userID, long ID, bool isDisapproved, string DisapprovedComment)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("DisapprovedByUserId", userID);
                param.Add("ID", ID);
                param.Add("isDisapproved", isDisapproved);
                param.Add("DisapprovedComment", DisapprovedComment);
                param.Add("DateDisapproved", DateTime.Now);
                param.Add("Resp", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var response = await _repository.Execute<int>("Sp_DisapprovePendingResignationIntyerview", param, commandType: CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DisapprovePendingResignationInterview(long userID, long ID) => {ex.Message}");
                throw;
            }
        }
        public async Task<List<ResignationInterviewDTO>> GetAllApprovedResignationInterview(long UserID, bool isApproved)
        {
            try
            {

                {
                    string query = "Select * from ResignationInterview where IsApproved = @IsApproved";
                    var param = new DynamicParameters();
                    param.Add("UserID", UserID);
                    param.Add("ISApproved", isApproved);

                    var response = (await _repository.GetAll<ResignationInterviewDTO>(query, param, commandType: CommandType.Text)).ToList();

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Approved Resignation Interview Company - {UserID}", ex);
                throw;
            }
        }
    }
}
