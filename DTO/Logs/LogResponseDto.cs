namespace m_motors_API.DTO.Logs
{
    public class LogResponseDto
    {
        public int IdLog { get; set; }
        public string Niveau { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Endpoint { get; set; }
        public string? Utilisateur { get; set; }
        public DateTime DateLog { get; set; }
    }
}