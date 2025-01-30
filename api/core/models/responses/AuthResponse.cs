namespace api.core.models.responses
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public UserDto? User{ get; set; }
    }
}