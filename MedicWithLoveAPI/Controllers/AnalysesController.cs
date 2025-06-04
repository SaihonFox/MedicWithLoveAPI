using Microsoft.AspNetCore.Mvc;

using MedicWithLoveAPI.Models;
using MedicWithLoveAPI.ModelsDTO;
using MedicWithLoveAPI.ModelsContext;
using Microsoft.EntityFrameworkCore;

namespace MedicWithLoveAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalysesController(PgSQLContext context) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Analysis>>> GetAnalyses() => Ok(await context.Analyses.ToListAsync());

	[HttpGet("{id:int}")]
	public async Task<ActionResult<Analysis>> GetAnalysis(int id)
	{
		try
		{
			var analysis = await context.Analyses.FindAsync(id);
			return Ok(analysis);
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
	}

	[HttpPost]
	public async Task<ActionResult<Analysis>> PostAnalysis([FromBody] AnalysisDTO analysis, [FromQuery] bool ignoreId = true)
	{
		if (analysis == null)
			return BadRequest("Analysis object cannot be null.");

		if (ignoreId)
			analysis.Id = 0;

		var newAnalysis = await context.Analyses.AddAsync(analysis);
		return CreatedAtAction(nameof(GetAnalysis), new { id = newAnalysis.Entity.Id }, newAnalysis.Entity);
	}

	[HttpPut]
	public ActionResult<Analysis> PutAnalysis([FromBody] AnalysisDTO analysis)
	{
		if (analysis == null)
			return BadRequest("Analysis object cannot be null.");

		var updatedAnalysis = context.Analyses.Update(analysis);
		return Ok(updatedAnalysis.Entity);
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> DeleteAnalysis(int id)
	{
		var entity = await context.Analyses.FindAsync(id);
		context.Analyses.Remove(entity!);
		return NoContent();
	}
}