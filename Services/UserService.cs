using JurnalWeb.Models;
using System.Text.Json;

namespace JurnalWeb.Services
{
    public class UserService
    {
        private readonly string filePath = "users.json";
        public List<User> GetAllUsers()
        {
            if (!File.Exists(filePath))
            {
                return new List<User>();
            }
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }
        public void SaveAllUsers(List<User> users)
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
        public bool UserExists(string username, string password)
        {
            return GetAllUsers().Any(u => u.Username == username && u.Password == password);
        }
        public void AddUser(User user)
        {
            var users = GetAllUsers();
            users.Add(user);
            SaveAllUsers(users);

        }

        public bool ValidateUser(string username, string password)
        {
            var users =GetAllUsers();
            return users.Any(u => u.Username == username && u.Password == password);
        }

    }
}