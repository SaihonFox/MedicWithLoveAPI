using MedicWithLoveAPI.Models;
using MedicWithLoveAPI.ModelsContext;
using MedicWithLoveAPI.ModelsDTO;

using Microsoft.AspNetCore.Mvc;

namespace MedicWithLoveAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientAnalysisCartItemController(PgSQLContext context) : ControllerBase
{
	[HttpPost]
	public async Task<ActionResult<PatientAnalysisCart>> Post([FromBody] PatientAnalysisCartItemDTO item)
	{
		var patientAnalysisCartEntry = await context.PatientAnalysisCartItems.AddAsync(new PatientAnalysisCartItem
		{
			PatientAnalysisCartId = item.PatientAnalysisCartId,
			AnalysisId = item.AnalysisId,
		});
		await context.SaveChangesAsync();
		return Ok(patientAnalysisCartEntry.Entity);
	}

	[HttpPut]
	public async Task<ActionResult<PatientAnalysisCartItemDTO>> Update([FromBody] PatientAnalysisCartItemDTO item)
	{
		if (item == null)
			return BadRequest("Analysis object cannot be null.");

		var updatedItem = await context.PatientAnalysisCartItems.FindAsync(item.Id);

		updatedItem!.ResultsDescription = item.ResultsDescription;

		var itemEntry = context.PatientAnalysisCartItems.Update(updatedItem);
		await context.SaveChangesAsync();
		return Ok(itemEntry.Entity);
	}
}