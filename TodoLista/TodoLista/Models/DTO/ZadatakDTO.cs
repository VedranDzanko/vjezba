namespace TodoLista.Models.DTO
{
    public class ZadatakDTO
    {
        public int Sifra { get; set; }
        public string? Naziv { get; set; }
        public DateTime Datum { get; set; }
        public string Todo_lista { get; set; }
        public string Kategorija { get; set; }
        public bool Status { get; set; }
    }
}
