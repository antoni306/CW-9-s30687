using CodeFirst.DTOs;
using CodeFirst.Exceptions;
using CodeFirst.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirst.Controllers;

[ApiController]
[Route("[controller]")]
public class PharmacyController(IDbService dbService) :ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreatePrescription([FromBody] PrescriptionCreateDTO prescriptionCreateDto)
    {
        try
        {
            var res = await dbService.AddPrescription(prescriptionCreateDto);
            return Ok(res);
        }
        catch (NotFoundException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> getPatientDetails([FromRoute] int id)
    {
        try
        {
            var res = await dbService.GetPatientDetails(id);
            return Ok(res);
        }
        catch (NotFoundException e)
        {
            return BadRequest(e.Message);
        }
    }
    
}