using System;
using System.Collections.Generic;

namespace library_app.Data;

public partial class SubscriptionType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Duration { get; set; }

    public int? Allowance { get; set; }
}
