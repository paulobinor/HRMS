﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public  class StateDTO
    {
        [JsonIgnore]
        public long CountryID { get; set; }
        public long StateID { get; set; }
        public string StateName { get; set; }
    }
}
