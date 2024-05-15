using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using FileUploadDownloader.FileInterfaces;
using FileUploadDownloader.FileViewModels;

namespace FileUploadDownloader.Services
{
    public class BufferedFileUploadLocalService : IBufferedFileUploadService
    {        
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<BufferedFileUploadLocalService> _logger;

        public BufferedFileUploadLocalService(IWebHostEnvironment webHostEnvironment, ILogger<BufferedFileUploadLocalService> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }
        /// <summary>
        /// Method uploads file to the wwwroot/Uploads subfolder.
        /// </summary>
        /// <param name="file">Represents a file sent with the HttpRequest.</param>
        /// <returns>Executes upload of file</returns>
        /// <exception cref="Exception">File Copy Failed</exception>
        public async Task<bool> UploadFile(IFormFile file)
        {
            try
            {
                //Bind service to FileUploadModel.
                FileUploadModel fileUploadModel = new FileUploadModel
                {
                    File = file,
                    FileName = file.FileName,
                    FileExtension = Path.GetExtension(file.FileName),
                    FileSize = file.Length
                };
                //Validate current file extension against dictionary.
                var allowedExtensions = MimeTypes.GetMimeTypes();
                if (allowedExtensions.ContainsKey(fileUploadModel.FileExtension))
                {
                    var maxFileSize = 5 * 1024 * 1024; // 5MB
                    if (fileUploadModel.FileSize > maxFileSize)
                    {
                        return false;
                    }

                    // Save the file to the server.
                    var fileName = Path.GetFileName(fileUploadModel.File.FileName);
                    //Get the root path of your application via IWebHostEnvironment.
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await fileUploadModel.File.CopyToAsync(stream);
                    }
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex) 
            {
                throw new Exception("File Copy Failed", ex);
            }
        }


        public async Task<FileDownloadModel?> DownloadFile(string fileName)
        {
            try
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);
                if (fileName.Length < 0)
                {
                    return null;
                }

                await using var stream = new FileStream(path,FileMode.Open);
                using var memoryStream = new MemoryStream();
                {
                    await stream.CopyToAsync(memoryStream);
                }
                return new FileDownloadModel
                {
                    FileName = fileName,
                    FileContent = memoryStream.ToArray(),
                    ContentType = GetContentType(fileName),
                    DownloadResult = true
                };
            }

            catch (Exception ex)
            {
                throw new Exception("File Download Failed", ex);
            }
        }

        // Get content type
        public string GetContentType(string path)
        {
            var allowedExtensions = MimeTypes.GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return allowedExtensions[ext];
        }
    }
}
