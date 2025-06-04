using System;
using System.Collections.Generic;

using MedicWithLoveAPI.Models;

namespace MedicWithLoveAPI.ModelsDTO;

public partial class AnalysisCategoriesListDTO
{
    public int Id { get; set; }

    public int AnalysisId { get; set; }

    public int AnalysisCategoryId { get; set; }

	public static implicit operator AnalysisCategoriesList(AnalysisCategoriesListDTO dto) => new()
	{
		Id = dto.Id,
		AnalysisId = dto.AnalysisId,
		AnalysisCategoryId = dto.AnalysisCategoryId
	};

	public static implicit operator AnalysisCategoriesListDTO(AnalysisCategoriesList analysisCategoriesList) => new()
	{
		Id = analysisCategoriesList.Id,
		AnalysisId = analysisCategoriesList.AnalysisId,
		AnalysisCategoryId = analysisCategoriesList.AnalysisCategoryId
	};
}
