using System.ComponentModel.DataAnnotations;

namespace CodeFirst.DTOs;

public class PrescriptionCreateDTO
{
    [Required]
    public PatientCreateDTO Patient { get; set; }
    [Required]
    public List<PrescriptionMedicamentCreateDTO> Medicaments { get; set; }
    [Required]
    public int IdDoctor { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public DateTime DueDate { get; set; }
}