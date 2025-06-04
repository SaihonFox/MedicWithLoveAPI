using MedicWithLoveAPI.Models;

namespace MedicWithLoveAPI.Hubs;

public interface IAnalysisHub
{
	Task AnalysisAdded(Analysis analysis);

	Task AnalysisUpdated(Analysis analysis);

	Task AnalysisDeleted(Analysis analysis);
}