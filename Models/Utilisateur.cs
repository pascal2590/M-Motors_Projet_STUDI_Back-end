namespace m_motors_API.Models
{
    public class Utilisateur
    {
        public int IdUser { get; set; }

        public string Nom { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int? RoleId { get; set; }

        public Role Role { get; set; }

        public ICollection<SuiviDossier> Suivis { get; set; }            
    }
}
