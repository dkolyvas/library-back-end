namespace library_app.DTO
{
    public class BookShowDTO
    {
        public int Id { get; set; }

        public string? Isbn { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Category { get; set; }
        public int? CategoryId {    get; set;
        }

        public string? Position { get; set; }

        public string? Author
        {
            get; set;
        }
        public bool Available { get; set; }
    }
}
