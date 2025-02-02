﻿using Dapper;
using hrms_be_backend_data.AppConstants;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Data;

namespace hrms_be_backend_data.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {

        private readonly ILogger<EmployeeRepository> _logger;
        private readonly IDapperGenericRepository _dapper;

        public EmployeeRepository(ILogger<EmployeeRepository> logger, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _dapper = dapper;
        }
        public async Task<string> ProcessEmployeeBasis(EmployeeBasisReq payload)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", payload.EmployeeId);
                param.Add("@StaffId", payload.StaffId);
                param.Add("@FirstName", payload.FirstName);
                param.Add("@MiddleName", payload.MiddleName);
                param.Add("@LastName", payload.LastName);
                param.Add("@DOB", payload.DOB);
                param.Add("@PersonalEmail", payload.PersonalEmail);
                param.Add("@OfficialEmail", payload.OfficialEmail);
                param.Add("@PhoneNumber", payload.PhoneNumber);
                param.Add("@EmploymentStatusId", payload.EmploymentStatusId);
                param.Add("@BranchId", payload.BranchId);
                param.Add("@EmployeeTypeId", payload.EmployeeTypeId);
                param.Add("@DepartmentId", payload.DepartmentId);
                param.Add("@ResumptionDate", payload.ResumptionDate);
                param.Add("@JobRoleId", payload.JobRoleId);
                param.Add("@UnitId", payload.UnitId);
                param.Add("@GradeId", payload.GradeId);
                param.Add("@IsMD", payload.IsMD);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);
                param.Add("@IsModifield", payload.IsModifield);
                param.Add("@SupervisorId", payload.SupervisorId);
                param.Add("@GroupHeadId", payload.GroupHeadId);

                var res = await _dapper.Get<string>("sp_process_employee_basis", param, commandType: CommandType.StoredProcedure);
                _logger.LogInformation($" response from create employee: {res}");
                return res;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"EmployeeRepository => ProcessEmployeeBasis ===> {ex.Message}");
                throw;
            }
        }
        public async Task<string> ProcessEmployeePersonalInfo(EmployeePersonalInfoReq payload)
        {
            _logger.LogInformation($"Payload to send to DB: {JsonConvert.SerializeObject(payload)}");
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", payload.EmployeeId);
                param.Add("@BirthPlace", payload.BirthPlace);
                param.Add("@NationalityId", payload.NationalityId);
                param.Add("@StateOfOriginId", payload.StateOfOriginId);
                param.Add("@LGAOfOriginId", payload.LGAOfOriginId);
                param.Add("@TownOfOrigin", payload.TownOfOrigin);
                param.Add("@MaidenName", payload.MaidenName);
                param.Add("@MaritalStatusId", payload.MaritalStatusId);
                param.Add("@SpouseName", payload.SpouseName);
                param.Add("@NoOfChildren", payload.NoOfChildren);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                //param.Add("@DateCreated", payload.DateCreated);
                param.Add("@GenderId", payload.GenderId);
                param.Add("@ProfileImage", payload.ProfileImage);
               // param.Add("@PreviousEmployerAddress", payload.PreviousEmployerAddress);
                param.Add("@HasChildren", payload.HasChildren);
                param.Add("@NIN", payload.NIN);

                var res = await _dapper.Get<string>("sp_process_employee_personal_info", param, commandType: CommandType.StoredProcedure);

                _logger.LogInformation($"response from DB: {res}");

                return res;
            }
            catch (Exception ex)
            {
               // var err = ex.Message;
                _logger.LogError($"EmployeeRepository => ProcessEmployeePersonalInfo ===> {ex.Message}");
                throw;
            }
        }
        public async Task<string> ProcessEmployeeIdentification(EmployeeIdentificationReq payload)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", payload.EmployeeId);
                param.Add("@IdentificationTypeId", payload.IdentificationTypeId);
                param.Add("@IdentificationNumber", payload.IdentificationNumber);
                param.Add("@CountryIdentificationIssuedId", payload.CountryIdentificationIssuedId);
                param.Add("@IdentificationDocument", payload.IdentificationDocument);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);

                return await _dapper.Get<string>("sp_process_employee_identification", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"EmployeeRepository => ProcessEmployeeIdentification ===> {ex.Message}");
                throw;
            }
        }
        public async Task<string> ProcessEmployeeContactDetails(EmployeeContactDetailsReq payload)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", payload.EmployeeId);
                param.Add("@PersonalEmail", payload.PersonalEmail);
                param.Add("@PhoneNumber", payload.PhoneNumber);
                param.Add("@CurrentAddressStateId", payload.CurrentAddressStateId);
                param.Add("@CurrentAddressLGAId", payload.CurrentAddressLGAId);
                param.Add("@CurrentAddressCity", payload.CurrentAddressCity);
                param.Add("@CurrentAddressOne", payload.CurrentAddressOne);
                param.Add("@CurrentAddressTwo", payload.CurrentAddressTwo);
                param.Add("@MailingAddressStateId", payload.MailingAddressStateId);
                param.Add("@MailingAddressLGAId", payload.MailingAddressLGAId);
                param.Add("@MailingAddressCity", payload.MailingAddressCity);
                param.Add("@MailingAddressOne", payload.MailingAddressOne);
                param.Add("@MailingAddressTwo", payload.MailingAddressTwo);
                param.Add("@SpouseAddressStateId", payload.SpouseAddressStateId);
                param.Add("@SpouseAddressLGAId", payload.SpouseAddressLGAId);
                param.Add("@SpouseAddressCity", payload.SpouseAddressCity);
                param.Add("@SpouseAddressOne", payload.SpouseAddressOne);
                param.Add("@SpouseAddressTwo", payload.SpouseAddressTwo);
                param.Add("@NextOfKinName", payload.NextOfKinName);
                param.Add("@NextOfKinRelationship", payload.NextOfKinRelationship);
                param.Add("@NextOfKinPhoneNumber", payload.NextOfKinPhoneNumber);
                param.Add("@NextOfKinEmailAddress", payload.NextOfKinEmailAddress);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);

                return await _dapper.Get<string>("sp_process_employee_contact_details", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"EmployeeRepository => ProcessEmployeeContactDetails ===> {ex.Message}");
                throw;
            }
        }
        public async Task<string> ProcessEmployeeProfesionalBackground(EmployeeProfesionalBackgroundReq payload)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", payload.EmployeeId);
                param.Add("@CompanyName", payload.CompanyName);
                param.Add("@CompanyAddress", payload.CompanyAddress);
                param.Add("@PositionHead", payload.PositionHead);
                param.Add("@StartDate", payload.StartDate);
                param.Add("@EndDate", payload.EndDate);
                param.Add("@ContactEmail", payload.ContactEmail);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);

                return await _dapper.Get<string>("sp_process_employee_prof_background", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"EmployeeRepository => ProcessEmployeeProfesionalBackground ===> {ex.Message}");
                throw;
            }
        }
        public async Task<string> ProcessEmployeeReference(EmployeeReferenceReq payload)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", payload.EmployeeId);
                param.Add("@FullName", payload.FullName);
                param.Add("@ContactAddress", payload.ContactAddress);
                param.Add("@Occupation", payload.Occupation);
                param.Add("@PeriodKnown", payload.PeriodKnown);
                param.Add("@PhoneNumber", payload.PhoneNumber);
                param.Add("@EmailAddress", payload.EmailAddress);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);

                return await _dapper.Get<string>("sp_process_employee_reference", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"EmployeeRepository => ProcessEmployeeReference ===> {ex.Message}");
                throw;
            }
        }
        public async Task<string> ProcessEmployeeEduBackground(EmployeeEduBackgroundReq payload)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", payload.EmployeeId);
                param.Add("@InstitutionName", payload.InstitutionName);
                param.Add("@CertificateName", payload.CertificateName);
                param.Add("@CertificateDoc", payload.CertificateDoc);
                param.Add("@StartDate", payload.StartDate);
                param.Add("@EndDate", payload.EndDate);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);

                return await _dapper.Get<string>("sp_process_employee_edu_background", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"EmployeeRepository => ProcessEmployeeEduBackground ===> {ex.Message}");
                throw;
            }
        }
        public async Task<string> ProcessEmployeeBankDetails(EmployeeBankDetailsReq payload)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", payload.EmployeeId);
                param.Add("@BankName", payload.BankName);
                param.Add("@BVN", payload.BVN);
                param.Add("@NIN", payload.NIN);
                param.Add("@AccountName", payload.AccountName);
                param.Add("@AccountNumber", payload.AccountNumber);
                param.Add("@PensionAdministrator", payload.PensionAdministrator);
                param.Add("@PensionPinNumber", payload.PensionPinNumber);
                param.Add("@TaxNumber", payload.TaxNumber);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);

                return await _dapper.Get<string>("sp_process_employee_bank_details", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"EmployeeRepository => ProcessEmployeeBankDetails ===> {ex.Message}");
                throw;
            }
        }
        public async Task<string> ProcessEmployeeCompensation(EmployeeCompensationReq payload)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", payload.EmployeeId);
                param.Add("@PayrollId", payload.PayrollId);
                param.Add("@BaseSalary", payload.BaseSalary);
                param.Add("@SalaryEffectiveFrom", payload.SalaryEffectiveFrom);
                param.Add("@CreatedByUserId", payload.CreatedByUserId);
                param.Add("@DateCreated", payload.DateCreated);

                return await _dapper.Get<string>("sp_process_employee_compensation", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"EmployeeRepository => ProcessEmployeeBankDetails ===> {ex.Message}");
                throw;
            }
        }

        public async Task<string> ApproveEmployee(long EmployeeId, string PasswordHash, long CreatedByUserId)
        {
            _logger.LogInformation($"Received request to approve employee: {JsonConvert.SerializeObject(new { EmployeeId, PasswordHash, CreatedByUserId })} ");
            try
            {
                string pwd = BCrypt.Net.BCrypt.HashPassword(PasswordHash, BCrypt.Net.BCrypt.GenerateSalt());
                var param = new DynamicParameters();
                param.Add("@EmployeeId", EmployeeId);
                param.Add("@PasswordHash", pwd);
                param.Add("@CreatedByUserId", CreatedByUserId);
                param.Add("@DateCreated", DateTime.Now);

                var res = await _dapper.Get<string>("sp_approve_employee", param, commandType: CommandType.StoredProcedure);
                _logger.LogInformation($"Approve Employee sp_approve_employee response: {res}");
                return res;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"EmployeeRepository => ApproveEmployee ===> {ex.Message}, stacktrace: {ex.StackTrace}");
                throw;
            }
        }
        public async Task<string> DisapproveEmployee(long Id, string Comment, long CreatedByUserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                param.Add("@Comment", Comment);
                param.Add("@CreatedByUserId", CreatedByUserId);
                param.Add("@DateCreated", DateTime.Now);

                return await _dapper.Get<string>("sp_disapprove_employee", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"EmployeeRepository => DisapproveEmployee ===> {ex.Message}");
                throw;
            }
        }
        public async Task<string> DeleteEmployee(long Id, string Comment, long CreatedByUserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", Id);
                param.Add("@Comment", Comment);
                param.Add("@CreatedByUserId", CreatedByUserId);
                param.Add("@DateCreated", DateTime.Now);

                return await _dapper.Get<string>("sp_delete_employee", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"EmployeeRepository => DeleteEmployee ===> {ex.Message}");
                throw;
            }
        }
        public async Task<string> CheckEmployeeStaffId(string StaffId, long CompanyId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@StaffId", StaffId);
                param.Add("@CompanyId", CompanyId);
                param.Add("@DateCreated", DateTime.Now);

                return await _dapper.Get<string>("sp_check_employee_staff_id", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"EmployeeRepository => CheckEmployeeStaffId ===> {ex.Message}");
                throw;
            }
        }
        public async Task<EmployeeWithTotalVm> GetEmployees(int PageNumber, int RowsOfPage, long AccessByUserId)
        {
            var returnData = new EmployeeWithTotalVm();
            try
            {
                var param = new DynamicParameters();

                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@AccessByUserId", AccessByUserId);
                var result = await _dapper.GetMultiple("sp_get_employees", param, gr => gr.Read<long>(), gr => gr.Read<EmployeeVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<EmployeeVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeRepository -> GetEmployees => {ex}");
                return returnData;
            }

        }
        public async Task<EmployeeWithTotalVm> GetEmployeesApproved(int PageNumber, int RowsOfPage, long AccessByUserId)
        {
            var returnData = new EmployeeWithTotalVm();
            try
            {
                var param = new DynamicParameters();

                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@AccessByUserId", AccessByUserId);
                var result = await _dapper.GetMultiple("sp_get_employees_approved", param, gr => gr.Read<long>(), gr => gr.Read<EmployeeVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<EmployeeVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeRepository -> GetEmployeesApproved => {ex}");
                return returnData;
            }

        }
        public async Task<EmployeeWithTotalVm> GetEmployeesDisapproved(int PageNumber, int RowsOfPage, long AccessByUserId)
        {
            var returnData = new EmployeeWithTotalVm();
            try
            {
                var param = new DynamicParameters();

                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@AccessByUserId", AccessByUserId);
                var result = await _dapper.GetMultiple("sp_get_employees_disapproved", param, gr => gr.Read<long>(), gr => gr.Read<EmployeeVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<EmployeeVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeRepository -> GetEmployeesDisapproved => {ex}");
                return returnData;
            }

        }
        public async Task<EmployeeWithTotalVm> GetEmployeesDeleted(int PageNumber, int RowsOfPage, long AccessByUserId)
        {
            var returnData = new EmployeeWithTotalVm();
            try
            {
                var param = new DynamicParameters();

                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@AccessByUserId", AccessByUserId);
                var result = await _dapper.GetMultiple("sp_get_employees_deleted", param, gr => gr.Read<long>(), gr => gr.Read<EmployeeVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<EmployeeVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeRepository -> GetEmployeesDeleted => {ex}");
                return returnData;
            }

        }
        public async Task<EmployeeWithTotalVm> GetEmployeesPending(int PageNumber, int RowsOfPage, long AccessByUserId)
        {
            var returnData = new EmployeeWithTotalVm();
            try
            {
                var param = new DynamicParameters();

                param.Add("@PageNumber", PageNumber);
                param.Add("@RowsOfPage", RowsOfPage);
                param.Add("@AccessByUserId", AccessByUserId);
                var result = await _dapper.GetMultiple("sp_get_employees_pending", param, gr => gr.Read<long>(), gr => gr.Read<EmployeeVm>(), commandType: CommandType.StoredProcedure);
                var totalData = result.Item1.SingleOrDefault<long>();
                var data = result.Item2.ToList<EmployeeVm>();
                returnData.totalRecords = totalData;
                returnData.data = data;
                return returnData;

            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeRepository -> GetEmployeesPending => {ex}");
                return returnData;
            }

        }
        public async Task<EmployeeSindgleVm> GetEmployeeById(long EmployeeId, long CompanyId)
        {
            try
            {
                var returnData = new EmployeeSindgleVm();
                var param = new DynamicParameters();
                param.Add("@EmployeeId", EmployeeId);
                param.Add("@CompanyId", CompanyId);
                var result = await _dapper.GetMultiple("sp_get_employee_by_id", param, gr => gr.Read<EmployeeFullVm>(), 
                    gr => gr.Read<EmployeeCertificationVm>(), 
                    gr => gr.Read<EmployeeEduBackgroundVm>(),
                    gr => gr.Read<EmployeeIdentificationVm>(), 
                    gr => gr.Read<EmployeeProfBackgroundVm>(), 
                    gr => gr.Read<EmployeeRefereeVm>(), 
                    commandType: CommandType.StoredProcedure
                    );

                var employeeFullVm = result.Item1.SingleOrDefault();
                var employeeCertificationVm = result.Item2.ToList<EmployeeCertificationVm>();
                var employeeEduBackgroundVm = result.Item3.ToList<EmployeeEduBackgroundVm>();
                var employeeIdentificationVm = result.Item4.ToList<EmployeeIdentificationVm>();
                var employeeProfBackgroundVm = result.Item5.ToList<EmployeeProfBackgroundVm>();
                var employeeRefereeVm = result.Item6.ToList<EmployeeRefereeVm>();
                returnData.Employee = employeeFullVm;
                returnData.EmployeeCertifications = employeeCertificationVm;
                returnData.EmployeeEduBackground = employeeEduBackgroundVm;
                returnData.EmployeeIdentifications = employeeIdentificationVm;
                returnData.EmployeeProfBackground = employeeProfBackgroundVm;
                returnData.EmployeeReferees = employeeRefereeVm;

                return returnData;


                //return await _dapper.Get<EmployeeSindgleVm>("sp_get_employee_by_id", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeRepository => GetEmployeeById || {ex}");
                return null;
            }
        }
        public async Task<int> AddEmployeeBulk(DataTable dataTable, RequesterInfo requester, long currentStaffCount, int listCount, long companyID)
        {
            try
            {
                var param = new
                {
                    DateCreated = DateTime.Now,
                    CreatedBy = requester.Username,
                    UserID = requester.UserId,
                    CurrentStaffCount = currentStaffCount,
                    Count = listCount,
                    CompanyID = companyID,
                    Users = dataTable.AsTableValuedParameter("EmployeeType"),
                };
                var resp = await _dapper.BulkInsert<int>(param, "sp_CreateEmployeeBulk");
                return resp;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: AddUserBulk(AddEmployeeBulk user) ===>{ex.Message}");
                throw;
            }
        }
        public async Task<EmployeeFullVm> GetEmployeeByUserId(long UserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", UserId);
                return await _dapper.Get<EmployeeFullVm>("sp_get_employee_by_userid", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeRepository => GetEmployeeByUserId || {ex}");
                return new EmployeeFullVm();
            }
        }

        public async Task<EmployeeFullVm> GetEmployeeByEmail(string email, long companyID)
        {
            string query = @"select * from Employee where OfficialMail = @Email and IsDeleted = @IsDeleted and IsApproved = @IsApproved and CompanyID = @CompanyID";
            var param = new DynamicParameters();
            param.Add("Email", email);
            param.Add("IsDeleted", false);
            param.Add("IsApproved", true);
            param.Add("CompanyID", companyID);
            return await _dapper.Get<EmployeeFullVm>(query, param, commandType: CommandType.Text);

        }
        public async Task<List<EmployeeCertificationVm>> GetEmployeeCertification(long EmployeeId, long CompanyId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", EmployeeId);
                param.Add("@CompanyId", CompanyId);
                return await _dapper.GetAll<EmployeeCertificationVm>("sp_get_employee_certifications", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeRepository => GetEmployeeCertification || {ex}");
                return null;
            }
        }
        public async Task<List<EmployeeEduBackgroundVm>> GetEmployeeEduBackground(long EmployeeId, long CompanyId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", EmployeeId);
                param.Add("@CompanyId", CompanyId);
                return await _dapper.GetAll<EmployeeEduBackgroundVm>("sp_get_employee_edu_background", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeRepository => GetEmployeeEduBackground || {ex}");
                return null;
            }
        }
        public async Task<List<EmployeeIdentificationVm>> GetEmployeeIdentification(long EmployeeId, long CompanyId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", EmployeeId);
                param.Add("@CompanyId", CompanyId);
                return await _dapper.GetAll<EmployeeIdentificationVm>("sp_get_employee_identification", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeRepository => GetEmployeeIdentification || {ex}");
                return null;
            }
        }
        public async Task<List<EmployeeProfBackgroundVm>> GetEmployeeProfBackground(long EmployeeId, long CompanyId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", EmployeeId);
                param.Add("@CompanyId", CompanyId);
                return await _dapper.GetAll<EmployeeProfBackgroundVm>("sp_get_employee_identification", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeRepository => GetEmployeeProfBackground || {ex}");
                return null;
            }
        }
        public async Task<List<EmployeeRefereeVm>> GetEmployeeReferee(long EmployeeId, long CompanyId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", EmployeeId);
                param.Add("@CompanyId", CompanyId);
                return await _dapper.GetAll<EmployeeRefereeVm>("sp_get_employee_reference", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeRepository => GetEmployeeReferee || {ex}");
                return null;
            }
        }

        public async Task<long> GetHR_ID(long CompanyId, long EmployeeID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyId", CompanyId);
                param.Add("@EmployeeID", EmployeeID);
                return await _dapper.Get<long>(ApplicationConstant.sp_get_hr_Id, param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"EmployeeRepository => GetEmployeeReferee || {ex}");
                return default;
            }
        }
    }
}
