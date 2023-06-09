using Com.XpressPayments.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.Gender
{
    public  interface IGenderRepository
    {
        Task<IEnumerable<GenderDTO>> GetAllGender();
    }
}
