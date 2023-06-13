using Com.XpressPayments.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories
{
    public interface IRolesRepo
    {
        Task<RolesDTO> GetRolesByName(string RoleName);
    }
}
