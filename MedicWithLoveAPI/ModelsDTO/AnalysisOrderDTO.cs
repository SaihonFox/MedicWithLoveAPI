using System;
using MedicWithLoveAPI.Models;

namespace MedicWithLoveAPI.ModelsDTO;

public partial class AnalysisOrderDTO
{
	public int Id { get; set; }

	public bool AtHome { get; set; }

	public int UserId { get; set; }

	public int PatientId { get; set; }

	public DateTime RegistrationDate { get; set; }

	public int? PatientAnalysisCartId { get; set; }

	public DateTime AnalysisDatetime { get; set; }

	public string? Comment { get; set; }

	public int AnalysisOrderStateId { get; set; }

	public static implicit operator AnalysisOrder(AnalysisOrderDTO dto) => new()
	{
		Id = dto.Id,
		AtHome = dto.AtHome,
		UserId = dto.UserId,
		PatientId = dto.PatientId,
		RegistrationDate = dto.RegistrationDate,
		PatientAnalysisCartId = dto.PatientAnalysisCartId,
		AnalysisDatetime = dto.AnalysisDatetime,
		Comment = dto.Comment,
		AnalysisOrderStateId = dto.AnalysisOrderStateId,
	};

	public static implicit operator AnalysisOrderDTO(AnalysisOrder analysisCategory) => new()
	{
		Id = analysisCategory.Id,
		AtHome = analysisCategory.AtHome,
		UserId = analysisCategory.UserId,
		PatientId = analysisCategory.PatientId,
		RegistrationDate = analysisCategory.RegistrationDate,
		PatientAnalysisCartId = analysisCategory.PatientAnalysisCartId,
		AnalysisDatetime = analysisCategory.AnalysisDatetime,
		Comment = analysisCategory.Comment,
		AnalysisOrderStateId = analysisCategory.AnalysisOrderStateId
	};
}
