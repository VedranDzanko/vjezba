using System.ComponentModel.DataAnnotations;

namespace TodoLista.Models
{
    public abstract class Entitet
    {
        [Key]
        public int Sifra { get; set; }
    }
}
