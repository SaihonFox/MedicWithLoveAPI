using MedicWithLoveAPI.Models;

using Microsoft.AspNetCore.SignalR;

namespace MedicWithLoveAPI.Hubs;

public class AnalysisHub : Hub<IAnalysisHub>
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
		await Clients.All.AnalysisAdded(analysis);
	}

	public async Task AnalysisUpdated(Analysis analysis)
	{
		await Clients.All.AnalysisUpdated(analysis);
	}

	public async Task AnalysisDeleted(Analysis analysis)
	{
		await Clients.All.AnalysisDeleted(analysis);
	}
}