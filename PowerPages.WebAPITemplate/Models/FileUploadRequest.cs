using System.ComponentModel.DataAnnotations;

namespace PowerPages.WebAPITemplate.API.Models
{
    public class FileUploadRequest
    {       
            [Required]
            public string fileName { get; set; } = string.Empty;
            [Required]
            public string fileContents { get; set; } = string.Empty;
          
    }
}
