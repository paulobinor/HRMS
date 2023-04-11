using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.Repositories.Departments.Repository;
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

namespace Com.XpressPayments.Data.Repositories.Employee
{
    public  class EmployeeRepository : IEmployeeRepository
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

        public async Task<dynamic> UpdateEmployee(UpdateEmployeeDTO Emp, string updatedbyUserEmail)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", EmployeeEnum.UPDATE);
                    param.Add("@EmpIDUpd", Convert.ToInt32(Emp.EmpID));
                    param.Add("@ProfileImageUpd", Emp.ProfileImage.Trim());
                    param.Add("@BirthPlaceUpd", Emp.BirthPlace.Trim());
                    param.Add("@NationalityUpd", Emp.Nationality.Trim());
                    param.Add("@StateOfOriginUpd", Emp.StateOfOrigin.Trim());
                    param.Add("@LGAUpd", Emp.LGA.Trim());
                    param.Add("@TownUpd", Emp.Town.Trim());
                    param.Add("@MaidenNameUpd", Emp.MaidenName.Trim());
                    param.Add("@MobileNumber2Upd", Emp.MobileNumber2.Trim());
                    param.Add("@CurrentDesignationUpd,", Emp.CurrentDesignation.Trim());
                    param.Add("@SexUpd", Emp.Sex.Trim());
                    param.Add("@MaritalStatusUpd", Emp.MaritalStatus.Trim());
                    param.Add("@SpouseContactAddressUpd", Emp.SpouseContactAddress.Trim());
                    param.Add("@SpouseMobileUpd", Emp.SpouseMobile.Trim());
                    param.Add("@NamesOfChildrenUpd", Emp.NamesOfChildren.Trim());
                    param.Add("@ChildrenSexUpd", Emp.ChildrenSex.Trim());
                    param.Add("@ChildrenDOBUpd", Emp.ChildrenDOB.Trim());
                    param.Add("@ResidentialAddressUpd", Emp.ResidentialAddress.Trim());
                    param.Add("@PositionUpd", Emp.Position.Trim());
                    param.Add("@ResumptionDateUpd", Emp.ResumptionDate.Trim());
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
                    param.Add("@ContactPersonRelationshipUpd,", Emp.ContactPersonRelationship.Trim());
                    param.Add("@FirstDegreeUpd", Emp.FirstDegree.Trim());
                    param.Add("@FirstDegreeSchoolAttendedUpd", Emp.FirstDegreeSchoolAttended.Trim());
                    param.Add("@GradeObtainedUpd", Emp.GradeObtained.Trim());
                    param.Add("@FirstDegreeYearUpd", Convert.ToInt32(Emp.FirstDegreeYear));
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
                    param.Add("@AccountNumberUpd", Convert.ToInt32(Emp.AccountNumber));
                    param.Add("@BankNameUpd", Emp.BankName.Trim());
                    param.Add("@BVNUpd", Convert.ToInt32(Emp.BVN));
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
                    param.Add("@@SecondReferenceEmailUpd", Emp.SecondReferenceEmail.Trim());
                    param.Add("@FirstDegreeNameAndLocationUpd", Emp.FirstDegreeNameAndLocation.Trim());
                    param.Add("@FirstDegreeEntranceYearUpd", Convert.ToInt32(Emp.FirstDegreeEntranceYear));
                    param.Add("@FirstDegreeExitYearUpd", Convert.ToInt32(Emp.FirstDegreeExitYear));
                    param.Add("@FirstDegreeCertificateAndDegreeObtainedUpd", Emp.FirstDegreeCertificateAndDegreeObtained.Trim());
                    param.Add("@FirstDegreeMatricNoUpd", Emp.FirstDegreeMatricNo.Trim());
                    param.Add("@SecondDegreeNameAndLocationUpd", Emp.SecondrayEducationNameAndLocation.Trim());
                    param.Add("@SecondDegreeEntranceYearUpd", Convert.ToInt32(Emp.SecondDegreeEntranceYear));
                    param.Add("@SecondDegreeExitYearUpd", Convert.ToInt32(Emp.SecondDegreeExitYear));
                    param.Add("@SecondDegreeCertificateAndDegreeObtainedUpd", Emp.SecondDegreeCertificateAndDegreeObtained.Trim());
                    param.Add("@SecondDegreeMatricNoUpd", Emp.SecondDegreeMatricNo.Trim());
                    param.Add("@SecondrayEducationNameAndLocationUpd", Emp.SecondrayEducationNameAndLocation.Trim());
                    param.Add("@SecondrayEducationEntranceYearUpd", Convert.ToInt32(Emp.SecondrayEducationEntranceYear));
                    param.Add("@SecondrayEducationExitYearUpd", Convert.ToInt32(Emp.SecondrayEducationExitYear));
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
                    param.Add("@SignatureUpd", Emp.Signature.Trim());
                    param.Add("@CompanyIdUpd", Emp.CompanyID);

                    param.Add("@Updated_By_User_Email", updatedbyUserEmail.Trim());

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

        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeeCompanyId(long EmpID)
        {
            try
            {
                using (SqlConnection _dapper = new SqlConnection(_connectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@Status", 8);
                    param.Add("@CompanyIdGet", EmpID);

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

    }
}
