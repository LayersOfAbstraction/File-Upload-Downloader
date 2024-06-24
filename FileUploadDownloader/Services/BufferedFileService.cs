using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using FileUploadDownloader.FileInterfaces;
using FileUploadDownloader.FileViewModels;
using Microsoft.AspNetCore.Routing.Constraints;

namespace FileUploadDownloader.Services
{
    public class BufferedFileService : IBufferedFileService
    {        
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileProvider _fileProvider;

        public BufferedFileService(IWebHostEnvironment webHostEnvironment, IFileProvider provider)
        {
            _webHostEnvironment = webHostEnvironment;
            _fileProvider = provider;
        }

        public List<FileModel> GetFileModels()
        {
            var fileModels = new List<FileModel>();
            foreach (var item in _fileProvider.GetDirectoryContents(""))
            {
                fileModels.Add(new FileModel()
                {
                    FileName = item.Name,
                    FileExtension = item.PhysicalPath
                });
            }
            return fileModels;
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

        public async Task<byte[]> DownloadFile(string fileName)
        {
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);
            byte[] fileBytes;

            using (var sourceStream = new FileStream(path, FileMode.Open))
            {
                fileBytes = new byte[sourceStream.Length];
                await sourceStream.ReadAsync(fileBytes, 0, fileBytes.Length);
            }

            return fileBytes;
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

        /* PROBLEM: To display the details of a photo
         * GET file view model where file view photo is equal to ID.
         * Get extension.
         * SET PixelCount = 0
         * Create Canvas 1024 * 1024px.          
         * IF Extension authorised               
         *      Foreach pixel in PixelArray
         *          PixelCount = pixel
         *          
         *          IF pixelCount > Canvas
         *              Resize pixels.
        */

        // PROBLEM: To delete a photo from view
        //
        //

        // Problem: To Update a photo 
        //
        //
    }
}
