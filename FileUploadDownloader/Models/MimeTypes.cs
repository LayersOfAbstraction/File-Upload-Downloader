namespace FileUploadDownloader.Models
{
    public static class MimeTypes
    {
        public static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".pdf", "application/pdf"},
                {".doc", "application/msword"},
                {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".ppt", "application/vnd.ms-powerpoint"},
                {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
                {".txt", "text/plain"},
                {".rtf", "application/rtf"},
                {".csv", "text/csv"},
                {".bmp", "image/bmp"},
                {".tif", "image/tiff"},
                {".tiff", "image/tiff"},
                {".ico", "image/vnd.microsoft.icon"},
                {".svg", "image/svg+xml"},
                {".zip", "application/zip"},
                {".rar", "application/x-rar-compressed"},
                {".tar", "application/x-tar"},
                {".gz", "application/gzip"},
                {".7z", "application/x-7z-compressed"},
                {".mp3", "audio/mpeg"},
                {".wav", "audio/wav"},
                {".mp4", "video/mp4"},
                {".avi", "video/x-msvideo"},
                {".mov", "video/quicktime"},
                {".flv", "video/x-flv"},
                {".mkv", "video/x-matroska"},
                {".html", "text/html"},
                {".css", "text/css"},
                {".js", "text/javascript"},
                {".php", "application/x-httpd-php"},
                {".py", "text/x-python"},
                {".java", "text/x-java-source"},
                {".c", "text/x-csrc"},
                {".cpp", "text/x-c++src"},
                {".cs", "text/plain"},
                {".h", "text/plain"},
                {".json", "application/json"},
                {".xml", "application/xml"},
                {".mhtml", "multipart/related"},
                {".apk", "application/vnd.android.package-archive"},
                {".aab", "application/vnd.android.package-archive"},
                {".ipa", "application/octet-stream"},
                {".plist", "application/xml"},
                {".mobileconfig", "application/x-apple-aspen-config"}
            };
        }
    }
}