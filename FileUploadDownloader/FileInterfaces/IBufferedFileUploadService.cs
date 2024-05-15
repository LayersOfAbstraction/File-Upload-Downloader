using FileUploadDownloader.FileViewModels;

namespace FileUploadDownloader.FileInterfaces
{
    public interface IBufferedFileUploadService
    {
        Task<bool> UploadFile(IFormFile file);
        Task<FileDownloadModel?> DownloadFile(string fileName);
        string GetContentType(string contentType);
    }
}
