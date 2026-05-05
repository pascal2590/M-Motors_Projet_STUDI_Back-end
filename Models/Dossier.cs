using m_motors_API.Enums;
using System;
using System.Collections.Generic;

namespace m_motors_API.Models
{
    public class Dossier
    {
        public int IdDossier { get; set; }

        public TypeDossier TypeDossier { get; set; }

        public StatutDossier Statut { get; set; } = StatutDossier.en_attente;

        public DateTime DateCreation { get; set; } = DateTime.Now;

        public int? ClientId { get; set; }

        public int? VehiculeId { get; set; }

        public Client Client { get; set; }

        public Vehicule Vehicule { get; set; }

        public ICollection<DocumentClient> Documents { get; set; }

        public ICollection<SuiviDossier> Suivis { get; set; }
        
        public ICollection<DossierFinancement> Financements { get; set; }
    }
}
