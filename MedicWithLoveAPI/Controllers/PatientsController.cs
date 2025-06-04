using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MedicWithLoveAPI.Models;
using MedicWithLoveAPI.ModelsDTO;
using MedicWithLoveAPI.ModelsContext;

namespace MedicWithLoveAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController(PgSQLContext context) : ControllerBase
{
	[HttpGet("login")]
	public async Task<ActionResult<Patient>> Login([FromQuery] string login, [FromQuery] string password)
	{
		try
		{
			var patient = (await context.Patients.ToListAsync()).FirstOrDefault(p => p.Login.Equals(login, StringComparison.CurrentCultureIgnoreCase) && p.Password == password);
			if (patient == null)
				return NotFound();

			return Ok(patient);
		}
		catch (Exception ex)
		{
			return NotFound(ex.Message);
		}
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<Patient>>> GetPatients() => Ok(await context.Patients.ToListAsync());

	[HttpGet("{id}")]
	public async Task<ActionResult<Patient>> GetPatient(int id)
	{
		var patient = await context.Patients.FindAsync(id);

		if (patient == null)
			return NotFound();

		return patient;
	}

	// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
	[HttpPost]
	public async Task<ActionResult<Patient>> Register([FromBody] PatientDTO patient)
	{
		ArgumentNullException.ThrowIfNull(patient, nameof(patient));

		patient.Id = 0;

		var patients = await context.Patients.ToListAsync();
		if (patients.Any(p =>
			(p.Email?.Equals(patient.Email, StringComparison.OrdinalIgnoreCase) ?? false) ||
			p.Login.Equals(patient.Login, StringComparison.OrdinalIgnoreCase) ||
			(p.Phone?.Equals(patient.Phone) ?? false) ||
			(p.Passport?.Equals(patient.Passport) ?? false)
		))
			return BadRequest("Пользователь с такими данными уже есть");
		var users = await context.Users.ToListAsync();
		if (users.Any(u =>
			 u.Login.Equals(patient.Login, StringComparison.OrdinalIgnoreCase) ||
			(u.Phone?.Equals(patient.Phone) ?? false) ||
			(u.Passport?.Equals(patient.Passport) ?? false)
		))
			return BadRequest("Пользователь с такими данными уже есть");


		var newPatient = await context.Patients.AddAsync(patient);
		return CreatedAtAction(nameof(GetPatient), new { id = newPatient.Entity.Id }, newPatient.Entity);
	}
}
