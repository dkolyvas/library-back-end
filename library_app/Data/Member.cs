using System;
using System.Collections.Generic;

namespace library_app.Data;

public partial class Member
{
    public int Id { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
