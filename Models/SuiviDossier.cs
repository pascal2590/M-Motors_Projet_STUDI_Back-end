using m_motors_API.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace m_motors_API.Models
{
    public class SuiviDossier
    {
        public int IdSuivi { get; set; }

        public StatutDossier Statut { get; set; }

        public string Commentaire { get; set; }

        [Column("date_modification")]
        public DateTime DateModification { get; set; }

        public int? DossierId { get; set; }

        public int? UserId { get; set; }

        public Dossier Dossier { get; set; }

        public Utilisateur Utilisateur { get; set; }
    }
}
