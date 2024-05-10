using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using FileUploadDownloader.Interfaces;

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
                string fileExtension = Path.GetExtension(file.FileName);
                //Validate current file extension against dictionary.
                var allowedExtensions = Models.MimeTypes.GetMimeTypes();
                if (allowedExtensions.ContainsKey(fileExtension))
                {
                    var maxFileSize = 5 * 1024 * 1024; // 5MB
                    if (file.Length > maxFileSize)
                    {
                        return false;
                    }

                    // Save the file to the server.
                    var fileName = Path.GetFileName(file.FileName);
                    //Get the root path of your application via IWebHostEnvironment.
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
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


        public async Task<byte[]?> DownloadFile(string fileName)
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
                return memoryStream.ToArray();
            }

            catch (Exception ex)
            {
                throw new Exception("File Download Failed", ex);
            }
        }

        // Get content type
        public string GetContentType(string path)
        {
            var allowedExtensions = Models.MimeTypes.GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return allowedExtensions[ext];
        }
    }
}
