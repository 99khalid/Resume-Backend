using Microsoft.AspNetCore.Http;

namespace Application.DTOs
{
    public class AttachmentDto
    {
        public IFormFile File { get; set; }
    }
}

