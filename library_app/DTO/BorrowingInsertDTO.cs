using System.ComponentModel.DataAnnotations;

namespace library_app.DTO
{
    public class BorrowingInsertDTO
    {

        
        public int MemberId { get; set; }
        
        public int BookId { get; set; }

    }
}
