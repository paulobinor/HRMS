using hrms_be_backend_common.Communication;
using hrms_be_backend_data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_business.ILogic
{
    public interface ITaxService
    {
        Task<ExecutedResult<TaxView>> GetTaxDetails(string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
    }
}
