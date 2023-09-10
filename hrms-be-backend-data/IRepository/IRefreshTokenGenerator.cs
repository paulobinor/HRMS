using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hrms_be_backend_data.IRepository
{
    public interface IRefreshTokenGenerator
    {
        string GenerateRefreshToken();
    }
}
