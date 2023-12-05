using System;
using System.Collections.Generic;

namespace library_app.Data;

public partial class Borrowing
{
    public int Id { get; set; }

    public int? MemberId { get; set; }

    public int? BookId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Member? Member { get; set; }
}
