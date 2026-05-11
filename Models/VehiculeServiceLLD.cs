using m_motors_API.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class VehiculeServiceLLD
{
    [Column("id_vehicule")]
    public int IdVehicule { get; set; }

    [Column("id_service")]
    public int IdService { get; set; }

    public Vehicule Vehicule { get; set; }
    public ServiceLLD ServiceLLD { get; set; }
}