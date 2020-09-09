using NetCoreTutorial.Domain;

namespace NetCoreTutorial.Helpers
{
    public class AppUser
    {
        public long Id { get; }
        public string Username { get; }

        public AppUser(User user)
        {
            Id = user.Id;
            Username = user.Username;
        }
    }
}