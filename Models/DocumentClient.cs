using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace m_motors_API.Models
{
    [Table("document")]
    public class DocumentClient
    {
        [Key]
        [Column("id_document")]
        public int IdDocument { get; set; }

        [Column("nom_document")]
        public string NomDocument { get; set; }

        // CHAMP IMPORTANT POUR US10
        [Column("type_document")]
        public string TypeDocument { get; set; }

        [Column("chemin_fichier")]
        public string CheminFichier { get; set; }

        [Column("date_upload")]
        public DateTime DateUpload { get; set; }

        [Column("dossier_id")]
        public int? DossierId { get; set; }

        public Dossier Dossier { get; set; }
    }
}
