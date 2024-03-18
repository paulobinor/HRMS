using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_common.Configuration;
using hrms_be_backend_data.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace hrms_be_backend_business.Logic
{
    public class FileUploaderService : IFileUploaderService
    {
        private readonly DocumentConfig _documentConfig;
        private readonly ImageDocumentConfig _imageDocumentConfig;
        private readonly ILogger<FileUploaderService> _logger;
        private IHostingEnvironment _hostEnvironment;
        private readonly IMailService _mailService;
        public FileUploaderService(IOptions<DocumentConfig> documentConfig, IOptions<ImageDocumentConfig> imageDocumentConfig, ILogger<FileUploaderService> logger, IHostingEnvironment hostEnvironment, IMailService mailService)
        {
            _documentConfig = documentConfig.Value;
            _imageDocumentConfig = imageDocumentConfig.Value;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            _mailService = mailService;
        }

        public async Task<ExecutedResult<string>> UploadBase64ToImage(string base64File, string MID, string OperationName)
        {

            string fileName;
            try
            {
                fileName = DateTime.Now.Ticks + ".jpg";
                string folder = $"{MID}\\{OperationName}\\{DateTime.Now.Year}\\{DateTime.Now.ToString("MMMM")}\\{DateTime.Now.ToString("dddd")}";

                _hostEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), $"{_imageDocumentConfig.FolderName}\\{folder}");
                var path = _hostEnvironment.WebRootPath;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path = Path.Combine(path, fileName);
                var response = Base64ToImage(path, base64File);

                var returnPath = $"{_imageDocumentConfig.FolderUrl}/{folder}/{fileName}";
                if (response != "Success")
                {
                    return new ExecutedResult<string>() { responseMessage = "Uploaded Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
                }
                return new ExecutedResult<string>() { responseMessage = response, responseCode = ((int)ResponseCode.Ok).ToString(), data = returnPath };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), new { Controller = "FileUploaderService", Method = "UploadImageBase64" });
                return new ExecutedResult<string>() { responseMessage = ResponseCode.ProcessingError.ToString(), responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> UploadBase64ToPdf(string base64File, string MID, string OperationName)
        {

            string fileName;
            try
            {
                fileName = DateTime.Now.Ticks + ".pdf";
                string folder = $"{MID}\\{OperationName}\\{DateTime.Now.Year}\\{DateTime.Now.ToString("MMMM")}\\{DateTime.Now.ToString("dddd")}";

                _hostEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), $"{_documentConfig.FolderName}\\{folder}");
                var path = _hostEnvironment.WebRootPath;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path = Path.Combine(path, fileName);
                var response = Base64ToPdf(path, base64File);

                var returnPath = $"{_documentConfig.FolderUrl}/{folder}/{fileName}";
                if (response != "Success")
                {
                    return new ExecutedResult<string>() { responseMessage = "Uploaded Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
                }
                return new ExecutedResult<string>() { responseMessage = response, responseCode = ((int)ResponseCode.Ok).ToString(), data = returnPath };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), new { Controller = "FileUploaderService", Method = "UploadImageExcel" });
                return new ExecutedResult<string>() { responseMessage = ResponseCode.ProcessingError.ToString(), responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
            }
        }
        private string Base64ToImage(string folderPath, string base64String)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64String);

                //Image image;
                //using (MemoryStream ms = new MemoryStream(bytes))
                //{
                //    image = Image.FromStream(ms);
                //}

                //string filePath = folderPath;
                //image.Save(filePath);
                return "Success";
            }
            catch (Exception ex)
            {
                return "Unable to save image";
            }
        }
        private string Base64ToPdf(string folderPath, string base64String)
        {
            try
            {
                base64String = base64String.Replace("data:application/pdf;base64,", "");
                byte[] pdfBytes = Convert.FromBase64String(base64String);

                File.WriteAllBytes(folderPath, pdfBytes);

                return "Success";
            }
            catch (Exception ex)
            {
                return "Unable to save document";
            }
        }
        private string Base64ToFile(string folderPath, string base64String)
        {
            try
            {
                string mystr = base64String.Replace("base64,", string.Empty);


                var testb = Convert.FromBase64String(mystr);

                System.IO.File.WriteAllBytes(folderPath, testb);

                return "Success";
            }
            catch (Exception ex)
            {
                return "Unable to save document";
            }
        }
    }
}
