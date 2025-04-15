using System;

namespace csc13001_plant_pos.Model
{
    public class RememberedCredential
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime LastUsed { get; set; }

        public RememberedCredential(string username, string password)
        {
            Username = username;
            Password = password;
            LastUsed = DateTime.Now;
        }

        public override string ToString()
        {
            return Username;
        }
    }
}