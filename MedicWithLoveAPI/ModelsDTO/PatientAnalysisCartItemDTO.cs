using MedicWithLoveAPI.Models;

namespace MedicWithLoveAPI.ModelsDTO;

public partial class PatientAnalysisCartItemDTO
{
	public int Id { get; set; }

	public int AnalysisId { get; set; }

	public int PatientAnalysisCartId { get; set; }

	public string? ResultsDescription { get; set; }

	public static implicit operator PatientAnalysisCartItem(PatientAnalysisCartItemDTO dto) => new()
	{
		Id = dto.Id,
		AnalysisId = dto.AnalysisId,
		PatientAnalysisCartId = dto.PatientAnalysisCartId,
		ResultsDescription = dto.ResultsDescription
	};

	public static implicit operator PatientAnalysisCartItemDTO(PatientAnalysisCartItem item) => new()
	{
		Id = item.Id,
		AnalysisId = item.AnalysisId,
		PatientAnalysisCartId = item.PatientAnalysisCartId,
		ResultsDescription = item.ResultsDescription
	};
}