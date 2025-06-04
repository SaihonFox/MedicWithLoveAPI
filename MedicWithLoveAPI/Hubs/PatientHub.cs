using MedicWithLoveAPI.Models;
using MedicWithLoveAPI.ModelsContext;

using Microsoft.AspNetCore.SignalR;

namespace MedicWithLoveAPI.Hubs;

public class PatientHub(PgSQLContext context) : Hub<IPatientHub>
{
	public override Task OnConnectedAsync()
	{
		return base.OnConnectedAsync();
	}

	public override Task OnDisconnectedAsync(Exception? exception)
	{
		return base.OnDisconnectedAsync(exception);
	}

	public async Task PatientAdded(Patient patient)
	{
		ArgumentNullException.ThrowIfNull(patient);

		ArgumentException.ThrowIfNullOrWhiteSpace(patient.Name);

		await context.Patients.AddAsync(patient);
		await context.SaveChangesAsync();

		await Clients.All.PatientAdded(patient);
	}
}