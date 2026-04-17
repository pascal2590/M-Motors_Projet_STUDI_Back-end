using System.Text.Json.Serialization;


namespace m_motors_API.Models
{
    public class DossierFinancement
    {
        public int Id { get; set; }

        public int DossierId { get; set; }

        public decimal? Apport { get; set; }

        public string Financement { get; set; }

        public int? Duree { get; set; }

        public int? Kilometrage { get; set; }

        public decimal? Mensualite { get; set; }

        [JsonIgnore]
        public Dossier Dossier { get; set; }
    }
}
