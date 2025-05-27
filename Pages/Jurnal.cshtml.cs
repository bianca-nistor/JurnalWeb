using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JurnalWeb.Models;
using JurnalWeb.Services;
using System.Text.Json;
using System.IO;

namespace JurnalWeb.Pages
{
    public class JurnalModel : PageModel
    {
        private readonly JurnalService _jurnalService;

        public JurnalModel()
        {
            _jurnalService = new JurnalService();
        }

        public List<Note> Note { get; set; } = new List<Note>();

        [BindProperty]
        public Note Nota { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }

        public IActionResult OnGet()
        {
            var username = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/Login");

            var toateNotele = _jurnalService.LoadNotes(username);

            Note = string.IsNullOrEmpty(Search)
                ? toateNotele
                : toateNotele.Where(n =>
                       n.Titlu.Contains(Search, StringComparison.OrdinalIgnoreCase) ||
                       n.Continut.Contains(Search, StringComparison.OrdinalIgnoreCase)).ToList();

            return Page();
        }

        public IActionResult OnPost()
        {
            var username = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/Login");

            var notes = _jurnalService.LoadNotes(username);

            Nota.Id = Guid.NewGuid();
            Nota.Data = DateTime.Now;
            notes.Add(Nota);

            var json = JsonSerializer.Serialize(notes);
            System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "App_Data", $"{username}_notes.json"), json);

            TempData["Message"] = "Notița a fost salvată!";
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(Guid id)
        {
            var username = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/Login");

            _jurnalService.DeleteNote(username, id);
            return RedirectToPage();
        }
    }
}
