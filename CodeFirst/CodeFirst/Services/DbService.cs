using CodeFirst.Data;
using CodeFirst.DTOs;
using CodeFirst.Exceptions;
using CodeFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;

namespace CodeFirst.Services;

public interface IDbService
{
    public Task<bool> AddPrescription([FromBody] PrescriptionCreateDTO prescriptionCreateDto);
    public Task<PatientGetDetailsDTO> GetPatientDetails([FromRoute]int id);
}
public class DbService(AppDbContext dbContext) : IDbService
{
    public async Task<bool> AddPrescription(PrescriptionCreateDTO prescriptionCreateDto)
    {
        if (prescriptionCreateDto.Date >= prescriptionCreateDto.DueDate)
            throw new Exception("Date cannot be equal or higher than dueDate");
        if (prescriptionCreateDto.Medicaments.Count > 10)
            throw new Exception("Max 10 medicaments per prescription");
        EntityEntry<Patient> patient=null;
        var doctor = prescriptionCreateDto.IdDoctor;
        var medicaments = prescriptionCreateDto.Medicaments;
        if (!dbContext.Doctors.Any(d => d.IdDoctor.Equals(doctor)))
        {
            throw new NotFoundException("no such doctor: " + doctor);
        }
        foreach (var medicament in medicaments)
        {
            if (!dbContext.Medicaments.Any(med => med.IdMedicament.Equals(medicament.IdMedicament)))
            {
                throw new NotFoundException("no such medicament: "+medicament.IdMedicament);
            }
        }

        
        if (!dbContext.Patients.Any(p => p.IdPatient.Equals(prescriptionCreateDto.Patient.IdPatient)))
        {
            patient=await dbContext.Patients.AddAsync(new Patient()
            {
                FirstName = prescriptionCreateDto.Patient.FirstName,
                LastName = prescriptionCreateDto.Patient.LastName,
                BirthDate = prescriptionCreateDto.Patient.BirthDate
            });
            await dbContext.SaveChangesAsync();
            

        }

        var prescription=dbContext.Prescriptions.Add(new Prescription()
        {
            Date = prescriptionCreateDto.Date,
            IdDoctor = doctor,
            IdPatient = patient==null? prescriptionCreateDto.Patient.IdPatient:patient.Entity.IdPatient,
            DueDate = prescriptionCreateDto.DueDate
        });
        await dbContext.SaveChangesAsync();
        foreach (var medicament in prescriptionCreateDto.Medicaments)
        {
            await dbContext.PrescriptionMedicaments.AddAsync(new PrescriptionMedicament()
            {
                IdMedicament = medicament.IdMedicament,
                IdPrescription = prescription.Entity.IdPrescription,
                Details = medicament.Details,
                Dose = medicament.Dose
            });
        }

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<PatientGetDetailsDTO> GetPatientDetails(int id)
    {
        var patient = await dbContext.Patients.FirstOrDefaultAsync(p => p.IdPatient.Equals(id));
        if (patient is null)
            throw new NotFoundException("no such patient in database: " + id);

        var toRet = new PatientGetDetailsDTO()
        {
            IdPatient = id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Medicaments = new List<PrescriptionsGetDTO>()
        };
        var prescriptionsIds = await dbContext.Prescriptions.Where(pres => pres.IdPatient.Equals(id)).Include(pres=>pres.Doctor).ToListAsync();

        foreach (var temp in prescriptionsIds)
        {
            var pres = new PrescriptionsGetDTO()
            {
                Date = temp.Date,
                Doctor = new DoctorGetDTO()
                {
                    FirstName = temp.Doctor.FirstName,
                    IdDoctor = temp.Doctor.IdDoctor
                },
                DueDate = temp.DueDate,
                IdPrescription = temp.IdPrescription,
                Medicaments = await dbContext.PrescriptionMedicaments
                    .Where(pm => pm.IdPrescription.Equals(temp.IdPrescription)).Include(pm => pm.Medicament).Select(
                        join => new MedicamentGetDTO()
                        {
                            Name = join.Medicament.Name,
                            Description = join.Medicament.Description,
                            Dose = join.Dose,
                            IdMedicament = join.IdMedicament,
                            Type = join.Medicament.Type
                        }).ToListAsync()
            };
            toRet.Medicaments.Add(pres);
        }

        return  toRet;
        
        
    }
}