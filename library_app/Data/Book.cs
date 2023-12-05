using System;
using System.Collections.Generic;

namespace library_app.Data;

public partial class Book
{
    public int Id { get; set; }

    public string? Isbn { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? CategoryId { get; set; }

    public string? Position { get; set; }

    public string? Author { get; set; }

    public virtual ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();

    public virtual Category? Category { get; set; }
}
