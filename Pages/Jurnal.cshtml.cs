using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JurnalWeb.Models;
using JurnalWeb.Services;

public class JurnalModel : PageModel
{
    public List<Note> Note { get; set; } = new();
    [BindProperty]
    public Note Nota { get; set; }

    public IActionResult OnGet()
    {
        var username = HttpContext.Session.GetString("username");
        if (string.IsNullOrEmpty(username))
            return RedirectToPage("/Login");

        var service = new JurnalService();
        Note = service.LoadNotes(username);
        return Page();
    }

    public IActionResult OnPost()
    {
        var username = HttpContext.Session.GetString("username");
        if (string.IsNullOrEmpty(username))
            return RedirectToPage("/Login");

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var service = new JurnalService();
        service.SaveNote(username, Nota);
        return RedirectToPage();
    }
}
