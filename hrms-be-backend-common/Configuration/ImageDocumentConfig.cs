namespace hrms_be_backend_common.Configuration
{
    public class ImageDocumentConfig
    {
        public string AllowedFileExtensions { get; set; }
        public int MaximumSizeInKilobyte { get; set; }
        public string FolderName { get; set; }
        public string FolderUrl { get; set; }
    }
}
