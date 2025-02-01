namespace api.core.models
{
    public class CreateUser
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Deleted { get; set; }
    }
}