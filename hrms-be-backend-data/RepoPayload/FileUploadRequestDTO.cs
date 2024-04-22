using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload
{
    public class FileUploadRequest
    {
        public string AppName { get; set; }

        public string UserId { get; set; }

        public IFormFile Image { get; set; }
    }
    public class FileResponse
    {
        public string ResponseCode { get; set; }

        public string ResponseMessage { get; set; }

        public string UploadUrl { get; set; }
    }
}
