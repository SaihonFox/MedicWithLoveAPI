using MedicWithLoveAPI.Models;
using MedicWithLoveAPI.ModelsContext;

using Microsoft.AspNetCore.SignalR;

namespace MedicWithLoveAPI.Hubs;

public class AnalysisHub(PgSQLContext context) : Hub<IAnalysisHub>
{
	public override Task OnConnectedAsync()
	{
		return base.OnConnectedAsync();
	}

	public override Task OnDisconnectedAsync(Exception? exception)
	{
		return base.OnDisconnectedAsync(exception);
	}

	public async Task AnalysisAdded(Analysis analysis)
	{
		ArgumentNullException.ThrowIfNull(analysis);

		ArgumentException.ThrowIfNullOrWhiteSpace(analysis.Name);
		ArgumentException.ThrowIfNullOrWhiteSpace(analysis.Biomaterial);
		ArgumentException.ThrowIfNullOrWhiteSpace(analysis.ResultsAfter);

		await context.Analyses.AddAsync(analysis);
		await context.SaveChangesAsync();

		await Clients.All.AnalysisAdded(analysis);
	}

	public async Task AnalysisUpdated(Analysis analysis)
	{
		ArgumentNullException.ThrowIfNull(analysis);

		ArgumentException.ThrowIfNullOrWhiteSpace(analysis.Name);
		ArgumentException.ThrowIfNullOrWhiteSpace(analysis.Biomaterial);
		ArgumentException.ThrowIfNullOrWhiteSpace(analysis.ResultsAfter);

		var new_analysis = await context.Analyses.FindAsync(analysis.Id) ?? throw new KeyNotFoundException($"Analysis not found.");

		context.Analyses.Update(new_analysis);
		await context.SaveChangesAsync();

		await Clients.All.AnalysisUpdated(new_analysis);
	}

	public async Task AnalysisDeleted(Analysis analysis)
	{
		ArgumentNullException.ThrowIfNull(analysis);

		var old_analysis = await context.Analyses.FindAsync(analysis.Id) ?? throw new KeyNotFoundException($"Analysis not found.");

		context.Analyses.Remove(old_analysis);
		await context.SaveChangesAsync();

		await Clients.All.AnalysisDeleted(old_analysis);
	}
}