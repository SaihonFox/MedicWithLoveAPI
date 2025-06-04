using MedicWithLoveAPI.Models;

namespace MedicWithLoveAPI.ModelsDTO;

public partial class RequestDTO
{
	public int Id { get; set; }

	public int DoctorId { get; set; }

	public int PatientId { get; set; }

	public int? PatientAnalysisCartId { get; set; }

	public DateTime AnalysisDatetime { get; set; }

	public string? Comment { get; set; }

	public int RequestStateId { get; set; }

	public DateTime RequestSended { get; set; }

	public DateTime? RequestChanged { get; set; }

	public static implicit operator Request(RequestDTO dto) => new()
	{
		Id = dto.Id,
		DoctorId = dto.DoctorId,
		PatientId = dto.PatientId,
		PatientAnalysisCartId = dto.PatientAnalysisCartId,
		AnalysisDatetime = dto.AnalysisDatetime,
		Comment = dto.Comment,
		RequestStateId = dto.RequestStateId,
		RequestSended = dto.RequestSended,
		RequestChanged = dto.RequestChanged
	};

	public static implicit operator RequestDTO(Request request) => new()
	{
		Id = request.Id,
		DoctorId = request.DoctorId,
		PatientId = request.PatientId,
		PatientAnalysisCartId = request.PatientAnalysisCartId,
		AnalysisDatetime = request.AnalysisDatetime,
		Comment = request.Comment,
		RequestStateId = request.RequestStateId,
		RequestSended = request.RequestSended,
		RequestChanged = request.RequestChanged
	};
}
