//using Com.XpressPayments.Bussiness.LeaveModuleService.Service.ILogic;
//using Com.XpressPayments.Bussiness.Services.Logic;
//using Com.XpressPayments.Data.DTOs;
//using Com.XpressPayments.Data.Enums;
//using Com.XpressPayments.Data.GenericResponse;
//using Com.XpressPayments.Data.LeaveModuleDTO.DTO;
//using Com.XpressPayments.Data.LeaveModuleRepository.LeaveRequestRepo;
//using Com.XpressPayments.Data.Repositories.Branch;
//using Com.XpressPayments.Data.Repositories.Company.IRepository;
//using Com.XpressPayments.Data.Repositories.CountryStateLga;
//using Com.XpressPayments.Data.Repositories.EmployeeType;
//using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
//using ExcelDataReader;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Com.XpressPayments.Bussiness.LeaveModuleService.Service.Logic
//{
//    public  class LeaveRequestService : ILeaveRequestService
//    {
//        private readonly IAuditLog _audit;

//        private readonly ILogger<LeaveRequestService> _logger;
//        //private readonly IConfiguration _configuration;
//        private readonly IAccountRepository _accountRepository;
//        private readonly ICompanyRepository _companyrepository;
//        private readonly ILeaveRequestRepository _LeaveRequestRepository;

//        public LeaveRequestService(/*IConfiguration configuration*/ IAccountRepository accountRepository, ILogger<LeaveRequestService> logger,
//            ILeaveRequestRepository LeaveRequestRepository, IAuditLog audit, ICompanyRepository companyrepository)
//        {
//            _audit = audit;

//            _logger = logger;
//            //_configuration = configuration;
//            _accountRepository = accountRepository;
//            _LeaveRequestRepository = LeaveRequestRepository;
//            _companyrepository = companyrepository;
//        }
//        public async Task<BaseResponse> CreateLeaveRequest(LeaveRequestCreate payload)
//        {
//            //check if us
//            StringBuilder errorOutput = new StringBuilder();
//            var response = new BaseResponse();
//            try
//            {
//                if (string.IsNullOrEmpty(payload.Notes))
//                {
//                    response.ResponseCode = "08";
//                    response.ResponseMessage = "Note is required";
//                    return response;
//                }
//                if (payload.RequestYear<1)
//                {
//                    response.ResponseCode = "08";
//                    response.ResponseMessage = "Note is required";
//                    return response;
//                }
//                if (string.IsNullOrEmpty(payload.))
//                {
//                    response.ResponseCode = "08";
//                    response.ResponseMessage = "Note is required";
//                    return response;
//                }
//                else if (!Path.GetExtension(payload.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
//                {
//                    response.ResponseCode = "08";
//                    response.ResponseMessage = "File not an Excel Format";
//                    return response;
//                }
//                else
//                {
//                    var stream = new MemoryStream();
//                    await payload.CopyToAsync(stream);

//                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
//                    var reader = ExcelReaderFactory.CreateReader(stream);
//                    DataSet ds = new DataSet();
//                    ds = reader.AsDataSet();
//                    reader.Close();

//                    int rowCount = ds.Tables[0].Rows.Count;
//                    DataTable serviceDetails = ds.Tables[0];

//                    int k = 0;
//                    if (ds != null && ds.Tables.Count > 0)
//                    {

//                        string BranchName = serviceDetails.Rows[0][0].ToString();
//                        string CompanyName = serviceDetails.Rows[0][1].ToString();
//                        string Address = serviceDetails.Rows[0][2].ToString();
//                        string CountryName = serviceDetails.Rows[0][3].ToString();
//                        string StateName = serviceDetails.Rows[0][4].ToString();
//                        string LgaName = serviceDetails.Rows[0][5].ToString();


//                        if (BranchName != "BranchName" || CompanyName != "CompanyName"
//                        || Address != "Address" || CountryName != "CountryName" || StateName != "StateName" || LgaName != "LgaName")
//                        {
//                            response.ResponseCode = "08";
//                            response.ResponseMessage = "File header not in the Right format";
//                            return response;
//                        }
//                        else
//                        {
//                            for (int row = 1; row < serviceDetails.Rows.Count; row++)
//                            {

//                                string branchName = serviceDetails.Rows[row][0].ToString();
//                                var Company = await _companyrepository.GetCompanyByName(serviceDetails.Rows[row][1].ToString());
//                                string address = serviceDetails.Rows[row][2].ToString();
//                                var Country = await _CountryRepository.GetCountryByName(serviceDetails.Rows[row][3].ToString());
//                                var State = await _StateRepository.GetStateByName(serviceDetails.Rows[row][4].ToString());
//                                var Lga = await _LgaRepository.GetLgaByName(serviceDetails.Rows[row][5].ToString());

//                                long CompanyId = Company.CompanyId;
//                                long CountryId = Country.CountryID;
//                                long StateID = State.StateID;
//                                long LgaID = Lga.LGAID;


//                                var branchrequest = new CreateBranchDTO
//                                {
//                                    BranchName = branchName,
//                                    CompanyID = CompanyId,
//                                    Address = address,
//                                    CountryID = CountryId,
//                                    StateID = StateID,
//                                    LgaID = LgaID

//                                };

//                                var branchrequester = new RequesterInfo
//                                {
//                                    Username = requester.Username,
//                                    UserId = requester.UserId,
//                                    RoleId = requester.RoleId,
//                                    IpAddress = requester.IpAddress,
//                                    Port = requester.Port,


//                                };

//                                var resp = await CreateBranch(branchrequest, branchrequester);


//                                if (resp.ResponseCode == "00")
//                                {
//                                    k++;
//                                }
//                                else
//                                {
//                                    errorOutput.Append($"Row {row} failed due to {resp.ResponseMessage}" + "\n");
//                                }
//                            }
//                        }

//                    }



//                    if (k == rowCount - 1)
//                    {
//                        response.ResponseCode = "00";
//                        response.ResponseMessage = "All record inserted successfully";



//                        return response;
//                    }
//                    else
//                    {
//                        response.ResponseCode = "02";
//                        response.ResponseMessage = errorOutput.ToString();



//                        return response;
//                    }
//                }


//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Exception Occured ==> {ex.Message}");
//                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
//                response.ResponseMessage = "Exception occured";
//                response.Data = null;



//                return response;
//            }
//        }
//    }
//}
