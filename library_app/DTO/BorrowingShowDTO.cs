namespace library_app.DTO
{
    public class BorrowingShowDTO
    {
        public int Id { get; set; }

        public string? MemberName { get; set; }

        public string? BookTitle { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? MemberAddress { get; set; }
        public string? MemberEmail { get; set; }
        public string? MemberPhone { get; set; }
        public int? BookId { get; set; }

    }
}
