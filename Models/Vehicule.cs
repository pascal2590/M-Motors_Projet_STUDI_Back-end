using m_motors_API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("vehicule")]
public class Vehicule
{
    internal object? DateUpload;

    [Key]
    [Column("id_vehicule")]
    public int IdVehicule { get; set; }

    [Column("marque")]
    [Required]
    public string? Marque { get; set; }

    [Column("modele")]
    [Required]
    public string? Modele { get; set; }

    [Column("annee")]
    public int? Annee { get; set; }

    [Column("kilometrage")]
    public int? Kilometrage { get; set; }

    [Column("prix")]
    public decimal? Prix { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("type_offre")]
    [Required]
    public TypeOffre TypeOffre { get; set; }

    [Column("disponible")]
    public bool Disponible { get; set; } = true;

    [Column("date_ajout")]
    public DateTime DateAjout { get; set; } = DateTime.Now;

    [Column("image_url")]
    public string? ImageUrl { get; set; }

    public ICollection<Dossier> Dossiers { get; set; } = new List<Dossier>();

    public ICollection<VehiculeServiceLLD> VehiculeServices { get; set; } = new List<VehiculeServiceLLD>();
}
