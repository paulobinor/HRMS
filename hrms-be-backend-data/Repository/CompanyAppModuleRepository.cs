using Com.XpressPayments.Common.ViewModels;
using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.Repository
{
    public class CompanyAppModuleRepository : ICompanyAppModuleRepository
    {
        private string _connectionString;
        private readonly IDapperGenericRepository _repository;
        private readonly ILogger<CompanyAppModuleRepository> _logger;
        private readonly IConfiguration _configuration;
        public CompanyAppModuleRepository(ILogger<CompanyAppModuleRepository> logger, IConfiguration configuration, IDapperGenericRepository repository)
        {
            _logger = logger;
            _repository = repository;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task<List<GetCompanyAppModuleCount>> GetCompanyAppModuleCount()
        {
            try
            {
                string query = @"SELECT c.CompanyId, c.CompanyName, c.Email, c.Created_Date, COUNT(cam.AppModuleId) AS ModuleCount FROM Company AS c
                                 LEFT JOIN CompanyAppModules AS cam ON c.CompanyId = cam.CompanyId where cam.IsApproved = @IsApproved and cam.ISDeleted = @IsDeleted 
                                 GROUP BY c.CompanyId, c.CompanyName, c.Email , c.Created_Date";
                var param = new DynamicParameters();
                param.Add("IsApproved", true);
                param.Add("IsDeleted", false);

                var resp = await _repository.GetAll<GetCompanyAppModuleCount>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateCompanyAppModule => {ex.ToString()}");
                throw;
            }
        }

        public async Task<GetCompanyAppModuleByCompanyDTO> GetCompanyAppModuleByCompanyandModuleID(long companyID ,int moduleID)
        {
            try
            {
                string query = @"Select c.CompanyName, c.Email, am.AppModuleName , am.AppModuleCode , cam.* from CompanyAppModules cam join Company c on cam.CompanyId = c.CompanyId join AppModules am on cam.AppModuleId = am.AppModuleId 
                                    where cam.IsDeleted = @IsDeleted and IsDisapproved = @IsDisapproved and cam.CompanyId = @CompanyId and cam.AppModuleId = @AppModuleId";
                var param = new DynamicParameters();
                param.Add("IsDisapproved", false);
                param.Add("IsDeleted", false);
                param.Add("CompanyId", companyID);
                param.Add("AppModuleId", moduleID);

                var resp = await _repository.Get<GetCompanyAppModuleByCompanyDTO>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateCompanyAppModule => {ex.ToString()}");
                throw;
            }
        }

        public async Task<CompanyAppModuleDTO> GetCompanyAppModuleByID(long companyAppModuleID)
        {
            try
            {
                string query = @"Select c.CompanyName, c.Email, am.AppModuleName , am.AppModuleCode , cam.* from CompanyAppModules cam join Company c on cam.CompanyId = c.CompanyId join AppModules am on cam.AppModuleId = am.AppModuleId 
                                    where cam.IsDeleted = @IsDeleted and IsDisapproved = @IsDisapproved and cam.CompanyAppModuleId = @CompanyAppModuleId";
                var param = new DynamicParameters();
                param.Add("IsDisapproved", false);
                param.Add("IsDeleted", false);
                param.Add("CompanyAppModuleId", companyAppModuleID);

                var resp = await _repository.Get<CompanyAppModuleDTO>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateCompanyAppModule => {ex.ToString()}");
                throw;
            }
        }
        public async Task<List<AppModuleDTO>> GetAllAppModules()
        {
            try
            {
                string query = @" Select * from AppModules";


                var resp = await _repository.GetAll<AppModuleDTO>(query, null, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllAppModules => {ex.ToString()}");
                throw;
            }
        }

        public async Task<AppModuleDTO> GetAppModuleByID(int appMpduleID)
        {
            try
            {
                string query = @" Select * from AppModules where AppModuleId = @AppModuleId";
                var param = new DynamicParameters();
                param.Add("AppModuleId", appMpduleID);
                var resp = await _repository.Get<AppModuleDTO>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAppModuleByID => {ex.ToString()}");
                throw;
            }
        }
        public async Task<List<GetCompanyAppModuleByCompanyDTO>> GetCompanyAppModuleByCompanyID(long companyID)
        {
            try
            {
                string query = @"Select c.CompanyName, c.Email, am.AppModuleName , am.AppModuleCode , cam.* from CompanyAppModules cam join Company c on cam.CompanyId = c.CompanyId join AppModules am on cam.AppModuleId = am.AppModuleId where cam.IsDeleted = @IsDeleted  and cam.CompanyId = @CompanyId";
                var param = new DynamicParameters();
                param.Add("CompanyId", companyID);
                param.Add("IsDeleted", false);

                var resp = await _repository.GetAll<GetCompanyAppModuleByCompanyDTO>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateCompanyAppModule => {ex.ToString()}");
                throw;
            }
        }
        public async Task<List<GetCompanyAppModuleByCompanyDTO>> GetPendingCompanyAppModule()
        {
            try
            {
                string query = @"Select c.CompanyName, c.Email, am.AppModuleName , am.AppModuleCode , cam.* from CompanyAppModules cam join Company c on cam.CompanyId = c.CompanyId join AppModules am on cam.AppModuleId = am.AppModuleId where cam.IsDeleted = @IsDeleted and cam.IsApproved = @IsApproved and cam.IsDisapproved = @IsDisapproved";
                var param = new DynamicParameters();
                param.Add("IsDisapproved", false);
                param.Add("IsApproved", false);
                param.Add("IsDeleted", false);

                var resp = await _repository.GetAll<GetCompanyAppModuleByCompanyDTO>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateCompanyAppModule => {ex.ToString()}");
                throw;
            }
        }

        public async Task<int> CreateCompanyAppModule(CompanyAppModuleDTO companyAppModule)
        {
            try
            {
                string query = @"Insert into CompanyAppModules (CompanyId , AppModuleId , DateCreated , IsActive , CreatedByUserId) values (@CompanyId , @AppModuleId , @DateCreated , @IsActive , @CreatedByUserId) SELECT SCOPE_IDENTITY()";
                var param = new DynamicParameters();
                param.Add("CompanyId", companyAppModule.CompanyId);
                param.Add("AppModuleId", companyAppModule.AppModuleId);
                param.Add("DateCreated", companyAppModule.DateCreated);
                param.Add("IsActive", companyAppModule.IsActive);
                param.Add("CreatedByUserId", companyAppModule.CreatedByUserId);

                var resp = await _repository.Insert<int>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateCompanyAppModule => {ex.ToString()}");
                throw;
            }
        }


        public async Task<int> UpdateCompanyAppModule(CompanyAppModuleDTO companyAppModule)
        {
            try
            {
                string query = @"Update CompanyAppModules set IsActive = @IsActive , IsDeleted = @IsDeleted , DeletedByUserId = @DeletedByUserId where CompanyAppModuleId = @CompanyAppModuleId";
                var param = new DynamicParameters();

                param.Add("IsActive", companyAppModule.IsActive);
                param.Add("IsDeleted", companyAppModule.IsDeleted);
                param.Add("DeletedByUserId", companyAppModule.DeletedByUserId);
                param.Add("CompanyAppModuleId", companyAppModule.CompanyAppModuleId);

                var resp = await _repository.Update<int>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateCompanyAppModule => {ex.ToString()}");
                throw;
            }
        }


        public async Task<int> ApproveCompanyAppModule(CompanyAppModuleDTO companyAppModule)
        {
            try
            {
                string query = @"Update CompanyAppModules set IsActive = @IsActive , IsApproved = @IsApproved , ApprovedByUserId = @ApprovedByUserId , DateApproved = @DateApproved where CompanyAppModuleId = @CompanyAppModuleId";
                var param = new DynamicParameters();

                param.Add("IsActive", companyAppModule.IsActive);
                param.Add("IsApproved", companyAppModule.IsApproved);
                param.Add("ApprovedByUserId", companyAppModule.ApprovedByUserId);
                param.Add("DateApproved", companyAppModule.DateApproved);
                param.Add("CompanyAppModuleId", companyAppModule.CompanyAppModuleId);

                var resp = await _repository.Update<int>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: ApproveCompanyAppModule => {ex.ToString()}");
                throw;
            }
        }

        public async Task<int> DisapproveCompanyAppModule(CompanyAppModuleDTO companyAppModule)
        {
            try
            {
                string query = @"Update CompanyAppModules set IsActive = @IsActive , IsDisapproved = @IsDisapproved , DisapprovedByUserId = @DisapprovedByUserId , DateApproved = @DateApproved where CompanyAppModuleId = @CompanyAppModuleId";
                var param = new DynamicParameters();

                param.Add("IsActive", companyAppModule.IsActive);
                param.Add("IsDisapproved", companyAppModule.IsDisapproved);
                param.Add("DisapprovedByUserId", companyAppModule.DisapprovedByUserId);
                param.Add("DateApproved", companyAppModule.DateApproved);
                param.Add("CompanyAppModuleId", companyAppModule.CompanyAppModuleId);

                var resp = await _repository.Update<int>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DisapproveCompanyAppModule => {ex.ToString()}");
                throw;
            }
        }
    }
}
