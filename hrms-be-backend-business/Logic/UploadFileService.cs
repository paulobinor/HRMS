using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Dynamic;
using System.Net;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace hrms_be_backend_business.Logic
{
    public class UploadFileService : IUploadFileService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<UploadFileService> _logger;
        private readonly IUploadFileService _uploadFileService;

        public UploadFileService(IConfiguration config, ILogger<UploadFileService> logger, IUploadFileService uploadFileService)
        {
            _config = config;
            _logger = logger;
            _uploadFileService = uploadFileService;
        }

        public async Task<ExecutedResult<string>> UploadFile(IFormFile formFile, long EmployeeID)
        {
            try
            {
                var uploadPath = _config["FileUploadConfig:UploadFolderPath"];
                var uploadBaseURL = _config["FileUploadConfig:FileUploadBaseURL"];
                var errorMessages = string.Empty;

                if (formFile == null || formFile.Length == 0)
                    errorMessages += "|Resignation letter is required";


                string url = uploadBaseURL + @"\LeaveRequests";

                if (!Directory.Exists(url))
                    Directory.CreateDirectory(url);

                if (string.IsNullOrEmpty(errorMessages))
                {
    
                    using HttpClient httpClient = new HttpClient();
                    FileUploadRequest request = new FileUploadRequest
                    {
                        AppName = "HRMS",
                        UserId = EmployeeID.ToString(),
                        Image = formFile
                    };
                    MultipartFormDataContent formDataContent = new MultipartFormDataContent
                    {
                        { new StreamContent(request.Image.OpenReadStream()), "Image", request.Image.FileName },
                        { new StringContent(request.AppName), "AppName" },
                        { new StringContent(request.UserId), "UserId" }
                    };

                    HttpResponseMessage response = await httpClient.PostAsync(url, formDataContent);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        await response.Content.ReadAsStringAsync();
                        return new ExecutedResult<string>() { responseMessage = errorMessages, responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                    }
                    var uploadResponse = JsonConvert.DeserializeObject<FileResponse>(await response.Content.ReadAsStringAsync());
                    if (uploadResponse.ResponseCode != "00")
                    {
                        _logger.LogInformation("file upload failed");
                        return new ExecutedResult<string>() { responseMessage = errorMessages, responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                    }
                    return new ExecutedResult<string>() { responseMessage = "File uploaded Successfully", responseCode = 00.ToString(), data = uploadResponse.UploadUrl };
                }
                else
                {
                    return new ExecutedResult<string>() { responseMessage = errorMessages, responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Submitting resignation", ex);
                return new ExecutedResult<string>() { responseMessage = "An error occurred", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
            }
        }

    }
}
