using Dapper;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using System.Data;

namespace hrms_be_backend_data.Repository
{
    public class CompanyRepository : ICompanyRepository
    {       
        private readonly ILogger<CompanyRepository> _logger;      
        private readonly IDapperGenericRepository _dapper;

        public CompanyRepository(ILogger<CompanyRepository> logger, IDapperGenericRepository dapper)
        {           
            _logger = logger;         
            _dapper = dapper;
        }
        public async Task<string> ProcessCompany(ProcessCompanyReq payload)
        {
            try
            {
                string pwd = BCrypt.Net.BCrypt.HashPassword(payload.AdminPasswordHash, BCrypt.Net.BCrypt.GenerateSalt());
                var param = new DynamicParameters();

                param.Add("@CompanyId", payload.CompanyId);
                param.Add("@CompanyName", payload.CompanyName);
                param.Add("@CompanyCode", payload.CompanyCode);
                param.Add("@CompanyLogo", payload.CompanyLogo);
                param.Add("@Website", payload.Website);
                param.Add("@FullAddress", payload.FullAddress);
                param.Add("@Email", payload.Email);
                param.Add("@ContactPhone", payload.ContactPhone);
                param.Add("@IsPublicSector", payload.IsPublicSector);
                param.Add("@AdminStaffId", payload.AdminStaffId);
                param.Add("@AdminFirstName", payload.AdminFirstName);
                param.Add("@AdminMiddleName", payload.AdminMiddleName);
                param.Add("@AdminLastName", payload.AdminLastName);
                param.Add("@AdminOfficialMail", payload.AdminOfficialMail);
                param.Add("@AdminPhoneNumber", payload.AdminPhoneNumber);
                param.Add("@AdminPasswordHash", pwd);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                param.Add("@IsModifield", payload.IsModifield);

                return await _dapper.Get<string>("sp_process_company", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyRepository -> ProcessCompany => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<string> DeleteCompany(DeleteCompanyReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@CompanyId", payload.CompanyId);
                param.Add("@Comment", payload.Comment);               
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);             

                return await _dapper.Get<string>("sp_delete_company", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyRepository -> DeleteCompany => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<string> ApproveCompany(ApproveCompanyReq payload)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@CompanyId", payload.CompanyId);              
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);

                return await _dapper.Get<string>("sp_approve_company", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyRepository -> ApproveCompany => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<CompanyWithTotalVm> GetCompanies(int PageNumber, int RowsOfPage)
        {
            var returnData = new CompanyWithTotalVm();
            try
            {
                var param = new DynamicParameters();               
                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                var result = await _dapper.GetMultiple("sp_get_companies", param, gr => gr.Read<long>(), gr => gr.Read<CompanyVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<CompanyVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyRepository -> GetCompanies => {ex}");
                return returnData;
            }

        }
        public async Task<CompanyWithTotalVm> GetCompaniesPending(int PageNumber, int RowsOfPage)
        {
            var returnData = new CompanyWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                var result = await _dapper.GetMultiple("sp_get_companies_pending", param, gr => gr.Read<long>(), gr => gr.Read<CompanyVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<CompanyVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyRepository -> GetCompaniesPending => {ex}");
                return returnData;
            }

        }
        public async Task<CompanyWithTotalVm> GetCompaniesActivated(int PageNumber, int RowsOfPage)
        {
            var returnData = new CompanyWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                var result = await _dapper.GetMultiple("sp_get_companies_activated", param, gr => gr.Read<long>(), gr => gr.Read<CompanyVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<CompanyVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyRepository -> GetCompaniesActivated => {ex}");
                return returnData;
            }

        }
        public async Task<CompanyWithTotalVm> GetCompaniesDeactivated(int PageNumber, int RowsOfPage)
        {
            var returnData = new CompanyWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                var result = await _dapper.GetMultiple("sp_get_companies_deactivated", param, gr => gr.Read<long>(), gr => gr.Read<CompanyVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<CompanyVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyRepository -> GetCompaniesDeactivated => {ex}");
                return returnData;
            }

        }
        public async Task<CompanyWithTotalVm> GetCompaniesPublicSector(int PageNumber, int RowsOfPage)
        {
            var returnData = new CompanyWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                var result = await _dapper.GetMultiple("sp_get_companies_public_sector", param, gr => gr.Read<long>(), gr => gr.Read<CompanyVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<CompanyVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyRepository -> GetCompaniesPublicSector => {ex}");
                return returnData;
            }

        }
        public async Task<CompanyWithTotalVm> GetCompaniesPrivateSector(int PageNumber, int RowsOfPage)
        {
            var returnData = new CompanyWithTotalVm();
            try
            {
                var param = new DynamicParameters();
                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                var result = await _dapper.GetMultiple("sp_get_companies_private_sector", param, gr => gr.Read<long>(), gr => gr.Read<CompanyVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<CompanyVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyRepository -> GetCompaniesPrivateSector => {ex}");
                return returnData;
            }

        }

        public async Task<CompanyFullVm> GetCompanyById(long Id)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                return await _dapper.Get<CompanyFullVm>("sp_get_company_by_id", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyRepository -> GetCompanyById => {ex}");
                return new CompanyFullVm();
            }

        }
        public async Task<CompanyFullVm> GetCompanyByName(string CompanyName)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyName", CompanyName);
                return await _dapper.Get<CompanyFullVm>("sp_get_company_by_name", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CompanyRepository -> GetCompanyByName => {ex}");
                return new CompanyFullVm();
            }

        }
    }
}
