using Com.XpressPayments.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.Group
{
    public  interface IGroupRepository
    {
        Task<dynamic> CreateGroup(CreateGroupDTO Creategroup, string createdbyUserEmail);
        Task<dynamic> UpdateGroup(UpdateGroupDTO group, string updatedbyUserEmail);
        Task<dynamic> DeleteGroup(DeleteGroupDTO group, string deletedbyUserEmail);
        Task<IEnumerable<GroupDTO>> GetAllActiveGroup();
        Task<IEnumerable<GroupDTO>> GetAllGroup();
        Task<GroupDTO> GetGroupById(long GroupID);
        Task<GroupDTO> GetGroupByName(string GroupName);
        Task<GroupDTO> GetGroupByCompany(string GroupName, int companyId);
        Task<IEnumerable<GroupDTO>> GetAllGroupCompanyId(long companyId);
    }
}
