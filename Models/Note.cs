namespace JurnalWeb.Models
{
    public class Note
    {
        public DateTime Data { get; set; } = DateTime.Now;
        public string Titlu { get; set; }
        public string Continut { get; set; }
        public string CuloareText { get; set; } = "#333333"; // culoare pastel implicită
        public string CuloareFundal { get; set; } = "#fff9f5"; // fundal pastel implicit
        public Guid Id { get; set; } = Guid.NewGuid(); // pentru ștergere unică

    }
}
