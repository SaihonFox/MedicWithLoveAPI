using MedicWithLoveAPI.Models;
using MedicWithLoveAPI.ModelsContext;

using Microsoft.AspNetCore.SignalR;

namespace MedicWithLoveAPI.Hubs;

public class UserHub(PgSQLContext context) : Hub<IUserHub>
{
	public async Task UserUpdated(User user)
	{
		ArgumentNullException.ThrowIfNull(user);

		var new_user = await context.Users.FindAsync(user.Id) ?? throw new KeyNotFoundException($"User not found.");

		context.Users.Update(new_user);
		await context.SaveChangesAsync();

		await Clients.All.UserUpdated(new_user);
	}
}