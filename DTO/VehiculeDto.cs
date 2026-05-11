using m_motors_API.Enums;

namespace m_motors_API.DTOs
{
    public class VehiculeDto
    {
        public int IdVehicule { get; set; }

        public string Marque { get; set; }

        public string Modele { get; set; }

        public int? Annee { get; set; }

        public int? Kilometrage { get; set; }

        public decimal? Prix { get; set; }

        public string? Description { get; set; }

        public TypeOffre TypeOffre { get; set; }

        public bool Disponible { get; set; }

        public string? ImageUrl { get; set; }

        // SERVICES LLD
        public List<int>? ServicesLld { get; set; }
    }
}