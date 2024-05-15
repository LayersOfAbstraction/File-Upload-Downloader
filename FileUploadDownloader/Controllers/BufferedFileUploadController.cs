using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FileUploadDownloader.Interfaces;
using System.Diagnostics;
using System.Net.Mime;

namespace FileUploadDownloader
{
    /// <summary>
    /// Class designed for small file uploads and downloads
    /// </summary>
    public class BufferedFileUploadController : Controller
    {
        readonly IBufferedFileUploadService _bufferedFileUploadService;

        public BufferedFileUploadController(IBufferedFileUploadService bufferedFileUploadService)
        {
            _bufferedFileUploadService = bufferedFileUploadService;
        }

        // GET: StreamFileUploadController
        /// <summary>
        /// Returns a list of hyperlinked files to download.
        /// </summary>
        /// <param name="fileName">Gets the file name from whatever path the DownloadFile extension method has</param>
        /// <returns>Returns view.</returns>
        public async Task<ActionResult> Index(string fileName)
        {
            try
            {

                var fileDownloadModel = await _bufferedFileUploadService.DownloadFile(fileName);
                if (fileDownloadModel != null)
                {
                    //Chains the synchronous method of GetContentType to return the  
                    var contentType = _bufferedFileUploadService.GetContentType(fileName);
                    return File(fileDownloadModel.FileContent, fileDownloadModel.ContentType, fileDownloadModel.FileName);
                }
                else
                {
                    ViewBag.Message = "File Download Failed";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "File Download Failed";
                Debug.WriteLine("File Download Failed" + ex);
            }
            return View();
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
                if (await _bufferedFileUploadService.UploadFile(file))
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
