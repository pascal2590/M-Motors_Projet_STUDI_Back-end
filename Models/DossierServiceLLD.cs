namespace m_motors_API.Models
{
    public class DossierServiceLLD
    {
        public int IdDossier { get; set; }

        public int IdService { get; set; }

        public Dossier Dossier { get; set; }

        public ServiceLLD ServiceLLD { get; set; }
    }
}
