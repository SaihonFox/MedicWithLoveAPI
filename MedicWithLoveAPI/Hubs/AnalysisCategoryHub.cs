using MedicWithLoveAPI.Models;
using MedicWithLoveAPI.ModelsContext;

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace MedicWithLoveAPI.Hubs;

public class AnalysisCategoryHub(PgSQLContext context) : Hub<IAnalysisCategoryHub>
{
	public override Task OnConnectedAsync()
	{
		return base.OnConnectedAsync();
	}

	public override Task OnDisconnectedAsync(Exception? exception)
	{
		return base.OnDisconnectedAsync(exception);
	}

	public async Task AnalysisCategoryAdded(AnalysisCategory analysisCategory)
	{
		ArgumentNullException.ThrowIfNull(analysisCategory);

		ArgumentException.ThrowIfNullOrWhiteSpace(analysisCategory.Name);

		var new_analysisCategory = new AnalysisCategory { Name = analysisCategory.Name };

		await context.AnalysisCategories.AddAsync(new_analysisCategory);
		await context.SaveChangesAsync();

		await Clients.All.AnalysisCategoryAdded(analysisCategory);
	}

	public async Task AnalysisCategoryUpdated(AnalysisCategory analysisCategory)
	{
		ArgumentNullException.ThrowIfNull(analysisCategory);

		ArgumentException.ThrowIfNullOrWhiteSpace(analysisCategory.Name);

		var new_analysisCategory = await context.AnalysisCategories.FindAsync(analysisCategory.Id) ?? throw new KeyNotFoundException($"Analysis Category not found.");
		context.AnalysisCategories.Entry(new_analysisCategory).State = EntityState.Detached;
		new_analysisCategory.Name = analysisCategory.Name;

		context.AnalysisCategories.Update(new_analysisCategory);
		await context.SaveChangesAsync();

		await Clients.All.AnalysisCategoryUpdated(new_analysisCategory);
	}

	public async Task AnalysisCategoryDeleted(AnalysisCategory analysisCategory)
	{
		ArgumentNullException.ThrowIfNull(analysisCategory);

		var old_analysisCategory = await context.AnalysisCategories.FindAsync(analysisCategory.Id) ?? throw new KeyNotFoundException($"Analysis Category not found.");

		context.AnalysisCategories.Remove(old_analysisCategory);
		await context.SaveChangesAsync();

		await Clients.All.AnalysisCategoryDeleted(old_analysisCategory);
	}
}