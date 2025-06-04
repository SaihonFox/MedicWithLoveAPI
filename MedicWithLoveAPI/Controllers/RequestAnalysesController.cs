using Microsoft.AspNetCore.Mvc;

using MedicWithLoveAPI.Models;
using MedicWithLoveAPI.ModelsDTO;
using MedicWithLoveAPI.ModelsContext;
using Microsoft.EntityFrameworkCore;

namespace MedicWithLoveAPI.Controllers;

/*[ApiController]
[Route("api/[controller]")]
public class RequestAnalysesController(PgSQLContext context) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Request>>> GetRequestAnalyses() => Ok(await context.RequestAnalyses.ToListAsync());

	[HttpGet("{id}")]
	public async Task<ActionResult<Request>> GetRequestAnalysis(int id)
	{
		try
		{
			var request = await context.RequestAnalyses.FindAsync(id);
			return Ok(request);
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
	}

	[HttpPost]
	public async Task<ActionResult<Request>> PostRequestAnalysis([FromBody] RequestAnalysisDTO requestAnalysis, [FromQuery] bool ignoreId = true)
	{
		if (requestAnalysis == null)
			return BadRequest("RequestAnalysis object cannot be null.");

		context.SaveChangesFailed += (_, e) =>
		{
			Console.WriteLine($"\n\n\n{requestAnalysis.RequestId}\n\n\n");
		};

		var entry = await context.RequestAnalyses.AddAsync(new RequestAnalysis { AnalysisId = requestAnalysis.AnalysisId, RequestId = requestAnalysis.RequestId });
		await context.SaveChangesAsync();
		
		return CreatedAtAction(nameof(GetRequestAnalysis), new { id = entry.Entity.Id }, entry.Entity);
	}

	[HttpPut]
	public async Task<IActionResult> PutRequestAnalysis([FromBody] RequestAnalysisDTO request)
	{
		if (request == null)
			return BadRequest("RequestAnalysis object cannot be null.");

		try
		{
			context.RequestAnalyses.Update(request);
			await context.SaveChangesAsync();
			var updatedRequest = await context.RequestAnalyses.FindAsync(request.Id);
			return Ok(updatedRequest);
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
	}
}*/