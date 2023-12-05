namespace library_app.DTO
{
    public class SubscriptionShowDTO
    {
        public int Id { get; set; }

        public int? MemberId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Alowance { get; set; }
    }
}
