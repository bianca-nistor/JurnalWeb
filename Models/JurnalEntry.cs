namespace JurnalWeb.Models
{
    public class JurnalEntry
    {
        public int Id { get; set; }
        public string Titlu { get; set; }
        public string Continut { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
    }
}
