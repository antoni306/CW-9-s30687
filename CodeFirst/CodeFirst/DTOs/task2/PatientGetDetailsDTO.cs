namespace CodeFirst.DTOs;

public class PatientGetDetailsDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public List<PrescriptionsGetDTO> Medicaments { get; set; }
}