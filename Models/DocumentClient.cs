namespace m_motors_API.Models
{
    public class DocumentClient
    {
        public int IdDocument { get; set; }

        public string NomDocument { get; set; }

        public string CheminFichier { get; set; }

        public DateTime DateUpload { get; set; }

        public int? DossierId { get; set; }

        public Dossier Dossier { get; set; }
    }
}
