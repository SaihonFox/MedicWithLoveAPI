using MedicWithLoveAPI.Models;
using MedicWithLoveAPI.ModelsContext;
using MedicWithLoveAPI.ModelsDTO;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicWithLoveAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestsController(PgSQLContext context) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Request>>> GetRequests() => Ok(await context.Requests.OrderBy(x => x.Id).ToListAsync());

	[HttpGet("{id}")]
	public async Task<ActionResult<Request>> GetRequest(int id)
	{
		var request = await context.Requests
			.Include(x => x.Patient)
			.Include(x => x.Doctor)
			.Include(x => x.PatientAnalysisCart)
			.Include(x => x.RequestState)
			.FirstAsync(x => x.Id == id);
		return Ok(request);
	}

	[HttpPost]
	public async Task<ActionResult<Request>> PostRequest([FromBody] RequestDTO request, [FromQuery] bool ignoreId = true)
	{
		var newAnalysis = await context.Requests.AddAsync(new Request
		{
			AnalysisDatetime = request.AnalysisDatetime,
			DoctorId = request.DoctorId,
			PatientId = request.PatientId,
			Comment = request.Comment,
			RequestSended = request.RequestSended,
			RequestStateId = request.RequestStateId
		});
		await context.SaveChangesAsync();
		return CreatedAtAction(nameof(GetRequest), new { id = newAnalysis.Entity.Id }, newAnalysis.Entity);
	}

	[HttpPut]
	public async Task<ActionResult<Request>> PutRequest([FromBody] RequestDTO request)
	{
		var updatedRequest = context.Requests.Update(request);
		await context.SaveChangesAsync();
		return Ok(updatedRequest.Entity);
	}
}