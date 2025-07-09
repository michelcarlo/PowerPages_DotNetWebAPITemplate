namespace PowerPages.WebAPITemplate.API.Models
{
    public class FileUploadResponse
    {

        public string documentUrl { get; set; } = string.Empty;
        public int itemId { get; set; } = -1;
        public string error { get; set; } = string.Empty;

    }
}
