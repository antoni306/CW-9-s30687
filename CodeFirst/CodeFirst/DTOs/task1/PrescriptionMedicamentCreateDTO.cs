using System.ComponentModel.DataAnnotations;

namespace CodeFirst.DTOs;

public class PrescriptionMedicamentCreateDTO
{
    public int IdMedicament { get; set; }
    public int? Dose { get; set; }
    [MaxLength(100)] 
    public string Details { get; set; } = null!;
}