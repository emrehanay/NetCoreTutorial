using NetCoreTutorial.Domain;

namespace NetCoreTutorial.Model
{
    public class LoginResponse
    {
        public User User { get; }
        public string Token { get; }

        public LoginResponse(User user, string token)
        {
            User = user;
            Token = token;
        }
    }
}