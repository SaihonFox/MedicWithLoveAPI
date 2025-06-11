using MedicWithLoveAPI.Models;
using MedicWithLoveAPI.ModelsContext;
using MedicWithLoveAPI.ModelsDTO;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicWithLoveAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalysisCategoriesController(PgSQLContext context) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<List<AnalysisCategory>>> GetAnalysisCategories() =>
		Ok(await context.AnalysisCategories.Include(x => x.AnalysisCategoriesLists).OrderBy(x => x.Id).ToListAsync());

	[HttpGet("{id}")]
	public async Task<ActionResult<AnalysisCategory>> GetAnalysisCategory(int id)
	{
		try
		{
			var category = await context.AnalysisCategories.FindAsync(id);
			return Ok(category);
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
	}

	[HttpPost]
	public async Task<ActionResult<AnalysisCategory>> PostAnalysisCategory([FromBody] AnalysisCategoryDTO analysisCategory, [FromQuery] bool ignoreId = true)
	{
		ArgumentNullException.ThrowIfNull(analysisCategory, nameof(analysisCategory));

		if(ignoreId)
			analysisCategory.Id = 0;

		var newCategory = await context.AnalysisCategories.AddAsync(analysisCategory);
		return CreatedAtAction(nameof(GetAnalysisCategory), new { id = newCategory.Entity.Id }, newCategory);
	}

	[HttpPut]
	public async Task<ActionResult<AnalysisCategory>> PutAnalysisCategory([FromBody] AnalysisCategoryDTO analysisCategory)
	{
		ArgumentNullException.ThrowIfNull(analysisCategory, nameof(analysisCategory));

		ArgumentException.ThrowIfNullOrWhiteSpace(analysisCategory.Name);

		var updatedCategory = await context.AnalysisCategories.FindAsync(analysisCategory.Id);

		updatedCategory!.Name = analysisCategory.Name;

		var categoryEntry = context.AnalysisCategories.Update(updatedCategory);
		await context.SaveChangesAsync();
		return Ok(categoryEntry.Entity);
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult<AnalysisCategory>> DeleteAnalysisCategory(int id)
	{
		var category = await context.AnalysisCategories.FindAsync(id);

		if(category!.AnalysisCategoriesLists.Count > 0)
		{
			return BadRequest("Нельзя удалить по причине существующей привязке к анализам");
		}

		context.AnalysisCategories.Remove(category!);
		await context.SaveChangesAsync();
		return Ok(category);
	}
}