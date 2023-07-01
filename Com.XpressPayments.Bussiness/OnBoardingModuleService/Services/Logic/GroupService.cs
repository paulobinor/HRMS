using Com.XpressPayments.Bussiness.Services.ILogic;
using Com.XpressPayments.Data.DTOs;
using Com.XpressPayments.Data.Enums;
using Com.XpressPayments.Data.GenericResponse;
using Com.XpressPayments.Data.Repositories.Company.IRepository;
using Com.XpressPayments.Data.Repositories.Group;
using Com.XpressPayments.Data.Repositories.HOD;
using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.Logic
{
    public class GroupService : IGroupService
    {
        private readonly IAuditLog _audit;

        private readonly ILogger<GroupService> _logger;
        //private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyrepository;
        private readonly IGroupRepository _GroupRepository;
        private readonly IHODRepository _hODRepository;

        public GroupService(/*IConfiguration configuration*/ IAccountRepository accountRepository, ILogger<GroupService> logger,
            IGroupRepository GroupRepository, IAuditLog audit, ICompanyRepository companyrepository, IHODRepository hODRepository)
        {
            _audit = audit;

            _logger = logger;
            //_configuration = configuration;
            _accountRepository = accountRepository;
            _GroupRepository = GroupRepository;
            _companyrepository = companyrepository;
            _hODRepository = hODRepository;
        }

        public async Task<BaseResponse> CreateGroup(CreateGroupDTO GroupDto, RequesterInfo requester)
        {
            var response = new BaseResponse();
            try
            {
                string createdbyUserEmail = requester.Username;
                string createdbyUserId = requester.UserId.ToString();
                string RoleId = requester.RoleId.ToString();

                var ipAddress = requester.IpAddress.ToString();
                var port = requester.Port.ToString();

                var requesterInfo = await _accountRepository.FindUser(createdbyUserEmail);
                if (null == requesterInfo)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Requester information cannot be found.";
                    return response;
                }


                if (Convert.ToInt32(RoleId) != 2)
                {
                    if (Convert.ToInt32(RoleId) != 4)
                    {
                        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                        return response;

                    }

                }

                //validate DepartmentDto payload here 
                if (String.IsNullOrEmpty(GroupDto.GroupName) || GroupDto.CompanyID <= 0 )
            
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Please ensure all required fields are entered.";
                    return response;
                }

                var isExistsComp = await _companyrepository.GetCompanyById(GroupDto.CompanyID);
                if (null == isExistsComp)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Invalid Company suplied.";
                    return response;
                }
                else
                {
                    if (isExistsComp.IsDeleted)
                    {
                        response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"The Company suplied is already deleted, HOD cannot be created under it.";
                        return response;
                    }
                }

                //GroupDto.GroupName = $"{GroupDto.GroupName} ({isExistsComp.CompanyName})";

                var isExists = await _GroupRepository.GetGroupByCompany(GroupDto.GroupName, (int)GroupDto.CompanyID);
                if (null != isExists)
                {
                    response.ResponseCode = ResponseCode.DuplicateError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Group with name : {GroupDto.GroupName} already exists for this Company.";
                    return response;
                }

                dynamic resp = await _GroupRepository.CreateGroup(GroupDto, createdbyUserEmail);
                if (resp > 0)
                {
                    //update action performed into audit log here

                    var group = await _GroupRepository.GetGroupByName(GroupDto.GroupName);

                    response.Data = group;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Group created successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "An error occured while Creating Group. Please contact admin.";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: CreateGroup ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: CreateGroup ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> CreateGroupBulkUpload(IFormFile payload, RequesterInfo requester)
        {
            //check if us
            StringBuilder errorOutput = new StringBuilder();
            var response = new BaseResponse();
            try
            {
                if (payload == null || payload.Length <= 0)
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "No file for Upload";
                    return response;
                }
                else if (!Path.GetExtension(payload.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    response.ResponseCode = "08";
                    response.ResponseMessage = "File not an Excel Format";
                    return response;
                }
                else
                {
                    var stream = new MemoryStream();
                    await payload.CopyToAsync(stream);

                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    var reader = ExcelReaderFactory.CreateReader(stream);
                    DataSet ds = new DataSet();
                    ds = reader.AsDataSet();
                    reader.Close();

                    int rowCount = ds.Tables[0].Rows.Count;
                    DataTable serviceDetails = ds.Tables[0];

                    int k = 0;
                    if (ds != null && ds.Tables.Count > 0)
                    {

                        string GroupName = serviceDetails.Rows[0][0].ToString();
                        string HodName = serviceDetails.Rows[0][1].ToString();
                        string CompanyName = serviceDetails.Rows[0][2].ToString();


                        if (GroupName != "GroupName" || HodName != "HodName"
                        || CompanyName != "CompanyName")

                        {
                            response.ResponseCode = "08";
                            response.ResponseMessage = "File header not in the Right format";
                            return response;
                        }
                        else
                        {
                            for (int row = 1; row < serviceDetails.Rows.Count; row++)
                            {

                                string groupName = serviceDetails.Rows[row][0].ToString();
                                var hodName = await _hODRepository.GetHODByName(serviceDetails.Rows[row][1].ToString());
                                var company = await _companyrepository.GetCompanyByName(serviceDetails.Rows[row][2].ToString());

                                long hodID = hodName.HodID;
                                long companyID = company.CompanyId;


                                var grouprequest = new CreateGroupDTO
                                {
                                    GroupName = groupName,
                                    HodID = hodID,
                                    CompanyID = companyID


                                };

                                var grouprequester = new RequesterInfo
                                {
                                    Username = requester.Username,
                                    UserId = requester.UserId,
                                    RoleId = requester.RoleId,
                                    IpAddress = requester.IpAddress,
                                    Port = requester.Port,


                                };

                                var resp = await CreateGroup(grouprequest, grouprequester);


                                if (resp.ResponseCode == "00")
                                {
                                    k++;
                                }
                                else
                                {
                                    errorOutput.Append($"Row {row} failed due to {resp.ResponseMessage}" + "\n");
                                }
                            }
                        }

                    }



                    if (k == rowCount - 1)
                    {
                        response.ResponseCode = "00";
                        response.ResponseMessage = "All record inserted successfully";



                        return response;
                    }
                    else
                    {
                        response.ResponseCode = "02";
                        response.ResponseMessage = errorOutput.ToString();



                        return response;
                    }
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Exception occured";
                response.Data = null;



                return response;
            }
        }

        public async Task<BaseResponse> UpdateGroup(UpdateGroupDTO updateDto, RequesterInfo requester)
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


                if (Convert.ToInt32(RoleId) != 2)
                {
                    if (Convert.ToInt32(RoleId) != 4)
                    {
                        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                        return response;

                    }

                }

                //validate DepartmentDto payload here 
                if (String.IsNullOrEmpty(updateDto.GroupName) || updateDto.CompanyID <= 0
                    || updateDto.GroupID <= 0)
                {
                    response.ResponseCode = ResponseCode.ValidationError.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"Please ensure all required fields are entered.";
                    return response;
                }

                var group = await _GroupRepository.GetGroupById(updateDto.GroupID);
                if (null == group)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "No record found for the specified Group";
                    response.Data = null;
                    return response;
                }

                dynamic resp = await _GroupRepository.UpdateGroup(updateDto, requesterUserEmail);
                if (resp > 0)
                {
                    //update action performed into audit log here

                    var updatedGroup = await _GroupRepository.GetGroupById(updateDto.GroupID);

                    _logger.LogInformation("Group updated successfully.");
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Group updated successfully.";
                    response.Data = updatedGroup;
                    return response;
                }
                response.ResponseCode = ResponseCode.Exception.ToString();
                response.ResponseMessage = "An error occurred while updating Group.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: UpdateGroupDTO ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: UpdateGroupDTO ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> DeleteGroup(DeleteGroupDTO deleteDto, RequesterInfo requester)
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

                if (Convert.ToInt32(RoleId) != 2)
                {
                    if (Convert.ToInt32(RoleId) != 4)
                    {
                        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                        return response;

                    }

                }

                if (deleteDto.GroupID == 1)
                {
                    response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = $"System Default group cannot be deleted.";
                    return response;
                }

                var group = await _GroupRepository.GetGroupById(deleteDto.GroupID);
                if (null != group)
                {
                    dynamic resp = await _GroupRepository.DeleteGroup(deleteDto, requesterUserEmail);
                    if (resp > 0)
                    {
                        //update action performed into audit log here

                        var DeletedGroup = await _GroupRepository.GetGroupById(deleteDto.GroupID);

                        _logger.LogInformation($"Group with name: {DeletedGroup.GroupName} Deleted successfully.");
                        response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Group with name: {DeletedGroup.GroupName} Deleted successfully.";
                        response.Data = null;
                        return response;

                    }
                    response.ResponseCode = ResponseCode.Exception.ToString();
                    response.ResponseMessage = "An error occurred while deleting Group.";
                    response.Data = null;
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No record found for the specified Group";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: DeleteGroup ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: DeleteGroup ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllActiveGroup(RequesterInfo requester)
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
                    if (Convert.ToInt32(RoleId) != 4)
                    {
                        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                        return response;

                    }

                }

                //update action performed into audit log here

                var group = await _GroupRepository.GetAllActiveGroup();

                if (group.Any())
                {
                    response.Data = group;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Group fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No Group found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllActiveGroup() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllActiveGroup() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetAllGroup(RequesterInfo requester)
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
                    if (Convert.ToInt32(RoleId) != 4)
                    {
                        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                        return response;

                    }

                }

                //update action performed into audit log here

                var group = await _GroupRepository.GetAllGroup();

                if (group.Any())
                {
                    response.Data = group;
                    response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Group fetched successfully.";
                    return response;
                }
                response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "No Group found.";
                response.Data = null;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllGroup() ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllGroup() ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetGroupbyId(long GroupID, RequesterInfo requester)
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
                    if (Convert.ToInt32(RoleId) != 4)
                    {
                        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                        return response;

                    }

                }

                var group = await _GroupRepository.GetGroupById(GroupID);

                if (group == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Group not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = group;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Group fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetGroupById(long GroupID) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetGroupById(long GroupID) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

        public async Task<BaseResponse> GetGroupbyCompanyId(long companyId, RequesterInfo requester)
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
                    if (Convert.ToInt32(RoleId) != 4)
                    {
                        response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                        response.ResponseMessage = $"Your role is not authorized to carry out this action.";
                        return response;

                    }

                }

                var group = await _GroupRepository.GetAllGroupCompanyId(companyId);

                if (group == null)
                {
                    response.ResponseCode = ResponseCode.NotFound.ToString("D").PadLeft(2, '0');
                    response.ResponseMessage = "Group not found.";
                    response.Data = null;
                    return response;
                }

                //update action performed into audit log here

                response.Data = group;
                response.ResponseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = "Group fetched successfully.";
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllGroupCompanyId(long companyId) ==> {ex.Message}");
                response.ResponseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0');
                response.ResponseMessage = $"Exception Occured: GetAllGroupCompanyId(long companyId) ==> {ex.Message}";
                response.Data = null;
                return response;
            }
        }

    }
}
