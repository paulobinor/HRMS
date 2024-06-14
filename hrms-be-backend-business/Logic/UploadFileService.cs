using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.Enums;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
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
        //  private readonly IUploadFileService _uploadFileService;
        private readonly IHostEnvironment _hostingEnvironment;
        private readonly IWebHostEnvironment _webHostingEnvironment;
        private readonly IEmployeeService _employeeService;

        public UploadFileService(IConfiguration config, ILogger<UploadFileService> logger, IHostEnvironment hostingEnvironment, IEmployeeService employeeService, IWebHostEnvironment webHostingEnvironment)
        {
            _config = config;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _employeeService = employeeService;
            _webHostingEnvironment = webHostingEnvironment;
            //  _uploadFileService = uploadFileService;
        }

        public async Task<ExecutedResult<string>> UploadFile1(IFormFile file, string FullName)
        {
            try
            {
                string folderPath = _config["FileUploadConfig:UploadFolderPath"];

                _logger.LogInformation($"UploadFolderPath: {folderPath}");

                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("File is not selected or is empty.");
                }

                // Get the uploads folder path
                string uploadsFolder = Path.Combine(_hostingEnvironment.ContentRootPath, folderPath);
                _logger.LogInformation($"uploadsFolder: {uploadsFolder}");

                // Check if the folder exists, create it if not
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }


                string uniqueFileName = FullName + Path.GetExtension(file.FileName);

                _logger.LogInformation($"uniqueFileName: {uniqueFileName}");

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                _logger.LogInformation($"filePath: {filePath}");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                string urlPath = string.Empty;
                if (_webHostingEnvironment.IsDevelopment())
                {
                    _logger.LogInformation("This is a development environment as such we will return the content absolute root path");
                    urlPath = Path.Combine(_webHostingEnvironment.ContentRootPath, folderPath, uniqueFileName);

                }
                else
                {
                    _logger.LogInformation($"This is a production environment as such we will return the web root path: {_webHostingEnvironment.WebRootPath}");
                    urlPath = Path.Combine(_webHostingEnvironment.WebRootPath, folderPath, uniqueFileName);
                }
                _logger.LogInformation($"urlPath: {urlPath}");


                return new ExecutedResult<string>() { responseMessage = "File uploaded Successfully", responseCode = 00.ToString(), data = urlPath };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error uploading File", ex);
                return new ExecutedResult<string>() { responseMessage = "An error occurred", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
            }
        }

        private async Task<string> SaveFileAsync(IFormFile formFile)
        {
            var _folderPath = _config["FileUploadConfig:UploadFolderPath"];
            if (formFile == null || formFile.Length == 0)
                throw new ArgumentException("Invalid file.");

            var fileName = Path.GetFileName(formFile.FileName);
            var filePath = Path.Combine(_folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            // Return the URL path of the saved file (assuming your application is hosted on a web server)
            return $"/{_folderPath}/{fileName}".Replace("\\", "/");
        }

        public async Task<ExecutedResult<string>> UploadFile(IFormFile formFile, string FullName)
        {
            try
            {
                var uploadPath = _config["FileUploadConfig:UploadFolderPath"];
                var uploadBaseURL = _config["FileUploadConfig:FileUploadBaseURL"];
                var errorMessages = string.Empty;

                if (formFile == null || formFile.Length == 0)
                    errorMessages += "|file is required";


                // uploadPath = uploadPath + @"\LeaveRequests";
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);



                if (string.IsNullOrEmpty(errorMessages))
                {
                    var url = await SaveFileAsync(formFile);

                    //  using HttpClient httpClient = new HttpClient();
                    //  FileUploadRequest request = new FileUploadRequest
                    //  {
                    //      AppName = "HRMS",
                    //      UserId = FullName,
                    //      Image = formFile
                    //  };
                    //  MultipartFormDataContent formDataContent = new MultipartFormDataContent
                    //  {
                    //      { new StreamContent(request.Image.OpenReadStream()), "Image", request.Image.FileName },
                    //      { new StringContent(request.AppName), "AppName" },
                    //      { new StringContent(request.UserId), "UserId" }
                    //  };

                    //// string url = uploadBaseURL + "UploadFile";
                    //  HttpResponseMessage response = await httpClient.PostAsync(url, formDataContent);
                    //  if (response.StatusCode != HttpStatusCode.OK)
                    //  {
                    //      await response.Content.ReadAsStringAsync();
                    //      return new ExecutedResult<string>() { responseMessage = errorMessages, responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                    //  }
                    //  var uploadResponse = JsonConvert.DeserializeObject<FileResponse>(await response.Content.ReadAsStringAsync());
                    //  if (uploadResponse.ResponseCode != "00")
                    //  {
                    //      _logger.LogInformation("file upload failed");
                    //      return new ExecutedResult<string>() { responseMessage = errorMessages, responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                    //  }
                    //return new ExecutedResult<string>() { responseMessage = "File uploaded Successfully", responseCode = 00.ToString(), data = uploadResponse.UploadUrl };
                    return new ExecutedResult<string>() { responseMessage = "File uploaded Successfully", responseCode = 00.ToString(), data = url };
                }
                else
                {
                    return new ExecutedResult<string>() { responseMessage = errorMessages, responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error uploading file", ex);
                return new ExecutedResult<string>() { responseMessage = "An error occurred", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
            }
        }

    }
}
