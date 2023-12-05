using System;
using System.Collections.Generic;

namespace library_app.Data;

public partial class Subscription
{
    public int Id { get; set; }

    public int? MemberId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? Alowance { get; set; }

    public virtual Member? Member { get; set; }
}
