namespace m_motors_API.DTO
{
    public class DossierAchatDto
    {
        public int ClientId { get; set; }

        public int VehiculeId { get; set; }

        public decimal? Apport { get; set; }

        public string Financement { get; set; }

        public int? Duree { get; set; }

        public int? Kilometrage { get; set; }

        public decimal? Mensualite { get; set; }
    }
}
