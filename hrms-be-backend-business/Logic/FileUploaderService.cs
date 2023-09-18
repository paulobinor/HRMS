using hrms_be_backend_business.ILogic;
using hrms_be_backend_common.Communication;
using hrms_be_backend_data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_business.Logic
{
    public class FileUploaderService : IFileUploaderService
    {
        private readonly MerchantDocumentConfig _merchantDocumentConfig;
        private readonly TransactionBeneficiariesDocumentConfig _transactionBeneficiariesDocumentConfig;
        private readonly ILogger<FileUploaderService> _logger;
        private IHostingEnvironment _hostEnvironment;
        private readonly IMailService _mailService;
        public FileUploaderService(IOptions<MerchantDocumentConfig> merchantDocumentConfig, IOptions<TransactionBeneficiariesDocumentConfig> transactionBeneficiariesDocumentConfig, ILogger<FileUploaderService> logger, IHostingEnvironment hostEnvironment, IMailService mailService)
        {
            _merchantDocumentConfig = merchantDocumentConfig.Value;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
            _transactionBeneficiariesDocumentConfig = transactionBeneficiariesDocumentConfig.Value;
            _mailService = mailService;
        }
        public async Task<ExecutedResult<string>> UploadDocument(IFormFile UploadedFile, string MID, string OperationName)
        {

            string fileName;
            try
            {
                var path = string.Empty;
                var extension = "." + UploadedFile.FileName.Split('.')[UploadedFile.FileName.Split('.').Length - 1];
                if (!_merchantDocumentConfig.AllowedFileExtensions.Contains(extension.Remove(0)))
                {
                    return new ExecutedResult<string>() { responseMessage = "Invalid File", responseCode = ((int)ResponseCode.INVALID_REQUEST).ToString(), data = null };
                }

                fileName = DateTime.Now.Ticks + extension;
                string folder = $"{MID}\\{OperationName}\\{DateTime.Now.Year}\\{DateTime.Now.ToString("MMMM")}\\{DateTime.Now.ToString("dddd")}";

                _hostEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), $"{_merchantDocumentConfig.FolderName}\\{folder}");
                path = _hostEnvironment.WebRootPath;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path = Path.Combine(path, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await UploadedFile.CopyToAsync(stream);
                }
                // var filePathToSave = $"{_merchantDocumentConfig.FolderName}/{MID}/{fileName}";

                var returnPath = $"{_merchantDocumentConfig.FolderUrl}/{folder}/{fileName}";
                returnPath = returnPath.Replace("///", "/").Replace("//", "/");
                returnPath = returnPath.Replace("'\'", "/");

                return new ExecutedResult<string>() { responseMessage = "Uploaded Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = returnPath };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), new { Controller = "FileUploaderService", Method = "UploadMerchantDocument" });
                var msgBody = $"Hi, <br/> <br/> below is an exception detail occured <br/> <hr/>{ex}";
                _mailService.SendNotificationEmailAsync("Payout Exception", msgBody, AppOperationsData.Application_Operation_Management, XpressUserPriviledgeData.Checker, null);
                return new ExecutedResult<string>() { responseMessage = ResponseCode.ProcessingError.ToString(), responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
            }
        }
        public async Task<ExecutedResult<string>> UploadBase64ToImage(string base64File, string MID, string OperationName)
        {

            string fileName;
            try
            {
                fileName = DateTime.Now.Ticks + ".jpg";
                string folder = $"{MID}\\{OperationName}\\{DateTime.Now.Year}\\{DateTime.Now.ToString("MMMM")}\\{DateTime.Now.ToString("dddd")}";

                _hostEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), $"{_merchantDocumentConfig.FolderName}\\{folder}");
                var path = _hostEnvironment.WebRootPath;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path = Path.Combine(path, fileName);
                var response = Base64ToImage(path, base64File);

                var returnPath = $"{_merchantDocumentConfig.FolderUrl}/{folder}/{fileName}";
                if (response != "Success")
                {
                    return new ExecutedResult<string>() { responseMessage = "Uploaded Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
                }
                return new ExecutedResult<string>() { responseMessage = response, responseCode = ((int)ResponseCode.Ok).ToString(), data = returnPath };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), new { Controller = "FileUploaderService", Method = "UploadImageBase64" });
                var msgBody = $"Hi, <br/> <br/> below is an exception detail occured <br/> <hr/>{ex}";
                _mailService.SendNotificationEmailAsync("Payout Exception", msgBody, AppOperationsData.Application_Operation_Management, XpressUserPriviledgeData.Checker, null);
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

                _hostEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), $"{_merchantDocumentConfig.FolderName}\\{folder}");
                var path = _hostEnvironment.WebRootPath;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path = Path.Combine(path, fileName);
                var response = Base64ToPdf(path, base64File);

                var returnPath = $"{_merchantDocumentConfig.FolderUrl}/{folder}/{fileName}";
                if (response != "Success")
                {
                    return new ExecutedResult<string>() { responseMessage = "Uploaded Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
                }
                return new ExecutedResult<string>() { responseMessage = response, responseCode = ((int)ResponseCode.Ok).ToString(), data = returnPath };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), new { Controller = "FileUploaderService", Method = "UploadImageExcel" });
                var msgBody = $"Hi, <br/> <br/> below is an exception detail occured <br/> <hr/>{ex}";
                _mailService.SendNotificationEmailAsync("Payout Exception", msgBody, AppOperationsData.Application_Operation_Management, XpressUserPriviledgeData.Checker, null);
                return new ExecutedResult<string>() { responseMessage = ResponseCode.ProcessingError.ToString(), responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
            }
        }
        private string Base64ToImage(string folderPath, string base64String)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64String);

                System.Drawing.Image image;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = System.Drawing.Image.FromStream(ms);
                }

                image.Save(folderPath, System.Drawing.Imaging.ImageFormat.Jpeg);
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
                byte[] bytes = Convert.FromBase64String(base64String);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    Document document = new Document();
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(folderPath, FileMode.Create));
                    document.Open();
                    PdfContentByte cb = writer.DirectContent;
                    PdfImportedPage page;
                    PdfReader reader = new PdfReader(ms);
                    int pageCount = reader.NumberOfPages;
                    for (int i = 1; i <= pageCount; i++)
                    {
                        page = writer.GetImportedPage(reader, i);
                        cb.AddTemplate(page, 0, 0);
                        document.NewPage();
                    }
                    document.Close();
                }

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
