namespace FileUploadDownloader.FileViewModels
{
    public class FileDownloadModel
    {
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string ContentType { get; set; }
        public bool DownloadResult { get; set; }
    }
}