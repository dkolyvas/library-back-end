namespace library_app.Exceptions
{
    public class BorrowingNotAllowedException : Exception
    {
        public BorrowingNotAllowedException(string message)
            :base($"Borrowing is not allowed: {message}!")
        {
        }
    }
}
