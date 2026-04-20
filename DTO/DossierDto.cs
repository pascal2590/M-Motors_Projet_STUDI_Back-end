public class DossierDto
{
    public int IdDossier { get; set; }
    public string TypeDossier { get; set; }
    public string Statut { get; set; }
    public DateTime DateCreation { get; set; }

    public VehiculeDto Vehicule { get; set; }
    public List<DocumentDto> Documents { get; set; }
}
