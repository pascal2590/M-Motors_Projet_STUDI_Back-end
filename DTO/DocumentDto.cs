public class DocumentDto
{
    public int IdDocument { get; set; }

    public string NomDocument { get; set; } = "";
    public string TypeDocument { get; set; } = "";
    public string CheminFichier { get; set; } = "";

    public DateTime? DateUpload { get; set; }
}
