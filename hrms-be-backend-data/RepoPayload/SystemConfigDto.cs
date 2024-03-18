using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class SystemConfigDto
    {
        public int SystemConfigurationId { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }
    }
}
