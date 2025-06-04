using MedicWithLoveAPI.Models;

namespace MedicWithLoveAPI.ModelsDTO;

public partial class AnalysisDTO
{
	public int Id { get; set; }

	public string Name { get; set; } = null!;

	public string? Description { get; set; }

	public string? Preparation { get; set; }

	public string ResultsAfter { get; set; } = null!;

	public string Biomaterial { get; set; } = null!;

	public decimal Price { get; set; }

	public static implicit operator Analysis(AnalysisDTO dto) => new()
	{
		Id = dto.Id,
		Name = dto.Name,
		Description = dto.Description,
		Preparation = dto.Preparation,
		ResultsAfter = dto.ResultsAfter,
		Biomaterial = dto.Biomaterial,
		Price = dto.Price
	};

	public static implicit operator AnalysisDTO(Analysis analysis) => new()
	{
		Id = analysis.Id,
		Name = analysis.Name,
		Description = analysis.Description,
		Preparation = analysis.Preparation,
		ResultsAfter = analysis.ResultsAfter,
		Biomaterial = analysis.Biomaterial,
		Price = analysis.Price
	};
}