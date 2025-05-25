using System.ComponentModel.DataAnnotations;
using CodeFirst.Models;

namespace CodeFirst.DTOs;

public class PatientCreateDTO
{
    [Required]
    public int IdPatient { get; set; }
    [MaxLength(100)] 
    public string FirstName { get; set; } = null!;
    [MaxLength(100)]
    public string LastName { get; set; } = null!;
    public DateTime BirthDate { get; set; }

}