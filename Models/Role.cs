using System.Collections.Generic;

namespace m_motors_API.Models
{
    public class Role
    {
        public int IdRole { get; set; }

        public string NomRole { get; set; }

        public ICollection<Utilisateur> Utilisateurs { get; set; }
            = new List<Utilisateur>();

    }
}
