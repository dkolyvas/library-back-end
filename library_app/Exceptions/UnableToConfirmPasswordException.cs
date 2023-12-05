namespace library_app.Exceptions
{
    public class UnableToConfirmPasswordException: Exception
    {
        public UnableToConfirmPasswordException() :base("You must submit twice the same password")
        { }
    }
}
