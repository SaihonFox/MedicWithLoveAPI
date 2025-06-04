using MedicWithLoveAPI.Models;
using MedicWithLoveAPI.ModelsContext;
using MedicWithLoveAPI.ModelsDTO;
using MedicWithLoveAPI.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicWithLoveAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalysisOrdersController(PgSQLContext context, EmailService emailService) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<List<AnalysisOrder>>> Get() => Ok(await context.AnalysisOrders.ToListAsync());
	
	[HttpGet("user")]
	public ActionResult<List<AnalysisOrder>> Get4User([FromQuery] int id) => Ok(context.AnalysisOrders.ToList().Where(x => x.UserId == id));

	[HttpGet("{id}")]
	public async Task<ActionResult<AnalysisOrder>> Get(int id) => await context.AnalysisOrders.FindAsync(id) is { } order ? Ok(order) : NotFound($"AnalysisOrder with ID {id} not found");

	[HttpPost]
	public async Task<ActionResult<User>> Post([FromBody] AnalysisOrderDTO analysisOrder)
	{
		var analysisOrderEntry = await context.AnalysisOrders.AddAsync(analysisOrder);
		await context.SaveChangesAsync();

		var order = await context.AnalysisOrders
			.Include(x => x.User)
			.Include(x => x.Patient)
			.Include(x => x.PatientAnalysisCart)
				.ThenInclude(x => x.PatientAnalysisCartItems)
					.ThenInclude(x => x.Analysis)
			.FirstAsync(x => x.Id == analysisOrderEntry.Entity.Id);
		var patient = await context.Patients.FindAsync(analysisOrder.PatientId);


		if (!string.IsNullOrWhiteSpace(patient!.Email))
			emailService.SendEmailPatientOrder("tirilnar@gmail.com", order);

		return Ok(await context.Users.FindAsync(analysisOrder.UserId));
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult<AnalysisOrder>> Delete(int id)
	{
		var entity = await context.AnalysisOrders.FindAsync(id);
		context.AnalysisOrders.Remove(entity!);
		return Ok(await context.SaveChangesAsync());
	}
}