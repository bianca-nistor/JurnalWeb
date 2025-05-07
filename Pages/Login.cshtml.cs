using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using JurnalWeb.Models;
using JurnalWeb.Services;

public class LoginModel : PageModel
{
    [BindProperty]
    public User User { get; set; }

    public string Mesaj { get; set; }

    public IActionResult OnPost()
    {
        var service = new UserService();

        if (service.ValidateUser(User.Username, User.Password))
        {
            HttpContext.Session.SetString("username", User.Username); // Salvare sesiune
            return RedirectToPage("/Jurnal");
            
        }
        else
        {
            Mesaj = "Utilizator sau parolă incorecte.";
            return Page();
        }
    }

    private List<User> LoadUsers()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "users.json");
        if (!System.IO.File.Exists(path)) return new List<User>();

        var json = System.IO.File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
    }
}
