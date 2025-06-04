using MedicWithLoveAPI.Models;
using MedicWithLoveAPI.ModelsContext;

using Microsoft.AspNetCore.SignalR;

namespace MedicWithLoveAPI.Hubs;

public class RequestHub(PgSQLContext context) : Hub<IRequestHub>
{
	public override Task OnConnectedAsync()
	{
		return base.OnConnectedAsync();
	}

	public override Task OnDisconnectedAsync(Exception? exception)
	{
		return base.OnDisconnectedAsync(exception);
	}

	public async Task RequestAdded(Request request)
	{
		ArgumentNullException.ThrowIfNull(request);

		await context.Requests.AddAsync(request);
		await context.SaveChangesAsync();

		await Clients.All.RequestAdded(request);
	}

	public async Task RequestUpdated(Request request)
	{
		ArgumentNullException.ThrowIfNull(request);

		var new_request = await context.Requests.FindAsync(request.Id) ?? throw new KeyNotFoundException($"Request not found.");

		context.Requests.Update(new_request);
		await context.SaveChangesAsync();

		await Clients.All.RequestUpdated(new_request);
	}
}