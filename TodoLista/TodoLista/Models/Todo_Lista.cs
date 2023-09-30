using System.ComponentModel.DataAnnotations.Schema;

namespace TodoLista.Models
{
    public class Todo_Lista:Entitet
    {
        [ForeignKey("korisnik")]
        public Korisnik Korisnik { get; set; }
        public string? Naziv { get; set; } 
    }
}
