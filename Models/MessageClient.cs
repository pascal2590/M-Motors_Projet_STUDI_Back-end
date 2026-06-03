namespace m_motors_API.Models
{
    public class MessageClient
    {
        public int IdMessage { get; set; }

        public int ClientId { get; set; }

        public string Sujet { get; set; } = string.Empty;

        public string Contenu { get; set; } = string.Empty;

        public bool Lu { get; set; } = false;

        public DateTime DateEnvoi { get; set; } = DateTime.Now;

        public Client Client { get; set; }
    }
}