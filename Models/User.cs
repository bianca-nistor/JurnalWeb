using Microsoft.AspNetCore.Mvc;

namespace JurnalWeb.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string BgColor { get; set; } = "#fff9f5";
        public string TextColor { get; set; } = "#333333";


    }

}
