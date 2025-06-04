using System;
using System.Collections.Generic;

namespace MedicWithLoveAPI.ModelsDTO;

public partial class PatientAnalysisCartItemDTO
{
    public int Id { get; set; }

    public int AnalysisId { get; set; }

    public int PatientAnalysisCartId { get; set; }

	public string? ResultsDescription { get; set; }
}
