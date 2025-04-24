using System.Threading.Tasks;
using csc13001_plant_pos.Model;

namespace csc13001_plant_pos.Service
{
    public class UserSessionService
    {
        private readonly IAuthenticationService _authService;
        public UserSessionService(IAuthenticationService authService) {
            _authService = authService;
        }
        public User CurrentUser { get; private set; }
        
        public string AccessToken { get; private set; }

        public void SetUser(User user) 
        {
            CurrentUser = user;
        }

        public void SetAccessToken(string accessToken) 
        {
            AccessToken = accessToken;
        }

        public async Task ClearUser()
        {
            await _authService.LogoutAsync(this.AccessToken);
            CurrentUser = null;
            AccessToken = null;
        }
    }
}
