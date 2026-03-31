using System;
using System.Collections.Generic;

namespace m_motors_API.Models
{
    public class Client
    {
        public int IdClient { get; set; }

        public string Nom { get; set; }

        public string Prenom { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Telephone { get; set; }

        public string Adresse { get; set; }

        public DateTime DateUpload { get; set; } = DateTime.Now;

        public ICollection<Dossier> Dossiers { get; set; }
    }
}
