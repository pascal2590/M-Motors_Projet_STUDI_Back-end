using Microsoft.AspNetCore.Http;

namespace m_motors_API.DTO
{
    public class UploadDocumentDto
    {
        public int DossierId { get; set; }

        public string TypeDocument { get; set; }

        public IFormFile File { get; set; }
    }
}
