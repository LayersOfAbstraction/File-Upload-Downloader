namespace FileUploadDownloader.Interfaces
{
    public interface IBufferedFileUploadService
    {
        Task<bool> UploadFile(IFormFile file);
        Task<byte[]?> DownloadFile(string fileName);
        string GetContentType(string contentType);
    }
}
