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
	private readonly PgSQLContext _context = context;

	// GET: api/AnalysisCategories
	[HttpGet]
	public async Task<ActionResult<IEnumerable<AnalysisCategoriesList>>> GetAnalysisCategoriesLists()
	{
		return await _context.AnalysisCategoriesLists.ToListAsync();
	}

	// GET: api/AnalysisCategories/5
	[HttpGet("{id}")]
	public async Task<ActionResult<AnalysisCategoriesList>> GetAnalysisList(int id)
	{
		var analysisCategory = await _context.AnalysisCategoriesLists.FindAsync(id);

		if (analysisCategory == null)
		{
			return NotFound();
		}

		return analysisCategory;
	}

	// PUT: api/AnalysisCategories/5
	// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
	[HttpPut("{id}")]
	public async Task<IActionResult> PutAnalysisCategoriesList(int id, [FromBody] AnalysisCategoriesListDTO analysisCategoriesList)
	{
		if (id != analysisCategoriesList.Id)
			return BadRequest();

		_context.Entry(analysisCategoriesList).State = EntityState.Modified;

		try
		{
			await _context.SaveChangesAsync();
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

	// POST: api/AnalysisCategories
	// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
	[HttpPost]
	public async Task<ActionResult<AnalysisCategoriesList>> PostAnalysisCategory([FromBody] AnalysisCategoriesListDTO analysisCategoriesList)
	{
		_context.AnalysisCategoriesLists.Add(analysisCategoriesList);
		await _context.SaveChangesAsync();

		return CreatedAtAction("GetAnalysisCategory", new { id = analysisCategoriesList.Id }, analysisCategoriesList);
	}

	// DELETE: api/AnalysisCategories/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteAnalysisCategoryList(int id)
	{
		var analysisCategoriesList = await _context.AnalysisCategoriesLists.FindAsync(id);
		if (analysisCategoriesList == null)
		{
			return NotFound();
		}

		_context.AnalysisCategoriesLists.Remove(analysisCategoriesList);
		await _context.SaveChangesAsync();

		return NoContent();
	}

	private bool AnalysisCategoryListExists(int id)
	{
		return _context.AnalysisCategoriesLists.Any(e => e.Id == id);
	}
}