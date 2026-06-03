using System.ComponentModel.DataAnnotations;

namespace m_motors_API.DTO
{
    public class RegisterRequest
    {
        [Required]
        public string Nom { get; set; }
        [Required]
        public string Prenom { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Telephone { get; set; }
        public string Adresse { get; set; }
    }
}
