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
    public class DepartmentalModulesRepository : IDepartmentalModulesRepository
    {
        private string _connectionString;
        private readonly IDapperGenericRepository _repository;
        private readonly ILogger<DepartmentalModulesRepository> _logger;
        private readonly IConfiguration _configuration;
        public DepartmentalModulesRepository(ILogger<DepartmentalModulesRepository> logger, IConfiguration configuration, IDapperGenericRepository repository)
        {
            _logger = logger;
            _repository = repository;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task<List<GetDepartmentalModuleCount>> GetDepartmentalAppModuleCount(long companyID)
        {
            try
            {
                string query = @"Select d.DepartmentId, c.CompanyName , d.DepartmentName , c.Email , d.DateCreated , Count(dm.AppModuleId) as ModuleCount from Department D Left Join DepartmentalModules dm On d.DepartmentId = dm.DepartmentID
								 join Company c on d.CompanyId = c.CompanyId where dm.IsApproved = @IsApproved and dm.ISDeleted = @IsDeleted and c.CompanyId = @CompanyID
								 GROUP BY d.DepartmentId, c.CompanyName , d.DepartmentName , c.Email , d.DateCreated";
                var param = new DynamicParameters();
                param.Add("IsApproved", true);
                param.Add("IsDeleted", false);
                param.Add("CompanyID", companyID);

                var resp = await _repository.GetAll<GetDepartmentalModuleCount>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetDepartmentalAppModuleCount => {ex.ToString()}");
                throw;
            }
        }


        public async Task<List<GetDepartmentalModuleCount>> GetDisapprovedDepartmentalAppModuleCount(long companyID)
        {
            try
            {
                string query = @"Select d.DepartmentId, c.CompanyName , d.DepartmentName , c.Email , d.DateCreated , Count(dm.AppModuleId) as ModuleCount from Department D Left Join DepartmentalModules dm On d.DepartmentId = dm.DepartmentID
								 join Company c on d.CompanyId = c.CompanyId where dm.IsDisapproved = @IsDisapproved and dm.ISDeleted = @IsDeleted and c.CompanyId = @CompanyID
								 GROUP BY d.DepartmentId, c.CompanyName , d.DepartmentName , c.Email , d.DateCreated";
                var param = new DynamicParameters();
                param.Add("IsDisapproved", true);
                param.Add("IsDeleted", false);
                param.Add("CompanyID", companyID);

                var resp = await _repository.GetAll<GetDepartmentalModuleCount>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetDisapprovedDepartmentalAppModuleCount => {ex.ToString()}");
                throw;
            }
        }

        public async Task<List<GetDepartmentalModuleCount>> GetAllDepartmentalAppModuleCount(long companyID)
        {
            try
            {
                string query = @"Select d.DepartmentId, c.CompanyName , d.DepartmentName , c.Email , d.DateCreated , Count(dm.AppModuleId) as ModuleCount from Department D Left Join DepartmentalModules dm On d.DepartmentId = dm.DepartmentID
								 join Company c on d.CompanyId = c.CompanyId where dm.ISDeleted = @IsDeleted and c.CompanyId = @CompanyId
								 GROUP BY d.DepartmentId, c.CompanyName , d.DepartmentName , c.Email , d.DateCreated";
                var param = new DynamicParameters();
                param.Add("IsDeleted", false);
                param.Add("CompanyID", companyID);

                var resp = await _repository.GetAll<GetDepartmentalModuleCount>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllDepartmentalAppModuleCount => {ex.ToString()}");
                throw;
            }
        }

        public async Task<GetDepartmentModuleByDepartmentDTO> GetDepartmentalAppModuleByDepartmentandModuleID(long departmentID, int moduleID)
        {
            try
            {
                string query = @"Select d.DepartmentName, am.AppModuleName , am.AppModuleCode , dm.* from DepartmentalModules dm join Department d on dm.DepartmentID = d.DepartmentId join AppModules am on dm.AppModuleId = am.AppModuleId 
                                    where dm.IsDeleted = @IsDeleted and dm.IsDisapproved = @IsDisapproved and dm.DepartmentId = @DepartmentId and dm.AppModuleId = @AppModuleId";
                var param = new DynamicParameters();
                param.Add("IsDisapproved", false);
                param.Add("IsDeleted", false);
                param.Add("DepartmentId", departmentID);
                param.Add("AppModuleId", moduleID);

                var resp = await _repository.Get<GetDepartmentModuleByDepartmentDTO>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetDepartmentalAppModuleByDepartmentandModuleID => {ex.ToString()}");
                throw;
            }
        }

        public async Task<DepartmentalModulesDTO> GetDepartmentalAppModuleByID(long departmentalAppModuleID)
        {
            try
            {
                string query = @"Select d.DepartmentName, am.AppModuleName , am.AppModuleCode , dm.* from DepartmentalModules dm join Department d on dm.DepartmentID = d.DeptId join AppModules am on dm.AppModuleId = am.AppModuleId 
                                    where dm.IsDeleted = @IsDeleted and dm.IsDisapproved = @IsDisapproved and dm.DeparmentalModuleID = @DeparmentalModuleID";
                var param = new DynamicParameters();
                param.Add("IsDisapproved", false);
                param.Add("IsDeleted", false);
                param.Add("DeparmentalModuleID", departmentalAppModuleID);

                var resp = await _repository.Get<DepartmentalModulesDTO>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetDepartmentalAppModuleByID => {ex.ToString()}");
                throw;
            }
        }
       

        public async Task<List<GetDepartmentModuleByDepartmentDTO>> GetDepartmentalAppModuleByDepartmentID(long departmentID)
        {
            try
            {
                string query = @"Select d.DepartmentName, am.AppModuleName , am.AppModuleCode , dm.* from DepartmentalModules dm join Department d on dm.DepartmentID = d.DeptId join AppModules am on dm.AppModuleId = am.AppModuleId 
                                    where dm.IsDeleted = @IsDeleted and dm.DepartmentID = @DepartmentID";
                var param = new DynamicParameters();
                param.Add("DepartmentID", departmentID);
                param.Add("IsDeleted", false);

                var resp = await _repository.GetAll<GetDepartmentModuleByDepartmentDTO>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetDepartmentalAppModuleByDepartmentID => {ex.ToString()}");
                throw;
            }
        }
        public async Task<List<GetDepartmentModuleByDepartmentDTO>> GetPendingDepartmentalAppModule(long companyID)
        {
            try
            {
                string query = @"Select d.DepartmentName, am.AppModuleName , am.AppModuleCode , dm.* from DepartmentalModules dm join Department d on dm.DepartmentID = d.DepartmentId join AppModules am on dm.AppModuleId = am.AppModuleId 
                                    where dm.IsDeleted = @IsDeleted and dm.DepartmentID = @DepartmentID and d.CompanyID = @CompanyID";
                var param = new DynamicParameters();
                param.Add("IsDisapproved", false);
                param.Add("IsApproved", false);
                param.Add("IsDeleted", false);
                param.Add("CompanyID", companyID);

                var resp = await _repository.GetAll<GetDepartmentModuleByDepartmentDTO>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetPendingDepartmentalAppModule => {ex.ToString()}");
                throw;
            }
        }
        public async Task<string> CreateDepartmentalAppModule(DepartmentalModulesReq departmentalAppModule)
        {
            try
            {                
                var param = new DynamicParameters();
                param.Add("DepartmentId", departmentalAppModule.DepartmentId);
                param.Add("AppModuleId", departmentalAppModule.AppModuleId);
                param.Add("DateCreated", departmentalAppModule.DateCreated);              
                param.Add("CreatedByUserId", departmentalAppModule.CreatedByUserId);
                var resp = await _repository.Get<string>("sp_create_department_app_module", param, commandType: CommandType.StoredProcedure);

                return resp;

            }
            catch (Exception ex)
            {               
                _logger.LogError($"MethodName: CreateDepartmentalAppModule => {ex}");
                throw;
            }
        }
        public async Task<long> CreateDepartmentalAppModule(DepartmentalModulesDTO departmentalAppModule)
        {
            try
            {
                string query = @"Insert into DepartmentalModules (DepartmentId , AppModuleId , DateCreated , IsActive , CreatedByUserId) values (@DepartmentId , @AppModuleId , @DateCreated , @IsActive , @CreatedByUserId) SELECT SCOPE_IDENTITY()";
                var param = new DynamicParameters();
                param.Add("DepartmentId", departmentalAppModule.DepartmentId);
                param.Add("AppModuleId", departmentalAppModule.AppModuleId);
                param.Add("DateCreated", departmentalAppModule.DateCreated);
                param.Add("IsActive", departmentalAppModule.IsActive);
                param.Add("CreatedByUserId", departmentalAppModule.CreatedByUserId);

                var resp = await _repository.Insert<int>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateDepartmentalAppModule => {ex.ToString()}");
                throw;
            }
        }


        public async Task<long> UpdateDepartmentAppModule(DepartmentalModulesDTO departmentalAppModule)
        {
            try
            {
                string query = @"Update DepartmentalModules set IsActive = @IsActive , IsDeleted = @IsDeleted , DeletedByUserId = @DeletedByUserId where DeparmentalModuleId = @DeparmentalModuleId";
                var param = new DynamicParameters();

                param.Add("IsActive", departmentalAppModule.IsActive);
                param.Add("IsDeleted", departmentalAppModule.IsDeleted);
                param.Add("DeletedByUserId", departmentalAppModule.DeletedByUserId);
                param.Add("DeparmentalModuleId", departmentalAppModule.DeparmentalModuleId);

                var resp = await _repository.Update<int>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateDepartmentAppModule => {ex.ToString()}");
                throw;
            }
        }


        public async Task<long> ApproveDepartmentalAppModule(DepartmentalModulesDTO departmentalAppModule)
        {
            try
            {
                string query = @"Update DepartmentalModules set IsActive = @IsActive , IsApproved = @IsApproved , ApprovedByUserId = @ApprovedByUserId , DateApproved = @DateApproved where DeparmentalModuleId = @DeparmentalModuleId";
                var param = new DynamicParameters();

                param.Add("IsActive", departmentalAppModule.IsActive);
                param.Add("IsApproved", departmentalAppModule.IsApproved);
                param.Add("ApprovedByUserId", departmentalAppModule.ApprovedByUserId);
                param.Add("DateApproved", departmentalAppModule.DateApproved);
                param.Add("DeparmentalModuleId", departmentalAppModule.DeparmentalModuleId);

                var resp = await _repository.Update<int>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: ApproveDepartmentalAppModule => {ex.ToString()}");
                throw;
            }
        }

        public async Task<long> DisapproveDepartmentalAppModule(DepartmentalModulesDTO departmentalAppModule)
        {
            try
            {
                string query = @"Update DepartmentalModules set IsActive = @IsActive , IsDisapproved = @IsDisapproved , DisapprovedByUserId = @DisapprovedByUserId , DateApproved = @DateApproved where DeparmentalModuleId = @DeparmentalModuleId";
                var param = new DynamicParameters();

                param.Add("IsActive", departmentalAppModule.IsActive);
                param.Add("IsDisapproved", departmentalAppModule.IsDisapproved);
                param.Add("DisapprovedByUserId", departmentalAppModule.DisapprovedByUserId);
                param.Add("DateApproved", departmentalAppModule.DateApproved);
                param.Add("DeparmentalModuleId", departmentalAppModule.DeparmentalModuleId);

                var resp = await _repository.Update<int>(query, param, commandType: CommandType.Text);

                return resp;

            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DisapproveDepartmentalAppModule => {ex.ToString()}");
                throw;
            }
        }

    }
}
