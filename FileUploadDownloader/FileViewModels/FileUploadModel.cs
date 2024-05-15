namespace FileUploadDownloader.FileViewModels
{
    public class FileUploadModel
    {
        public IFormFile? File { get; set; }
        public string? FileName { get; set; }
        public string? FileExtension { get; set; }
        public long FileSize { get; set; }
        public bool UploadResult { get; set; }
    }
}