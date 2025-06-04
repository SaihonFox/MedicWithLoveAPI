using System;
using System.Collections.Generic;

namespace MedicWithLoveAPI.Models;

public partial class Request
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

    public virtual User Doctor { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;

    public virtual PatientAnalysisCart? PatientAnalysisCart { get; set; }

    public virtual RequestState RequestState { get; set; } = null!;
}
