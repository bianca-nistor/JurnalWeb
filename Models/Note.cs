namespace JurnalWeb.Models
{
    public class Note
    {
        public DateTime Data { get; set; } = DateTime.Now;
        public string Titlu { get; set; }
        public string Continut { get; set; }
    }
}
