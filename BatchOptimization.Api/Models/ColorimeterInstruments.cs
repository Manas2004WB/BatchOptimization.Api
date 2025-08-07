using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class ColorimeterInstruments
{
    public int ColorimeterInstrumentId { get; set; }

    public string ColorimeterInstrument { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Users? CreatedByNavigation { get; set; }

    public virtual Users? UpdatedByNavigation { get; set; }
}
