using MedicWithLoveAPI.Models;

namespace MedicWithLoveAPI.Hubs;

public interface IAnalysisCategoryHub
{
	Task AnalysisCategoryAdded(AnalysisCategory analysisCategory);

	Task AnalysisCategoryUpdated(AnalysisCategory analysisCategory);

	Task AnalysisCategoryDeleted(AnalysisCategory analysisCategory);
}