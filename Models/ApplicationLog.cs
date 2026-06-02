namespace m_motors_API.Models
{
    public class ApplicationLog
    {
        public int IdLog { get; set; }
        public string Niveau { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Exception { get; set; }
        public string? StackTrace { get; set; }
        public string? Endpoint { get; set; }
        public string? MethodeHttp { get; set; }
        public string? Utilisateur { get; set; }
        public DateTime DateLog { get; set; } = DateTime.UtcNow;
    }
}