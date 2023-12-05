namespace library_app.Exceptions
{
    public class UserExistsException: Exception
    {
        public UserExistsException(string username): base($"A user with username {username} already exists")
        { }
    }
}
