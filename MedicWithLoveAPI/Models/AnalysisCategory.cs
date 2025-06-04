﻿using System;
using System.Collections.Generic;

namespace MedicWithLoveAPI.Models;

public partial class AnalysisCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AnalysisCategoriesList> AnalysisCategoriesLists { get; set; } = new List<AnalysisCategoriesList>();
}
