using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JurnalWeb.Models;
using JurnalWeb.Services;

public class SignupModel : PageModel
{
    [BindProperty]
    public User User { get; set; }

    public string Mesaj { get; set; }

    public IActionResult OnPost()
    {
        var service = new UserService();

        if (service.UserExists(User.Username,User.Password))
        {
            Mesaj = "Acest nume de utilizator este deja folosit.";
            return Page();
        }

        service.AddUser(User);
        Mesaj = "Cont creat cu succes! Mergi la login.";

        return Page(); 
    }
}
