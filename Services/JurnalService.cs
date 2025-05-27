using JurnalWeb.Models;
using System.Text.Json;

namespace JurnalWeb.Services
{
    public class JurnalService
    {
        private string GetUserFilePath(string username)
        {
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return Path.Combine(folder, $"{username}_notes.json");
        }

       // private string GetPath(string username) => $"jurnal_{username}.json";
        public List<Note> LoadNotes(string username)
        {
            string path = GetUserFilePath(username);
            if (!File.Exists(path)) return new List<Note>();
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Note>>(json) ?? new List<Note>();
        }

        public void SaveNote(string username, Note nota)
        {
            var notes = LoadNotes(username);
            nota.Id = Guid.NewGuid();
            notes.Add(nota);
            string json = JsonSerializer.Serialize(notes);
            File.WriteAllText(GetUserFilePath(username), json);
        }

        public void DeleteNote(string username, Guid id)
        {
            var path = GetUserFilePath(username);
            if (!File.Exists(path))
                return;

            var notes = JsonSerializer.Deserialize<List<Note>>(File.ReadAllText(path)) ?? new List<Note>();
            var updatedNotes = notes.Where(n => n.Id != id).ToList();
            File.WriteAllText(path, JsonSerializer.Serialize(updatedNotes));
        }
        public void UpdateNote(string username, Guid id, string titluNou, string continutNou)
        {
            var path = GetUserFilePath(username);
            if (!File.Exists(path)) return;

            var notes = JsonSerializer.Deserialize<List<Note>>(File.ReadAllText(path)) ?? new List<Note>();
            var note = notes.FirstOrDefault(n => n.Id == id);
            if (note != null)
            {
                note.Titlu = titluNou;
                note.Continut = continutNou;
                File.WriteAllText(path, JsonSerializer.Serialize(notes));
            }
        }


    }

}