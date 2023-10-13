﻿using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Data;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.AppConstants;

namespace hrms_be_backend_data.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private string _connectionString;
        private readonly ILogger<EmployeeRepository> _logger;
        private readonly IConfiguration _configuration;

        public EmployeeRepository(IConfiguration configuration, ILogger<EmployeeRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<dynamic> UpdateEmployee(UpdateEmployeeDTO Emp, string UpdatedByserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmployeeEnum.UPDATE);
                    param.Add("@EmpIDUpd", Convert.ToInt32(Emp.EmpID));
                    param.Add("@ProfileImageUpd", Emp.ProfileImage.Trim());

                    param.Add("@NationalityUpd", Emp.Nationality);
                    param.Add("@StateOfOriginUpd", Emp.StateOfOrigin);
                    param.Add("@LGAUpd", Emp.LGA);
                    param.Add("@TownUpd", Emp.Town.Trim());
                    param.Add("@MaidenNameUpd", Emp.MaidenName.Trim());
                    param.Add("@MobileNumber2Upd", Emp.MobileNumber2.Trim());
                    //param.Add("@CurrentDesignationUpd", Emp.CurrentDesignation.Trim());
                    param.Add("@SexUpd", Emp.Sex);
                    param.Add("@MaritalStatusUpd", Emp.MaritalStatus);
                    param.Add("@SpouseContactAddressUpd", Emp.SpouseContactAddress.Trim());
                    param.Add("@SpouseMobileUpd", Emp.SpouseMobile.Trim());
                    param.Add("@ResidentialAddressUpd", Emp.ResidentialAddress.Trim());
                    param.Add("@HomeAddressUpd", Emp.HomeAddress.Trim());
                    param.Add("@MailingAddressUpd", Emp.MailingAddress.Trim());
                    param.Add("@NofNameUpd", Emp.NofName.Trim());
                    param.Add("@NofContactAddressUpd", Emp.NofContactAddress.Trim());
                    param.Add("@NofMobileUpd", Emp.NofMobile.Trim());
                    param.Add("@NofRelationshipUpd", Emp.NofRelationship.Trim());
                    param.Add("@HighestQualificationUpd", Emp.HighestQualification.Trim());
                    param.Add("@HighestQualificationYearUpd", Emp.HighestQualificationYear.Trim());
                    param.Add("@HighestQualificationSchoolAttendedUpd", Emp.HighestQualificationSchoolAttended.Trim());
                    param.Add("@DisciplineUpd", Emp.Discipline.Trim());
                    param.Add("@ContactPersonNameUpd", Emp.ContactPersonName.Trim());
                    param.Add("@ContactPersonAddressUpd", Emp.ContactPersonAddress.Trim());
                    param.Add("@ContactPersonPhoneUpd", Emp.ContactPersonPhone.Trim());
                    param.Add("@ContactPersonRelationshipUpd", Emp.ContactPersonRelationship.Trim());
                    param.Add("@FirstDegreeUpd", Emp.FirstDegree.Trim());
                    param.Add("@FirstDegreeSchoolAttendedUpd", Emp.FirstDegreeSchoolAttended.Trim());
                    param.Add("@GradeObtainedUpd", Emp.GradeObtained.Trim());
                    param.Add("@FirstDegreeYearUpd", Emp.FirstDegreeYear.Trim());
                    param.Add("@SchoolAttendedUpd", Emp.SchoolAttended.Trim());
                    param.Add("@SecondaryEducationStartDateUpd", Emp.SecondaryEducationStartDate.Trim());
                    param.Add("@SecondaryEducationYearComletedUpd", Emp.SecondaryEducationYearComleted.Trim());
                    param.Add("@SecondaryEducationCertificateObtainedUpd", Emp.SecondaryEducationCertificateObtained.Trim());
                    param.Add("@MRECompanyNameUpd", Emp.MRECompanyName.Trim());
                    param.Add("@MREContactAddressUpd", Emp.MREContactAddress.Trim());
                    param.Add("@MREPositionHeldUpd", Emp.MREPositionHeld.Trim());
                    param.Add("@ResponsibilitiesUpd", Emp.Responsibilities.Trim());
                    param.Add("@GrossSalaryPerAnnumUpd", Emp.GrossSalaryPerAnnum.Trim());
                    param.Add("@YourPresentPensionFundAdminstratorUpd", Emp.YourPresentPensionFundAdminstrator.Trim());
                    param.Add("@YourPensionPinNumberUpd", Emp.YourPensionPinNumber.Trim());
                    param.Add("@TaxPayerIdentificationNumberUpd", Emp.TaxPayerIdentificationNumber.Trim());
                    param.Add("@BankAccountNameUpd", Emp.BankAccountName.Trim());
                    param.Add("@AccountNumberUpd", Emp.AccountNumber.Trim());
                    param.Add("@BankNameUpd", Emp.BankName.Trim());
                    param.Add("@BVNUpd", Emp.BVN.Trim());
                    param.Add("@FirstReferenceNameUpd", Emp.FirstReferenceName.Trim());
                    param.Add("@FirstReferenceAddressUpd", Emp.FirstReferenceName.Trim());
                    param.Add("@FirstReferenceOccupationUpd", Emp.FirstReferenceOccupation.Trim());
                    param.Add("@FirstReferencePeriodKnownUpd", Emp.FirstReferencePeriodKnown.Trim());
                    param.Add("@FirstReferenceMobileUpd", Emp.FirstReferenceMobile.Trim());
                    param.Add("@FirstReferenceEmailUpd", Emp.FirstReferenceEmail.Trim());
                    param.Add("@SecondReferenceNameUpd", Emp.SecondReferenceName.Trim());
                    param.Add("@SecondReferenceAddressUpd", Emp.SecondReferenceName.Trim());
                    param.Add("@SecondReferenceOccupationUpd", Emp.SecondReferenceOccupation.Trim());
                    param.Add("@SecondReferencePeriodKnownUpd", Emp.SecondReferencePeriodKnown.Trim());
                    param.Add("@SecondReferenceMobileUpd", Emp.SecondReferenceMobile.Trim());
                    param.Add("@SecondReferenceEmailUpd", Emp.SecondReferenceEmail.Trim());
                    param.Add("@FirstDegreeNameAndLocationUpd", Emp.FirstDegreeNameAndLocation.Trim());
                    param.Add("@FirstDegreeEntranceYearUpd", Emp.FirstDegreeEntranceYear.Trim());
                    param.Add("@FirstDegreeExitYearUpd", Emp.FirstDegreeExitYear.Trim());
                    param.Add("@FirstDegreeCertificateAndDegreeObtainedUpd", Emp.FirstDegreeCertificateAndDegreeObtained.Trim());
                    param.Add("@FirstDegreeMatricNoUpd", Emp.FirstDegreeMatricNo.Trim());
                    param.Add("@SecondDegreeNameAndLocationUpd", Emp.SecondrayEducationNameAndLocation.Trim());
                    param.Add("@SecondDegreeEntranceYearUpd", Emp.SecondDegreeEntranceYear.Trim());
                    param.Add("@SecondDegreeExitYearUpd", Emp.SecondDegreeExitYear.Trim());
                    param.Add("@SecondDegreeCertificateAndDegreeObtainedUpd", Emp.SecondDegreeCertificateAndDegreeObtained.Trim());
                    param.Add("@SecondDegreeMatricNoUpd", Emp.SecondDegreeMatricNo.Trim());
                    param.Add("@SecondrayEducationNameAndLocationUpd", Emp.SecondrayEducationNameAndLocation.Trim());
                    param.Add("@SecondrayEducationEntranceYearUpd", Emp.SecondrayEducationEntranceYear.Trim());
                    param.Add("@SecondrayEducationExitYearUpd", Emp.SecondrayEducationExitYear.Trim());
                    param.Add("@SecondrayEducationCertificateAndDegreeObtainedUpd", Emp.SecondrayEducationCertificateAndDegreeObtained.Trim());
                    param.Add("@SecondrayEducationExamNoUpd", Emp.SecondrayEducationExamNo.Trim());
                    param.Add("@NameOfProfessionalBodyUpd", Emp.NameOfProfessionalBody.Trim());
                    param.Add("@MembershipNoUpd", Emp.MembershipNo.Trim());
                    param.Add("@MemberStatusUpd", Emp.MemberStatus.Trim());
                    param.Add("@NameOfLastEmployerUpd", Emp.NameOfLastEmployer.Trim());
                    param.Add("@EmployerTypeOfBusinessUpd", Emp.EmployerTypeOfBusiness.Trim());
                    param.Add("@AddressOfPreviousEmployerUpd", Emp.AddressOfPreviousEmployer.Trim());
                    param.Add("@LocationOrBranchUpd", Emp.LocationOrBranch.Trim());
                    param.Add("@EmployerStartingDesignationUpd", Emp.EmployerStartingDesignation.Trim());
                    param.Add("@EmployerLastDesignationUpd", Emp.EmployerLastDesignation.Trim());
                    param.Add("@LastEmployerDateEmployedUpd", Emp.LastEmployerDateEmployed.Trim());
                    param.Add("@ReasonForLeavingUpd", Emp.ReasonForLeaving.Trim());
                    param.Add("@HRmailUpd", Emp.HRmail.Trim());
                    param.Add("@OfficeTelephoneNoUpd", Emp.OfficeTelephoneNo.Trim());
                    param.Add("@SupervisorFullNameUpd", Emp.SupervisorFullName.Trim());
                    param.Add("@SecondedByUpd", Emp.SecondedBy.Trim());
                    param.Add("@NameOfAgencyUpd", Emp.NameOfAgency.Trim());
                    param.Add("@AddressOfAgencyUpd", Emp.AddressOfAgency.Trim());
                    param.Add("@NameOfPreviousEmployerUpd", Emp.NameOfPreviousEmployer.Trim());
                    param.Add("@TypeOfBusinessUpd", Emp.TypeOfBusiness.Trim());
                    param.Add("@AddressOfPreviousEmployerUpd", Emp.AddressOfPreviousEmployer.Trim());
                    param.Add("@LocationAndBranchUpd", Emp.LocationAndBranch.Trim());
                    param.Add("@StartingDesignationUpd", Emp.StartingDesignation.Trim());
                    param.Add("@LastDesignationUpd", Emp.LastDesignation.Trim());
                    param.Add("@DateEmployedUpd", Emp.DateEmployed.Trim());
                    param.Add("@ReasonForLeavingPreviousEmployerUpd", Emp.ReasonForLeavingPreviousEmployer.Trim());
                    param.Add("@PreviousEmployerHRmailUpd", Emp.PreviousEmployerHRmail.Trim());
                    param.Add("@PreviousEmployerOfficePhoneUpd", Emp.PreviousEmployerOfficePhone.Trim());
                    param.Add("@PreviousEmployerSupervisorFullNameUpd", Emp.PreviousEmployerSupervisorFullName.Trim());
                    param.Add("@PreviousEmployerSecondedByUpd", Emp.PreviousEmployerSecondedBy.Trim());
                    param.Add("@PreviousEmployerNameOfAgencyUpd", Emp.PreviousEmployerNameOfAgency.Trim());
                    param.Add("@PreviousEmployerAddressOfAgencyUpd", Emp.PreviousEmployerAddressOfAgency.Trim());
                    param.Add("@DoYouHaveAnyPendingIssuesWithAformerEmployerUpd", Emp.DoYouHaveAnyPendingIssuesWithAformerEmployer.Trim());
                    param.Add("@IdentificationCountryOfIssueUpd", Emp.IdentificationCountryOfIssue);
                    param.Add("@IdentificationTypeUpd", Emp.IdentificationType.Trim());
                    param.Add("@IdentificationNumberUpd", Emp.IdentificationNumber.Trim());
                    param.Add("@IdentificationDocumentUpd", Emp.IdentificationDocument.Trim());
                    param.Add("@SignatureUpd", Emp.Signature.Trim());
                    param.Add("@CompanyIdUpd", Emp.CompanyID);
                    param.Add("@IsUpdateSession1Upd", Emp.IsUpdateSession1);
                    param.Add("@IsUpdateSession2Upd", Emp.IsUpdateSession2);
                    param.Add("@IsUpdateSession3Upd", Emp.IsUpdateSession3);
                    param.Add("@IsUpdateSession4Upd", Emp.IsUpdateSession4);
                    param.Add("@IsUpdateSession5Upd", Emp.IsUpdateSession5);

                    param.Add("@StampedResignationLetterfromPreviousEmployerUpd", Emp.StampedResignationLetterfromPreviousEmployer);
                    param.Add("@DegreeCertificateUpd", Emp.DegreeCertificate);
                    param.Add("@SSCEorWAECCertificateUpd", Emp.SSCEorWAECCertificate);
                    param.Add("@NYSCCertificateUpd", Emp.NYSCCertificate);
                    param.Add("@BirthCertificateUpd", Emp.BirthCertificate);
                    param.Add("@AdditionalQualificationUpd", Emp.AdditionalQualification);

                    param.Add("@Updated_By_User_Email", UpdatedByserEmail.Trim());

                    dynamic response = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Employee, param: param, commandType: CommandType.StoredProcedure);

                    return response;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: UpdateEmployee(UpdateEmployee Emp, string updtedbyUserEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAllActiveEmployee()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmployeeEnum.GETALLACTIVE);

                    var EmployeeDetails = await _dapper.QueryAsync<EmployeeDTO>(ApplicationConstant.Sp_Employee, param: param, commandType: CommandType.StoredProcedure);

                    return EmployeeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:  GetAllActiveEmployee() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployee()
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmployeeEnum.GETALL);

                    var EmployeeDetails = await _dapper.QueryAsync<EmployeeDTO>(ApplicationConstant.Sp_Employee, param: param, commandType: CommandType.StoredProcedure);

                    return EmployeeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:GetAllEmployee() ===>{ex.Message}");
                throw;
            }
        }
        public async Task<EmployeeDTO> GetEmployeeById(long EmpID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmployeeEnum.GETBYID);
                    param.Add("@EmpIDGet", EmpID);

                    var EmployeeDetails = await _dapper.QueryFirstOrDefaultAsync<EmployeeDTO>(ApplicationConstant.Sp_Employee, param: param, commandType: CommandType.StoredProcedure);

                    return EmployeeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetEmployeeById(long EmpID)===>{ex.Message}");
                throw;
            }
        }

        public async Task<EmployeeDTO> GetEmployeeByStaffID(long StaffID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmployeeEnum.GETBYEMAIL);
                    param.Add("@StaffID", StaffID);

                    var EmployeeDetails = await _dapper.QueryFirstOrDefaultAsync<EmployeeDTO>(ApplicationConstant.Sp_Employee, param: param, commandType: CommandType.StoredProcedure);

                    return EmployeeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName:  GetEmployeeByStaffID(long StaffID) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeeCompanyId(long CompanyId)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", 8);
                    param.Add("@CompanyIdGet", CompanyId);

                    var EmployeeDetails = await _dapper.QueryAsync<EmployeeDTO>(ApplicationConstant.Sp_Employee, param: param, commandType: CommandType.StoredProcedure);

                    return EmployeeDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: Task<IEnumerable<EmployeeDTO>> GetAllEmployeeCompanyId(long EmpID) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<EmployeeDTO>> GetEmpPendingApproval(long CompanyID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmployeeEnum.EMPPENDINGAPPROVAL);
                    param.Add("@CompanyIdGet", CompanyID);

                    var userDetails = await _dapper.QueryAsync<EmployeeDTO>(ApplicationConstant.Sp_Employee, param: param, commandType: CommandType.StoredProcedure);

                    return userDetails;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: GetEmpPendingApproval() ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> ApproveEmp(long approvedByuserId, string officialMail)
        {
            try
            {
                //var hashdefaultPassword = BCrypt.Net.BCrypt.HashPassword( BCrypt.Net.BCrypt.GenerateSalt());

                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmployeeEnum.APPROVEEMP);
                    param.Add("@ApprovedByUserId", approvedByuserId);
                    param.Add("@officialMailApproval", officialMail);

                    dynamic resp = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Employee, param: param, commandType: CommandType.StoredProcedure);

                    return resp;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: ApproveUser(long approvedByuserId, string defaultPass, string userEmail) ===>{ex.Message}");
                throw;
            }
        }

        public async Task<dynamic> DeclineEmp(long disapprovedByuserId, string userEmail, string comment)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmployeeEnum.DISPROVEEMP);
                    param.Add("@UserIdDisapprove", disapprovedByuserId);
                    param.Add("@UserEmailDisapprove", userEmail);
                    param.Add("@DisapprovedComment", comment);

                    dynamic resp = await _dapper.ExecuteAsync(ApplicationConstant.Sp_Employee, param: param, commandType: CommandType.StoredProcedure);

                    return resp;
                }
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"MethodName: DeclineEmp(long disapprovedByuserId, string userEmail, string comment) ===>{ex.Message}");
                throw;
            }
        }

    }
}