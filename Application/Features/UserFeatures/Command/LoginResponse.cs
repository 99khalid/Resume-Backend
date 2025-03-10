namespace Application.Features.UserFeatures.Command
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
    }
}
