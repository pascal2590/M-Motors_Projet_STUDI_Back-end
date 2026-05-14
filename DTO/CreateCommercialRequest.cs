namespace m_motors_API.DTO
{
    public class CreateCommercialRequest
    {
        public string Nom { get; set; }
        public string Prenom { get; set; } // Ajouté le 14/05/2026
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
