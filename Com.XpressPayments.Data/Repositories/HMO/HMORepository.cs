using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.Repositories.EmployeeType;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.HMO
{
    public  class HMORepository : IHMORepository
    {
        private string _connectionString;
        private readonly ILogger<HMORepository> _logger;
        private readonly IConfiguration _configuration;

        public HMORepository(IConfiguration configuration, ILogger<HMORepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> CreateHMO(CreateHMODTO create, string createdbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HMOEnum.CREATE);
                    param.Add("@HMONumber", create.HMONumber.Trim());
                    param.Add("@StaffID", create.StaffID.Trim());
                    param.Add("@DOB", create.DOB.Trim());
                    param.Add("@Telephone", create.Telephone.Trim());
                    param.Add("@ActiveEmail", create.ActiveEmail.Trim());
                    param.Add("@MaritalStatus", create.MaritalStatus.Trim());
                    param.Add("@NumberOfChildren", create.NumberOfChildren.Trim());
                    param.Add("@BloodGrp", create.BloodGrp.Trim());
                    param.Add("@Genotype", create.Genotype.Trim());
                    param.Add("@ChioceOfHospital", create.ChioceOfHospital);
                    param.Add("@SpouseName", create.SpouseName.Trim());
                    param.Add("@SpouseSex", create.SpouseSex.Trim());
                    param.Add("@SpouseDOB", create.SpouseDOB.Trim());
                    param.Add("@SpouseBloodGrp", create.SpouseBloodGrp.Trim());
                    param.Add("@SpouseGenotype", create.SpouseGenotype.Trim());
                    param.Add("@SpouseChioceOfHospital", create.SpouseChioceOfHospital);
                    param.Add("@NameOfChildren", create.NameOfChildren.Trim());
                    param.Add("@ChildrenSex", create.ChildrenSex.Trim());
                    param.Add("@ChildrenDOB", create.ChildrenDOB.Trim());
                    param.Add("@ChildrenBloodGrp", create.ChildrenBloodGrp.Trim());
                    param.Add("@ChildrenGenotype", create.ChildrenGenotype.Trim());
                    param.Add("@ChildrenChioceOfHospital", create.ChildrenChioceOfHospital);
                    param.Add("@Signature", create.Signature.Trim());
                    param.Add("@Date", create.Date.Trim());
                    param.Add("@StaffPassport", create.StaffPassport.Trim());
                    param.Add("@SpousePassport", create.SpousePassport.Trim());
                    param.Add("@Child1Passport", create.Child1Passport.Trim());
                    param.Add("@Child2Passport", create.Child2Passport.Trim());
                    param.Add("@Child3Passport", create.Child3Passport.Trim());
                    param.Add("@Child4Passport", create.Child4Passport.Trim());
                    

                    param.Add("@CompanyID", create.CompanyID);

                    param.Add("@Created_By_User_Email", createdbyUserEmail.Trim());
                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_HMO, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: CreateHMO(CreateHMODTO create, string createdbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> UpdateHMO(UpdateHMODTO HMO, string updatedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HMOEnum.UPDATE);
                    param.Add("@IDUpd", HMO.ID);
                    param.Add("@HMONumberUpd", HMO.HMONumber.Trim());
                    param.Add("@StaffIDUpd", HMO.StaffID.Trim());
                    param.Add("@DOBUpd", HMO.DOB.Trim());
                    param.Add("@TelephoneUpd", HMO.Telephone.Trim());
                    param.Add("@ActiveEmailUpd", HMO.ActiveEmail.Trim());
                    param.Add("@MaritalStatusUpd", HMO.MaritalStatus.Trim());
                    param.Add("@NumberOfChildrenUpd", HMO.NumberOfChildren.Trim());
                    param.Add("@BloodGrpUpd", HMO.BloodGrp.Trim());
                    param.Add("@GenotypeUpd", HMO.Genotype.Trim());
                    param.Add("@ChioceOfHospitalUpd", HMO.ChioceOfHospital);
                    param.Add("@SpouseNameUpd", HMO.SpouseName.Trim());
                    param.Add("@SpouseSexUpd", HMO.SpouseSex.Trim());
                    param.Add("@SpouseDOBUpd", HMO.SpouseDOB.Trim());
                    param.Add("@SpouseBloodGrpUpd", HMO.SpouseBloodGrp.Trim());
                    param.Add("@SpouseGenotypeUpd", HMO.SpouseGenotype.Trim());
                    param.Add("@SpouseChioceOfHospitalUpd", HMO.SpouseChioceOfHospital);
                    param.Add("@NameOfChildrenUpd", HMO.NameOfChildren.Trim());
                    param.Add("@ChildrenSexUpd", HMO.ChildrenSex.Trim());
                    param.Add("@ChildrenDOBUpd", HMO.ChildrenDOB.Trim());
                    param.Add("@ChildrenBloodGrpUpd", HMO.ChildrenBloodGrp.Trim());
                    param.Add("@ChildrenGenotypeUpd", HMO.ChildrenGenotype.Trim());
                    param.Add("@ChildrenChioceOfHospitalUpd", HMO.ChildrenChioceOfHospital);
                    param.Add("@SignatureUpd", HMO.Signature.Trim());
                    param.Add("@DateUpd", HMO.Date.Trim());
                    param.Add("@StaffPassportUpd", HMO.StaffPassport.Trim());
                    param.Add("@SpousePassportUpd", HMO.SpousePassport.Trim());
                    param.Add("@Child1PassportUpd", HMO.Child1Passport.Trim());
                    param.Add("@Child2PassportUpd", HMO.Child2Passport.Trim());
                    param.Add("@Child3PassportUpd", HMO.Child3Passport.Trim());
                    param.Add("@Child4PassportUpd", HMO.Child4Passport.Trim());

                    param.Add("@CompanyIdUpd", HMO.CompanyID);

                    param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_HMO, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateHMO(UpdateHMODTO HMO, string updatedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeleteHMO(DeleteHMODTO DEL, string deletedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HMOEnum.DELETE);
                    param.Add("@IDDelete", Convert.ToInt32(DEL.ID));
                    param.Add("@Deleted_By_User_Email", deletedbyUserEmail.Trim());
                    param.Add("@Reasons_For_Deleting_Department", DEL.Reasons_For_Delete == null ? "" : DEL.Reasons_For_Delete.ToString().Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_HMO, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeleteHOD(DeleteHodDTO hod, string deletedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<HMODTO>> GetAllActiveHMO()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HMOEnum.GETALLACTIVE);

                    var HMODetails = await _dapper.QueryAsync<HMODTO>(ApplicationConstant.Sp_HMO, param: param, commandType: CommandType.StoredProcedure);

                    return HMODetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllActiveHMO() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<HMODTO>> GetAllHMO()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HMOEnum.GETALL);

                    var HMODetails = await _dapper.QueryAsync<HMODTO>(ApplicationConstant.Sp_HMO, param: param, commandType: CommandType.StoredProcedure);

                    return HMODetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:GetAllHMO() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<HMODTO> GetHMOById(long ID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HMOEnum.GETBYID);
                    param.Add("@IDGet", ID);

                    var HMODetails = await _dapper.QueryFirstOrDefaultAsync<HMODTO>(ApplicationConstant.Sp_HMO, param: param, commandType: CommandType.StoredProcedure);

                    return HMODetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetHMOById(int ID) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<HMODTO> GetHMOByName(string HMONumber)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", HMOEnum.GETBYEMAIL);
                    param.Add("@HMONumberGet", HMONumber);

                    var HMODetails = await _dapper.QueryFirstOrDefaultAsync<HMODTO>(ApplicationConstant.Sp_HMO, param: param, commandType: CommandType.StoredProcedure);

                    return HMODetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetHMOByName(string HMONumber) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<HMODTO>> GetAllHMOCompanyId(long companyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", 8);
                    param.Add("@CompanyIdGet", companyId);

                    var HMODetails = await _dapper.QueryAsync<HMODTO>(ApplicationConstant.Sp_HMO, param: param, commandType: CommandType.StoredProcedure);

                    return HMODetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetAllHMOCompanyId(long companyId) ===>{ex.Message}");
                throw;
            }
        }
    }
}
