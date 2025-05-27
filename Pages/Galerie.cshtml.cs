using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using JurnalWeb.Models;

namespace JurnalWeb.Pages
{
    public class GalerieModel : PageModel
    {
        [BindProperty]
        public IFormFile ImageFile { get; set; }

        [BindProperty]
        public string Note { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }

        public List<GalleryEntry> Entries { get; set; } = new List<GalleryEntry>();
        public User CurrentUser { get; set; }

        public GalerieModel()
        {
            // Constructor gol - HttpContext nu este disponibil aici
        }

        public void OnGet()
        {
            var username = HttpContext.Session.GetString("username") ?? "anonim";
            var dataFile = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", $"gallery_{username}.json");

            CurrentUser = new User
            {
                Username = username,
                BgColor = "#fff9f5",
                TextColor = "#333333"
            };

            if (System.IO.File.Exists(dataFile))
            {
                var json = System.IO.File.ReadAllText(dataFile);
                var allEntries = JsonSerializer.Deserialize<List<GalleryEntry>>(json) ?? new List<GalleryEntry>();

                if (!string.IsNullOrEmpty(Search))
                {
                    Entries = allEntries
                        .Where(e => e.Note.Contains(Search, StringComparison.OrdinalIgnoreCase) ||
                                    e.FileName.Contains(Search, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                else
                {
                    Entries = allEntries;
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var username = HttpContext.Session.GetString("username") ?? "anonim";
            var dataFile = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", $"gallery_{username}.json");

            OnGet(); // Încarcă deja salvările existente

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                Entries.Add(new GalleryEntry
                {
                    FileName = fileName,
                    Note = Note ?? ""
                });

                // Salvare în fișier JSON specific utilizatorului
                var json = JsonSerializer.Serialize(Entries);
                var appData = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
                if (!Directory.Exists(appData)) Directory.CreateDirectory(appData);
                System.IO.File.WriteAllText(dataFile, json);
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(string filename)
        {
            var username = HttpContext.Session.GetString("username") ?? "anonim";
            var dataFile = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", $"gallery_{username}.json");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", filename);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            if (System.IO.File.Exists(dataFile))
            {
                var json = System.IO.File.ReadAllText(dataFile);
                var list = JsonSerializer.Deserialize<List<GalleryEntry>>(json) ?? new List<GalleryEntry>();
                list = list.Where(e => e.FileName != filename).ToList();
                System.IO.File.WriteAllText(dataFile, JsonSerializer.Serialize(list));
            }

            return RedirectToPage();
        }
    }
}
