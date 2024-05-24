using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FileUploadDownloader.FileInterfaces;
using System.Diagnostics;
using System.Net.Mime;
using Microsoft.Extensions.FileProviders;

namespace FileUploadDownloader
{
    /// <summary>
    /// Class designed for small file uploads and downloads
    /// </summary>
    public class BufferedFileController : Controller
    {
        readonly IBufferedFileService _bufferedFileUploadInterface;
        private readonly IFileProvider _fileProvider;

        public BufferedFileController(IBufferedFileService bufferedFileUploadInterface, IFileProvider provider)
        {
            _bufferedFileUploadInterface = bufferedFileUploadInterface;
            _fileProvider = provider;
        }

        // GET: StreamFileUploadController
        /// <summary>
        /// Returns a list of hyperlinked files to download.
        /// </summary>
        /// <param name="fileName">Gets the file name from whatever path the DownloadFile extension method has</param>
        /// <returns>Returns view.</returns>
        public ActionResult Index()
        {
            var fileModels = _bufferedFileUploadInterface.GetFileModels();
            return View(fileModels);
        }

        public async Task<IActionResult> Download(string filename)
        {
            byte[] fileBytes = await _bufferedFileUploadInterface.DownloadFile(filename);

            return File(fileBytes, "application/octet-stream", filename);
        }

        // GET: StreamFileUploadController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StreamFileUploadController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StreamFileUploadController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormFile file)
        {
            try
            {
                if (await _bufferedFileUploadInterface.UploadFile(file))
                {
                    ViewBag.Message = "File Upload Successful";
                }
                else
                {
                    ViewBag.Message = "File Upload Failed";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "File Upload Failed";
                Debug.WriteLine("File Upload Failed" + ex);
            }
            return View();
        }

        // GET: StreamFileUploadController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StreamFileUploadController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StreamFileUploadController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StreamFileUploadController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
