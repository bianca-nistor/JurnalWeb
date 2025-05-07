using JurnalWeb.Models;
using System.Text.Json;

namespace JurnalWeb.Services
{
    public class JurnalService
    {
        private string GetPath(string username) => $"jurnal_{username}.json";
        public List<Note>LoadNotes(string username)
        {
            string path = GetPath(username);
            if (!File.Exists(path)) return new List<Note>();
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Note>>(json) ?? new List<Note>();
        }
        public void SaveNote(string username, Note nota)
        {
            var note = LoadNotes(username);
            note.Add(nota);
            string json = JsonSerializer.Serialize(note);
            File.WriteAllText(GetPath(username), json);
        }
    }
}
