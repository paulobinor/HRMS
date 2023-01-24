using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpressHRMS.Data.DTO
{
    public class BanksDTO
    {
        public int BankID { get; set; }
        public string CbnCode { get; set; }
        public string BankName { get; set; }
    }
        public class CreateBankDTO
        {
            public string CbnCode { get; set; }
            public string BankName { get; set; }
        }
        public class DeleteBankDTO
        {
            public double BankID { get; set; }


        }

        public class UpdateBankDTO
        {
            public int BankID { get; set; }
            public string CbnCode { get; set; }
            public string BankName { get; set; }
        }

}
