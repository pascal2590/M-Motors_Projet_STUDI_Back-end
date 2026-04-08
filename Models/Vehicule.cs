using m_motors_API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("vehicule")]
public class Vehicule
{
    [Key]
    [Column("id_vehicule")]  // ← correspond exactement au nom de la colonne MySQL
    public int IdVehicule { get; set; }

    [Column("marque")]
    [Required]
    public string Marque { get; set; }

    [Column("modele")]
    [Required]
    public string Modele { get; set; }

    [Column("annee")]
    public int? Annee { get; set; }

    [Column("kilometrage")]
    public int? Kilometrage { get; set; }

    [Column("prix")]
    public decimal? Prix { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("type_offre")]
    [Required]
    public TypeOffre TypeOffre { get; set; }

    [Column("disponible")]
    public bool Disponible { get; set; } = true;

    [Column("date_ajout")]  // correspond à la colonne MySQL
    public DateTime DateAjout { get; set; } = DateTime.Now;

    public ICollection<Dossier> Dossiers { get; set; } = new List<Dossier>();

    public ICollection<VehiculeServiceLLD> VehiculeServices { get; set; } = new List<VehiculeServiceLLD>();
    public object DateUpload { get; internal set; }
    public object ImageUrl { get; internal set; }
}
