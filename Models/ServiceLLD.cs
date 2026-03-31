using System.Collections.Generic;

namespace m_motors_API.Models
{
    public class ServiceLLD
    {
        public int IdService { get; set; }

        public string NomService { get; set; }

        public string Description { get; set; }

        public ICollection<VehiculeServiceLLD> VehiculeServices { get; set; }
    }
}
