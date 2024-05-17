using FileUploadDownloader.FileViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadDownloader.FileInterfaces
{
    public interface IBufferedFileUploadService
    {
        Task<bool> UploadFile(IFormFile file);
        Task<byte[]> DownloadFile(string filename);
        List<FileModel> GetFileModels();
    }
}
