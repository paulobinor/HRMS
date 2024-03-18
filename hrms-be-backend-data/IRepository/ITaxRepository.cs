using hrms_be_backend_data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.IRepository
{
    public interface ITaxRepository
    {
        Task<List<TaxPayableVm>> GetTaxPayable(long CompanyId);
    }
}
