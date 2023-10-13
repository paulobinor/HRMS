﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class JobDescriptionDTO
    {
        public long JobDescriptionID { get; set; }
        public string JobDescriptionName { get; set; }
        public long CompanyID { get; set; }

        public DateTime Created_Date { get; set; }
        public string Created_By_User_Email { get; set; }

        public bool IsUpdated { get; set; }
        public DateTime? Updated_Date { get; set; }
        public string Updated_By_User_Email { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? Deleted_Date { get; set; }
        public string Deleted_By_User_Email { get; set; }
        public string Reasons_For_Delete { get; set; }
    }

    public class CreateJobDescriptionDTO
    {
        public string JobDescriptionName { get; set; }
        public long CompanyID { get; set; }

    }

    public class UpdateJobDescriptionDTO
    {
        public long JobDescriptionID { get; set; }
        public string JobDescriptionName { get; set; }
        public long CompanyID { get; set; }

    }

    public class DeletedJobDescriptionDTO
    {
        public long JobDescriptionID { get; set; }
        public string Reasons_For_Delete { get; set; }
    }
}