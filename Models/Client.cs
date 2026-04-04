using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace m_motors_API.Models
{
    [Table("client")]
    public class Client
    {
        [Key]
        [Column("id_client")]
        public int IdClient { get; set; }

        [Column("nom")]
        public string Nom { get; set; }

        [Column("prenom")]
        public string Prenom { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("telephone")]
        public string Telephone { get; set; }

        [Column("adresse")]
        public string Adresse { get; set; }

        [Column("DateUpload")]
        public DateTime DateUpload { get; set; } = DateTime.Now;

        // Relation optionnelle
        public ICollection<Dossier>? Dossiers { get; set; }
    }
}
