using Microsoft.AspNetCore.Mvc;

using MedicWithLoveAPI.Models;
using MedicWithLoveAPI.ModelsDTO;
using MedicWithLoveAPI.ModelsContext;
using Microsoft.EntityFrameworkCore;
using MedicWithLoveAPI.Hubs;

namespace MedicWithLoveAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalysesController(PgSQLContext context) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Analysis>>> GetAnalyses() => Ok(await context.Analyses.Include(x => x.AnalysisCategoriesLists).Include(x => x.PatientAnalysisCartItems).OrderBy(x => x.Id).ToListAsync());

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
		await context.SaveChangesAsync();

		return Ok(await context.Analyses.Include(x => x.AnalysisCategoriesLists).ThenInclude(x => x.AnalysisCategory).FirstAsync(x => x.Id == newAnalysis.Entity.Id));
	}

	[HttpPut]
	public async Task<ActionResult<Analysis>> PutAnalysis([FromBody] AnalysisDTO analysis)
	{
		if (analysis == null)
			return BadRequest("Analysis object cannot be null.");

		var updatedAnalysis = await context.Analyses.FindAsync(analysis.Id);

		updatedAnalysis!.Name = analysis.Name;
		updatedAnalysis!.Preparation = analysis.Preparation;
		updatedAnalysis!.Price = analysis.Price;
		updatedAnalysis!.Description = analysis.Description;
		updatedAnalysis!.Biomaterial = analysis.Biomaterial;
		updatedAnalysis!.ResultsAfter = analysis.ResultsAfter;

		var analysisEntry = context.Analyses.Update(updatedAnalysis);
		await context.SaveChangesAsync();

		return Ok(analysisEntry.Entity);
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> DeleteAnalysis(int id)
	{
		var entity = await context.Analyses.FindAsync(id);
		context.Analyses.Remove(entity!);
		await context.SaveChangesAsync();

		return Ok(entity);
	}
}