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
	// GET: api/AnalysisCategories
	[HttpGet]
	public async Task<ActionResult<List<AnalysisCategory>>> GetAnalysisCategories()
	{
		var categories = await context.AnalysisCategories.ToListAsync();
		return Ok(categories);
	}

	// GET: api/AnalysisCategories/5
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

	// POST: api/AnalysisCategories
	// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
	[HttpPost]
	public async Task<ActionResult<AnalysisCategory>> PostAnalysisCategory([FromBody] AnalysisCategoryDTO analysisCategory, [FromQuery] bool ignoreId = true)
	{
		ArgumentNullException.ThrowIfNull(analysisCategory, nameof(analysisCategory));

		if(ignoreId)
			analysisCategory.Id = 0;

		var newCategory = await context.AnalysisCategories.AddAsync(analysisCategory);
		return CreatedAtAction(nameof(GetAnalysisCategory), new { id = newCategory.Entity.Id }, newCategory);
	}

	// PUT: api/AnalysisCategories/5
	// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
	[HttpPut]
	public ActionResult<AnalysisCategory> PutAnalysisCategory([FromBody] AnalysisCategoryDTO analysisCategory)
	{
		ArgumentNullException.ThrowIfNull(analysisCategory, nameof(analysisCategory));

		ArgumentException.ThrowIfNullOrWhiteSpace(analysisCategory.Name);

		var updatedCategory = context.AnalysisCategories.Update(analysisCategory);
		return Ok(updatedCategory.Entity);
	}

	// DELETE: api/AnalysisCategories/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteAnalysisCategory(int id)
	{
		var entity = await context.AnalysisCategories.FindAsync(id);
		context.AnalysisCategories.Remove(entity!);
		return NoContent();
	}
}