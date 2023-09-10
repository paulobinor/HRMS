using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace hrms_be_backend_data.Repository
{
    public class ResignationClearanceRepository : IResignationClearanceRepository
    {
        private string _connectionString;
        private readonly IDapperGenericRepository _repository;
        private readonly ILogger<IResignationClearanceRepository> _logger;
        private readonly IConfiguration _configuration;
        public ResignationClearanceRepository(ILogger<ResignationClearanceRepository> logger, IConfiguration configuration, IDapperGenericRepository repository)
        {
            _logger = logger;
            _repository = repository;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<int> CreateResignationClearance(ResignationClearanceDTO resignation)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("UserID", resignation.UserID);
                param.Add("SRFID", resignation.SRFID);
                param.Add("LastName", resignation.LastName);
                param.Add("FirstName", resignation.FirstName);
                param.Add("MiddleName", resignation.MiddleName);
                param.Add("ReasonForResignation", resignation.ReasonForResignation);
                param.Add("ItemsReturnedToDepartment", resignation.ItemsReturnedToDepartment);
                param.Add("ItemsReturnedToAdmin", resignation.ItemsReturnedToAdmin);
                param.Add("Collateral", resignation.Collateral);
                param.Add("ItemsReturnedToHR", resignation.ItemsReturnedToHR);
                //param.Add("Loans", resignation.Loans);
                param.Add("LastDayOfWork", resignation.LastDayOfWork);

                await _repository.Insert<int>("Sp_SubmitResignation", param, commandType: CommandType.StoredProcedure);

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
    }
}
