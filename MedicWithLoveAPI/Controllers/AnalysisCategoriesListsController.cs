using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicWithLoveAPI.Models;
using MedicWithLoveAPI.ModelsDTO;
using MedicWithLoveAPI.ModelsContext;

namespace MedicWithLoveAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalysisCategoriesListsController(PgSQLContext context) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IEnumerable<AnalysisCategoriesList>>> GetAnalysisCategoriesLists()
	{
		return await context.AnalysisCategoriesLists.OrderBy(x => x.Id).ToListAsync();
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<AnalysisCategoriesList>> GetAnalysisList(int id)
	{
		var analysisCategory = await context.AnalysisCategoriesLists.FindAsync(id);

		if (analysisCategory == null)
		{
			return NotFound();
		}

		return analysisCategory;
	}

	[HttpPost]
	public async Task<IActionResult> UpdateCategories4Analysis([FromQuery] int analysisId, [FromBody] int[] categoryIds)
	{
		var categoriesInAnalysis = await context.AnalysisCategoriesLists.ToListAsync();
		categoriesInAnalysis = categoriesInAnalysis.Where(x => x.Analysis.Id == analysisId).ToList();
		context.AnalysisCategoriesLists.RemoveRange(categoriesInAnalysis);
		await context.SaveChangesAsync();

		foreach (var categoryId in categoryIds)
		{
			var category = await context.AnalysisCategories.FindAsync(categoryId);
			await context.AnalysisCategoriesLists.AddAsync(new AnalysisCategoriesList { AnalysisId = analysisId, AnalysisCategoryId = categoryId });
			await context.SaveChangesAsync();
		}

		return Ok();
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> PutAnalysisCategoriesList(int id, [FromBody] AnalysisCategoriesListDTO analysisCategoriesList)
	{
		if (id != analysisCategoriesList.Id)
			return BadRequest();

		context.Entry(analysisCategoriesList).State = EntityState.Modified;

		try
		{
			await context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!AnalysisCategoryListExists(id))
			{
				return NotFound();
			}
			else
			{
				throw;
			}
		}

		return NoContent();
	}

	// DELETE: api/AnalysisCategories/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteAnalysisCategoryList(int id)
	{
		var analysisCategoriesList = await context.AnalysisCategoriesLists.FindAsync(id);
		if (analysisCategoriesList == null)
		{
			return NotFound();
		}

		context.AnalysisCategoriesLists.Remove(analysisCategoriesList);
		await context.SaveChangesAsync();

		return NoContent();
	}

	private bool AnalysisCategoryListExists(int id)
	{
		return context.AnalysisCategoriesLists.Any(e => e.Id == id);
	}
}