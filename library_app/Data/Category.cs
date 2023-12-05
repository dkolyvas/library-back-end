using System;
using System.Collections.Generic;

namespace library_app.Data;

public partial class Category
{
    public int Id { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
