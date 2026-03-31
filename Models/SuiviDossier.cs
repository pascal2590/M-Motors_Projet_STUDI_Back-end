using m_motors_API.Enums;
using System;

namespace m_motors_API.Models
{
    public class SuiviDossier
    {
        public int IdSuivi { get; set; }

        public StatutDossier Statut { get; set; }

        public string Commentaire { get; set; }

        public DateTime DateModification { get; set; }

        public int? DossierId { get; set; }

        public int? UserId { get; set; }

        public Dossier Dossier { get; set; }

        public Utilisateur Utilisateur { get; set; }
    }
}
