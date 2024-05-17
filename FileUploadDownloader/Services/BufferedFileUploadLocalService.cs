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

        public BufferedFileUploadLocalService(IWebHostEnvironment webHostEnvironment, ILogger<BufferedFileUploadLocalService> logger)
        {
            _webHostEnvironment = webHostEnvironment;
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
                FileModel fileUploadModel = new FileModel
                {
                    File = file,
                    FileName = file.FileName,
                    FileExtension = Path.GetExtension(file.FileName),
                    FileSize = file.Length
                };
                //Validate current file extension against dictionary.
                var allowedExtensions = MimeTypes.GetMimeTypes();
                
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
            catch (Exception ex) 
            {
                throw new Exception("File Copy Failed", ex);
            }
        }

        // Get content type
        public string GetContentType(string path)
        {
            var allowedExtensions = MimeTypes.GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            if (allowedExtensions.ContainsKey(path))
            {
                return allowedExtensions[ext];
            }            
            
            else
            {
                throw new Exception("Extension invalid");
            }            
        }
    }
}
