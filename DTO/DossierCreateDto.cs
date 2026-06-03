using m_motors_API.Enums;

namespace m_motors_API.DTO
{
    public class DossierCreateDto
    {
        public int VehiculeId { get; set; }

        public int ClientId { get; set; }

        public TypeDossier TypeDossier { get; set; } // "Achat" ou "LLD"

        // Achat
        public decimal? Apport { get; set; }
        public string? Financement { get; set; }

        // LLD
        public int? Duree { get; set; }
        public int? Kilometrage { get; set; }
        public decimal? Mensualite { get; set; }
    }
}
