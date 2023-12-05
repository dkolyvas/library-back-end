namespace library_app.Exceptions
{
    public class EmailExistsException: Exception
    {
        public EmailExistsException()
        :base("The email you submited already exists")
        { }
    }
}
