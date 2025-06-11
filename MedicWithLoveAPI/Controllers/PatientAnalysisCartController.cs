using MedicWithLoveAPI.Models;
using MedicWithLoveAPI.ModelsContext;
using MedicWithLoveAPI.ModelsDTO;

using Microsoft.AspNetCore.Mvc;

namespace MedicWithLoveAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientAnalysisCartController(PgSQLContext context) : ControllerBase
{
	[HttpPost]
	public async Task<ActionResult<PatientAnalysisCart>> Post([FromBody] PatientAnalysisCartDTO patientAnalysisCart)
	{
		var patientAnalysisCartEntry = await context.PatientAnalysisCarts.AddAsync(new PatientAnalysisCart { PatientId = patientAnalysisCart.PatientId });
		await context.SaveChangesAsync();
		return Ok(context.PatientAnalysisCarts.Find(patientAnalysisCartEntry.Entity.Id));
	}
}