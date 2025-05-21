using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace JurnalWeb.Pages
{
    public class GalerieModel : PageModel
    {
        private readonly string _dataFile;

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        [BindProperty]
        public string Note { get; set; }

        public List<GalleryEntry> Entries { get; set; } = new List<GalleryEntry>();

        public class GalleryEntry
        {
            public string FileName { get; set; }
            public string Note { get; set; }
        }

        public GalerieModel()
        {
            _dataFile = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "gallery.json");
        }

        public void OnGet()
        {
            if (System.IO.File.Exists(_dataFile))
            {
                var json = System.IO.File.ReadAllText(_dataFile);
                Entries = JsonSerializer.Deserialize<List<GalleryEntry>>(json) ?? new List<GalleryEntry>();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            OnGet(); // Încarc? deja salv?rile existente

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

                // Salvare în fi?ier JSON
                var json = JsonSerializer.Serialize(Entries);
                var appData = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
                if (!Directory.Exists(appData)) Directory.CreateDirectory(appData);
                System.IO.File.WriteAllText(_dataFile, json);
            }

            return RedirectToPage();
        }
        public IActionResult OnPostDelete(string filename)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", filename);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            // Șterge din gallery.json
            string jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "gallery.json");
            if (System.IO.File.Exists(jsonPath))
            {
                var json = System.IO.File.ReadAllText(jsonPath);
                var list = JsonSerializer.Deserialize<List<GalleryEntry>>(json) ?? new List<GalleryEntry>();
                list = list.Where(e => e.FileName != filename).ToList();
                System.IO.File.WriteAllText(jsonPath, JsonSerializer.Serialize(list));
            }

            return RedirectToPage();
        }

    }
}
