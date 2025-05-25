namespace CodeFirst.DTOs;

public class PrescriptionsGetDTO
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentGetDTO> Medicaments { get; set; }
    public DoctorGetDTO Doctor { get; set; }
}