using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

public class DashboardModel : PageModel
{
    public string Username { get; set; }

    public IActionResult OnGet()
    {
        Username = HttpContext.Session.GetString("username");

        if (string.IsNullOrEmpty(Username))
        {
            return RedirectToPage("/Login");
        }

        return Page();
    }
}
