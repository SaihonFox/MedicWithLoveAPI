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
	public async Task<ActionResult<List<AnalysisOrder>>> Get() => Ok(await context.AnalysisOrders.OrderBy(x => x.Id).ToListAsync());
	
	[HttpGet("user")]
	public ActionResult<List<AnalysisOrder>> Get4User([FromQuery] int id) => Ok(context.AnalysisOrders
		.Include(x => x.Patient)
		.Include(x => x.User)
		.Include(x => x.PatientAnalysisCart)
			.ThenInclude(x => x.PatientAnalysisCartItems)
				.ThenInclude(x => x.Analysis)
		.ToList().Where(x => x.UserId == id));

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

	[HttpPut]
	public async Task<ActionResult<AnalysisOrder>> Update([FromBody] AnalysisOrderDTO analysisOrder)
	{
		if (analysisOrder == null)
			return BadRequest("Analysis object cannot be null.");

		var updatedAnalysisOrder = await context.AnalysisOrders.FindAsync(analysisOrder.Id);

		updatedAnalysisOrder!.AnalysisOrderStateId = analysisOrder.AnalysisOrderStateId;

		var analysisOrderEntry = context.AnalysisOrders.Update(updatedAnalysisOrder);
		await context.SaveChangesAsync();
		return Ok(analysisOrderEntry.Entity);
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult<AnalysisOrder>> Delete(int id)
	{
		var entity = await context.AnalysisOrders.FindAsync(id);
		context.AnalysisOrders.Remove(entity!);
		return Ok(await context.SaveChangesAsync());
	}
}