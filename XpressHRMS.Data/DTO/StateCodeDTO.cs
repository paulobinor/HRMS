using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace XpressHRMS.Data.DTO
{
    public class StateCodeDTO
    {
        public int CountryID { get; set; }
        //[JsonIgnore]
        public int StateID { get; set; }
        public string StateName { get; set; }
    }
}
