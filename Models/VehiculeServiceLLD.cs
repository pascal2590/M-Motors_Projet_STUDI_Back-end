namespace m_motors_API.Models
{
    public class VehiculeServiceLLD
    {
        public int IdVehicule { get; set; }

        public int IdService { get; set; }

        public Vehicule Vehicule { get; set; }

        public ServiceLLD ServiceLLD { get; set; }
    }
}
