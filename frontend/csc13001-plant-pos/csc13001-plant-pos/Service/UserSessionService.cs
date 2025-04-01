using csc13001_plant_pos.Model;

namespace csc13001_plant_pos.Service
{
    public class UserSessionService
    {
        public User CurrentUser { get; private set; }
        public void SetUser(User user)
        {
            CurrentUser = user;
        }
        public void ClearUser()
        {
            CurrentUser = null;
        }
    }
}
