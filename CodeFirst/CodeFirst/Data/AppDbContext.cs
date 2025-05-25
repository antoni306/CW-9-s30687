using CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Data;

public class AppDbContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    public AppDbContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        var patients = new List<Patient>
        {
            new Patient()
            {
                IdPatient = 1,
                BirthDate = new DateTime(2024, 1, 1),
                FirstName = "Antek",
                LastName = "Kostuj"
            },
            new Patient()
            {
                IdPatient = 2,
                BirthDate = new DateTime(2024, 1, 1),
                FirstName = "Ola",
                LastName = "Kostuj"
            },
            new Patient()
            {
                IdPatient = 3,
                BirthDate = new DateTime(2024, 1, 1),
                FirstName = "Zuzia",
                LastName = "Kostuj"
            },
            new Patient()
            {
                IdPatient = 4,
                BirthDate = new DateTime(2024, 1, 1),
                FirstName = "Tomasz",
                LastName = "Kostuj"
            },
            new Patient()
            {
                IdPatient = 5,
                BirthDate = new DateTime(2024, 1, 1),
                FirstName = "Sylwia",
                LastName = "Kostuj"
            }
        };
        var docs = new List<Doctor>
        {
            new Doctor()
            {
                IdDoctor = 1,
                Email = "email",
                FirstName = "Franek",
                LastName = "doktorski"
            },
            new Doctor()
            {
                IdDoctor = 2,
                Email = "email2",
                FirstName = "doktor",
                LastName = "doktorski"
            }
        };
        var meds = new List<Medicament>
        {
            new Medicament()
            {
                Description = "desc",
                Name = "lek 1",
                IdMedicament = 1,
                Type = "typ1"
                
            },
            new Medicament()
            {
                Description = "desc",
                Name = "lek 2",
                IdMedicament = 2,
                Type = "typ1"
            },
            new Medicament()
            {
                Description = "desc 3",
                Name = "lek 3",
                IdMedicament = 3,
                Type = "typ1"
            },
        };
        var prescriptions = new List<Prescription>
        {
            new Prescription()
            {
                IdPrescription = 1,
                Date = new DateTime(2024, 1, 1),
                IdDoctor = 1,
                DueDate = new DateTime(2026, 1, 1),
                IdPatient = 1
            },
            new Prescription()
            {
                IdPrescription = 2,
                Date = new DateTime(2024, 1, 1),
                IdDoctor = 2,
                DueDate = new DateTime(2025, 1, 1),
                IdPatient = 2
            }
        };
        var presmeds = new List<PrescriptionMedicament>
        {
            new PrescriptionMedicament()
            {
                Details = "no details",
                Dose = 10,
                IdMedicament = 1,
                IdPrescription = 1
            },
            new PrescriptionMedicament()
            {
                Details = "no details",
                Dose = 10,
                IdMedicament = 2,
                IdPrescription = 1
            }
        };
        modelBuilder.Entity<Doctor>().HasData(docs);
        modelBuilder.Entity<Patient>().HasData(patients);
        modelBuilder.Entity<Medicament>().HasData(meds);
        modelBuilder.Entity<Prescription>().HasData(prescriptions);
        modelBuilder.Entity<PrescriptionMedicament>()
            .HasKey(pm => new { pm.IdMedicament, pm.IdPrescription });
        modelBuilder.Entity<PrescriptionMedicament>().HasData(presmeds);
    }
}