namespace library_app.Exceptions
{
    public class IncorrectPasswordException : Exception
    {
       public IncorrectPasswordException() :base("The password you have submitted is incorrect")
        { }
    }
}
