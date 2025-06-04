using System;
using System.Collections.Generic;

using MedicWithLoveAPI.Models;

namespace MedicWithLoveAPI.ModelsDTO;

public partial class AnalysisCategoryDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

	public static implicit operator AnalysisCategory(AnalysisCategoryDTO dto) => new()
	{
		Id = dto.Id,
		Name = dto.Name
	};

	public static implicit operator AnalysisCategoryDTO(AnalysisCategory analysisCategory) => new()
	{
		Id = analysisCategory.Id,
		Name = analysisCategory.Name
	};
}
