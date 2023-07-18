using AutoMapper;
using Com.XpressPayments.Bussiness.Services.ILogic;
using Com.XpressPayments.Bussiness.ViewModels;
using Com.XpressPayments.Data.AppConstants;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.DTOs.Account;
using Com.XpressPayments.Data.Enums;

using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.Repositories.Company.IRepository;
using Com.XpressPayments.Data.Repositories.Employee;
using Com.XpressPayments.Data.Repositories.EmployeeType;
using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.Logic
{
    public  class EmployeeService : IEmployeeService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<EmployeeService> _logger;
        //private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly IEmployeeRepository _EmployeeRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public EmployeeService(/*IConfiguration configuration*/ IAccountRepository accountRepository, ILogger<EmployeeService> logger,
            IEmployeeRepository EmployeeRepository, IAuditLog audit, ICompanyRepository companyrepository, IWebHostEnvironment hostEnvironment, IMapper mapper)
        {
            _audit = audit;

            _logger = logger;
            //_configuration = configuration;
            _accountRepository = accountRepository;
            _EmployeeRepository = EmployeeRepository;
            _companyrepository = companyrepository;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
        }

        public async Task<BaseResponse> UpdateEmployee(UpdateEmployeeDTO updateDto, RequesterInfo requester)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {

                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                if (updateDto.EmpID < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Request EmpID is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.ProfileImage))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Profile Image is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.BirthPlace))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "BirthPlace is required";
                    return response;
                }
                if (updateDto.Nationality < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Nationality is required";
                    return response;
                }
                if (updateDto.StateOfOrigin < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "StateOfOrigin is required";
                    return response;
                }
                if (updateDto.LGA < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "LGA is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.Town))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Town is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.MaidenName))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Maiden Name is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.MobileNumber2))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Mobile Number2 is required";
                    return response;
                }
                if (updateDto.Sex < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Sex is required";
                    return response;
                }
                if (updateDto.MaritalStatus < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Marital Status is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SpouseContactAddress))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Spouse Contact Address is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SpouseMobile))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Spouse Mobile is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.ResidentialAddress))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Residential Address is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.HomeAddress))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Home Address  is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.MailingAddress))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Mailing Address  is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.NofName))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Nof Name  is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.NofContactAddress))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Nof Contact Address  is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.NofMobile))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Nof Mobile  is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.NofRelationship))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Nof Relationship is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.HighestQualification))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Highest Qualification is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.HighestQualificationYear))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Highest Qualification Year is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.HighestQualificationSchoolAttended))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Highest Qualification School Attended is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.Discipline))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Discipline is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.ContactPersonName))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Contact Person Name is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.ContactPersonAddress))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Contact Person Address is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.ContactPersonPhone))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Contact Person Phone is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.ContactPersonRelationship))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Contact Person Relationship is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.FirstDegree))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "First Degree is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.FirstDegreeSchoolAttended))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "First Degree School Attended is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.GradeObtained))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Grade Obtained is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.GradeObtained))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Grade Obtained is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.FirstDegreeYear))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "First Degree Year is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SchoolAttended))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "School Attended is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SecondaryEducationStartDate))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Secondary Education Start Date is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SecondaryEducationYearComleted))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Secondary Education Year Comleted is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SecondaryEducationCertificateObtained))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Secondary Education Certificate Obtained is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.MRECompanyName))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "MRE Company Name is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.MREContactAddress))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "MRE Contact Address is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.MREPositionHeld))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "MRE Position Held is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.Responsibilities))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Responsibilities is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.GrossSalaryPerAnnum))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Gross Salary PerAnnum is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.YourPresentPensionFundAdminstrator))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Your Present Pension Fund Adminstrator is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.YourPensionPinNumber))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Your Pension Pin Number is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.TaxPayerIdentificationNumber))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Tax Payer Identification Number is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.BankAccountName))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Bank Account Name is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.AccountNumber))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Account Number is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.BankName))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Bank Name is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.BVN))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "BVN is required";
                    return response;
                }
                 if (string.IsNullOrEmpty(updateDto.FirstReferenceName))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "First Reference Name is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.FirstReferenceAddress))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "First Reference Address is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.FirstReferenceOccupation))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "First Reference Occupation is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.FirstReferencePeriodKnown))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "First Reference Period Known is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.FirstReferenceMobile))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "First Reference Mobile is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.FirstReferenceEmail))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "First Reference Email is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SecondReferenceName))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Second Reference Name is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SecondReferenceAddress))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Second Reference Address is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SecondReferenceOccupation))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Second Reference Occupation is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SecondReferencePeriodKnown))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Second Reference Period Known is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SecondReferenceMobile))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Second Reference Mobile is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SecondReferenceEmail))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Second Reference Email is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.FirstDegreeNameAndLocation))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "First Degree Name And Location is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.FirstDegreeEntranceYear))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "First Degree Entrance Year is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.FirstDegreeEntranceYear))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "First Degree Entrance Year is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.FirstDegreeExitYear))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "First Degree Exit Year is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.FirstDegreeCertificateAndDegreeObtained))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "First Degree Certificate And Degree Obtained is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.FirstDegreeMatricNo))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "First Degree MatricNo is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SecondDegreeNameAndLocation))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Second Degree Name And Location is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SecondDegreeEntranceYear))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Second Degree Entrance Year is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SecondDegreeExitYear))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Second Degree Exit Year is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SecondDegreeCertificateAndDegreeObtained))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Second Degree Certificate And Degree Obtained is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SecondDegreeMatricNo))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Second Degree MatricNo is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SecondrayEducationNameAndLocation))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Secondray Education Name And Location is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SecondrayEducationEntranceYear))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Secondray Education Entrance Year is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SecondrayEducationExitYear))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Second Degree Certificate And Degree Obtained is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SecondrayEducationCertificateAndDegreeObtained))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Secondray Education Certificate And Degree Obtained is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SecondrayEducationExamNo))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Secondray Education ExamNo is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.NameOfProfessionalBody))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Name Of Professional Body is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.MembershipNo))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Membership Number is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.MemberStatus))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Member Status is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.NameOfLastEmployer))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Name Of Last Employer is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.EmployerTypeOfBusiness))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Employer Type Of Business is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.AddressOfLastEmployer))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Address Of Last Employer is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.LocationOrBranch))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Location Or Branch is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.EmployerStartingDesignation))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Employer Starting Designation is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.LastEmployerDateEmployed))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Last Employer Date Employed is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.ReasonForLeaving))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Reason For Leaving is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.HRmail))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "HR mail is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.OfficeTelephoneNo))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Office Telephone Number is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SupervisorFullName))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Supervisor Full Name is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SecondedBy))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "SecondedBy mail is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.NameOfAgency))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Name Of Agency mail is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.AddressOfAgency))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Address Of Agency mail is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.NameOfPreviousEmployer))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Name Of Previous Employer mail is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.LocationAndBranch))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Location And Branch mail is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.StartingDesignation))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Starting Designation mail is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.LastDesignation))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Last Designation mail is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.DateEmployed))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Date Employed mail is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.ReasonForLeavingPreviousEmployer))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Reason For Leaving Previous Employer mail is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.PreviousEmployerHRmail))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Previous Employer HR mail mail is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.PreviousEmployerOfficePhone))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Previous Employer Office Phone mail is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.PreviousEmployerSupervisorFullName))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Previous Employer Supervisor FullName mail is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.PreviousEmployerSecondedBy))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Previous Employer SecondedBy is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.PreviousEmployerNameOfAgency))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Previous Employer Name Of Agency is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.PreviousEmployerAddressOfAgency))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Previous Employer Address Of Agency is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.DoYouHaveAnyPendingIssuesWithAformerEmployer))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Do You Have Any Pending Issues With A former Employer is required";
                    return response;
                }
                if (updateDto.IdentificationCountryOfIssue < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Identification Country Of Issue is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.IdentificationType))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Identification Type is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.IdentificationNumber))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Identification Number is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.IdentificationDocument))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Identification Document is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.Signature))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Signature is required";
                    return response;
                }
                if (updateDto.CompanyID < 1)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "CompanyID is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.StampedResignationLetterfromPreviousEmployer))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Stamped Resignation Letter from Previous Employer is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.DegreeCertificate))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Degree Certificate is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.NYSCCertificate))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "NYSC Certificate is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.SSCEorWAECCertificate))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "SSCE or WAEC Certificate is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.BirthCertificate))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "Birth Certificate is required";
                    return response;
                }
                if (string.IsNullOrEmpty(updateDto.AdditionalQualification))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "HR mail is required";
                    return response;
                }



                //if (Convert.ToInt32(RoleId) != 1)
                //{
                //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //    return response;
                //}

                if (Convert.ToInt32(RoleId) != 2)
                {
                    if (Convert.ToInt32(RoleId) != 3)
                    {
                        if (Convert.ToInt32(RoleId) != 4)
                        {
                            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                            return response;
                        }


                    }

                }

                var Emp = await _EmployeeRepository.GetEmployeeById(updateDto.EmpID);
                if (null == Emp)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "No record found for the specified Employee";
                    response.Data = null;
                    return response;
                }

                dynamic resp = await _EmployeeRepository.UpdateEmployee(updateDto, requesterUserEmail);
                if (resp > 0)
                {
                    //update action performed into audit log here

                    var updatedEmp = await _EmployeeRepository.GetEmployeeById(updateDto.EmpID);

                    _logger.LogInformation("Employee updated successfully.");
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Employee updated successfully.";
                    response.Data = updatedEmp;
                    return response;
                }
                response.ResponseCode = ResponseCode.Exception.ToString();
                response.ResponseMessage = "An error occurred while updating Employee.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: UpdateEmployeeDTO ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: UpdateEmployeeDTO ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllActiveEmployee(RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }
                if (Convert.ToInt32(RoleId) != 2)
                {
                    if (Convert.ToInt32(RoleId) != 3)
                    {
                        if (Convert.ToInt32(RoleId) != 4)
                        {
                            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                            return response;
                        }


                    }

                }

                //update action performed into audit log here

                var Emp = await _EmployeeRepository.GetAllActiveEmployee();

                if (Emp.Any())
                {
                    response.Data = Emp;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Employee fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No Employee found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllActiveEmployee() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllActiveEmployee() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllEmployee(RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                //if (Convert.ToInt32(RoleId) > 2)
                //{
                //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //    return response;
                //}


                if (Convert.ToInt32(RoleId) != 2)
                {
                    if (Convert.ToInt32(RoleId) != 3)
                    {
                        if (Convert.ToInt32(RoleId) != 4)
                        {
                            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                            return response;
                        }


                    }

                }

                //update action performed into audit log here

                var Employee = await _EmployeeRepository.GetAllEmployee();

                if (Employee.Any())
                {
                    response.Data = Employee;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Employee fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No Employee found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllEmployee() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllEmployee() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetEmployeeById(long EmpID, RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                //if (Convert.ToInt32(RoleId) > 2)
                //{
                //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //    return response;
                //}


                if (Convert.ToInt32(RoleId) != 2)
                {
                    if (Convert.ToInt32(RoleId) != 3)
                    {
                        if (Convert.ToInt32(RoleId) != 4)
                        {
                            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                            return response;
                        }
                         

                    }

                }

                var EmployeeType = await _EmployeeRepository.GetEmployeeById(EmpID);

                if (EmployeeType == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Employee not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = EmployeeType;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Employee fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetEmployeeById(long EmployeeTypeID ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured:  GetEmployeeById(long EmployeeTypeID  ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetEmployeebyCompanyId(long companyId, RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                //if (Convert.ToInt32(RoleId) != 2)
                //{
                //    if (Convert.ToInt32(RoleId) != 3)
                //    {
                //        if (Convert.ToInt32(RoleId) != 4)
                //        {
                //            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //            return response;
                //        }


                //    }

                //}

                if (Convert.ToInt32(RoleId) != 2)
                {
                  
                        if (Convert.ToInt32(RoleId) != 4)
                        {
                            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                            return response;
                        }

                }

                var Emp = await _EmployeeRepository.GetAllEmployeeCompanyId(companyId);

                if (Emp == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Employee not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = Emp;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Employee fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllEmployeeCompanyId(long companyId) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllEmployeeCompanyId(long companyId) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public Tuple<bool, bool> checkPermission(int roleId, int roleId2)
        {
            bool checkCanCreateAndRead = false;
            bool canApprove = false;


            //logically check the role of those that are creating and the created
            if (roleId2 == ApplicationConstant.SuperAdmin)
            {
                if (roleId == ApplicationConstant.HrHead
                || roleId == ApplicationConstant.HrAdmin)
                {
                    checkCanCreateAndRead = true;
                    canApprove = true;
                }
            }


            if (roleId2 == ApplicationConstant.HrHead)
            {
                if (roleId == ApplicationConstant.GeneralUser)
                {
                    checkCanCreateAndRead = true;
                    canApprove = true;
                }
            }


            if (roleId2 == ApplicationConstant.HrAdmin)
            {
                if (roleId == ApplicationConstant.GeneralUser)
                {
                    checkCanCreateAndRead = true;
                    canApprove = false;
                }
            }

            return new Tuple<bool, bool>(checkCanCreateAndRead, canApprove);

        }

        public async Task<BaseResponse> GetEmpPendingApproval( long CompanyID, RequesterInfo requester)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }


                if (Convert.ToInt32(RoleId) != 2)
                {
                    if (Convert.ToInt32(RoleId) != 3)
                    {
                        if (Convert.ToInt32(RoleId) != 4)
                        {
                            response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                            response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                            return response;
                        }


                    }

                }

                var mappeduser = new List<EmployeeDTO>();
                var users = await _EmployeeRepository.GetEmpPendingApproval(CompanyID);
                if (users.Any())
                {
                    //update action performed into audit log here

                    foreach (var user in users)
                    {
                        var usermap = _mapper.Map<EmployeeDTO>(user);
                        usermap.UserStatus = "Pending";
                        usermap.UserStatusId = Convert.ToInt32(UserStatus.PENDING);
                        mappeduser.Add(usermap);
                    }


                    
                        response.Data = mappeduser;
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = "Employee Details fetched successfully.";
                        return response;
                    
                }
                else
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "No Users found.";
                    response.Data = null;
                    return response;
                }


             

                //update action performed into audit log here

                //var Employee = await _EmployeeRepository.GetEmpPendingApproval();

                //if (Employee.Any())
                //{
                //    response.Data = Employee;
                //    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = "Employee Details fetched successfully.";
                //    return response;
                //}

                //response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                //response.ResponseMessage = "No Employee Details found.";
                //response.Data = null;
                //return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetEmpPendingApproval(CompanyID) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetEmpPendingApproval(CompanyID) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> ApproveEmp(ApproveEmp approveEmp, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);

                var UserInfo = await _accountRepository.FindUser(approveEmp.officialMail);

                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                if (null == UserInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "User information cannot be found.";
                    return response;
                }



                //Tuple<bool, bool> checkRole = checkPermission(UserInfo.RoleId, requesterInfo.RoleId);


                //if (!checkRole.Item2)
                //{
                //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //    return response;
                //}


                if (Convert.ToInt32(RoleId) != 4)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }

                var user = await _accountRepository.FindUser(approveEmp.officialMail);
                if (user != null)
                {
                    if (user.CreatedByUserId == Convert.ToInt32(requesterUserId))
                    {
                        response.ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = "You cannot approve this User because User was created by you.";
                        return response;
                    }
                  

                    dynamic resp = await _EmployeeRepository.ApproveEmp(Convert.ToInt32(requesterUserId),  user.officialMail);
                    //dynamic resp = await _EmployeeRepository.ApproveEmp(requesterUserId, defaultPass, user.Email);

                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        _logger.LogInformation($"User with email: {user.officialMail} approved successfully.");
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"User with email: {user.officialMail} approved successfully.";
                        return response;
                    }
                    response.ResponseCode = ResponseCode.Exception.ToString();
                    response.ResponseMessage = "An error occurred while approving the user.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Invalid user. Not found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : ApproveEmp ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : ApproveEmp ==> {ex.Message}";
                response.Data = null;
                return response;
            }

        }

        public async Task<BaseResponse> DisapproveEmp(DisapproveEmpDto disapproveEmp, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string requesterUserEmail = requester.Username;
                string requesterUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(requesterUserEmail);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }

                //if (Convert.ToInt32(RoleId) != 1 || Convert.ToInt32(RoleId) != 4)
                //{
                //    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                //    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                //    return response;
                //}


                Tuple<bool, bool> checkRole = checkPermission(requesterInfo.RoleId, requesterInfo.RoleId);


                if (!checkRole.Item2)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                    return response;
                }

                var user = await _accountRepository.FindUser(disapproveEmp.Email);
                if (user != null)
                {
                    if (user.CreatedByUserId == Convert.ToInt32(requesterUserId))
                    {
                        response.ResponseCode = ResponseCode.ProcessingError.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = "You cannot disapprove this User because User was created by you.";
                        return response;
                    }




                    dynamic resp = await _accountRepository.DeclineUser(Convert.ToInt32(requesterUserId), user.Email, disapproveEmp.DisapprovedComment);
                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        _logger.LogInformation($"User with email: {user.Email} disapproved successfully.");
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"User with email: {user.Email} disapproved successfully.";
                        return response;
                    }
                    response.ResponseCode = ResponseCode.Exception.ToString();
                    response.ResponseMessage = "An error occurred while disapproving the user.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Invalid user. Not found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: ControllerMethod : DisapproveUser ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: ControllerMethod : DisapproveUser ==> {ex.Message}";
                response.Data = null;
                return response;
            }

        }

    }
}
