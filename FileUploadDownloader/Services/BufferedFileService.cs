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
         * GET Upload folder
         * GET file view model where file view photo is equal to ID.
         * Get extension.
         * SET PixelByteCount = 0
         * Create Canvas 1024 * 1024px.          
         * IF Extension authorised               
         *      FOREACH pixel in PixelByteArray
         *          PixelCount = pixel
         *          
         *          IF PixelByteCount > Canvas
         *              Resize pixels.
         *              PRINT PixelByteArray[PixelByteCount]
         *          END IF
         *      END FOREACH
         *      END
        */


        public FileModel GetPiture(String fileName)
        {
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);
            byte[] fileBytes;
            FileModel fm = new FileModel();
            byte PixelByteCount = 0;
            var canvas = 1920 * 1080; //Pixels shown on screen
            if (fileName == fm.FileName)
            {
                foreach (byte pixel in fileBytes.Length[PixelByteCount])
                {

                }
            }
        }
        /*PROBLEM: To delete a photo from view
         * GET Upload folder
         * GET file view model where file view photo is equal to ID.
         * GET extension.
         */


        // Problem: To Update a photo 
        //
        //
    }
}
