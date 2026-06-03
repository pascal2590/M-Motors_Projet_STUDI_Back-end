using m_motors_API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("vehicule")]
public class Vehicule
{
    [Key]
    [Column("id_vehicule")]
    public int IdVehicule { get; set; }

    [Required]
    [Column("marque")]
    public string? Marque { get; set; }

    [Required]
    [Column("modele")]
    public string? Modele { get; set; }

    [Column("annee")]
    public int? Annee { get; set; }

    [Column("kilometrage")]
    public int? Kilometrage { get; set; }

    [Column("prix")]
    public decimal? Prix { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Required]
    [Column("type_offre")]
    public TypeOffre TypeOffre { get; set; }

    [Column("disponible")]
    public bool Disponible { get; set; } = true;

    [Column("date_ajout")]
    public DateTime DateAjout { get; set; } = DateTime.Now;

    [Column("image_url")]
    public string? ImageUrl { get; set; }

    // relations
    public ICollection<Dossier> Dossiers { get; set; } = new List<Dossier>();

    public ICollection<VehiculeServiceLLD> VehiculeServices { get; set; } = new List<VehiculeServiceLLD>();
}