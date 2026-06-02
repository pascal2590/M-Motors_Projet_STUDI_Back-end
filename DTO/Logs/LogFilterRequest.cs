namespace m_motors_API.DTO.Logs
{
    public class LogFilterRequest
    {
        public string? Niveau { get; set; }
        public string? Recherche { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;    }
}