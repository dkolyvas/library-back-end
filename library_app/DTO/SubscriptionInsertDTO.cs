namespace library_app.DTO
{
    public class SubscriptionInsertDTO
    {
        public int SubscriptionType { get; set; }
        public bool ReplaceOld {  get; set; }

        public int MemberId {  get; set; }
        
    }
}
