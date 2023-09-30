using TodoLista.Data;
using TodoLista.Models;
using TodoLista.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TodoLista.Controllers
{
    /// <summary>
    /// Namijenjeno za CRUD operacije na entitetu TodoLista u bazi
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TodoController : ControllerBase
    {

        private readonly TodoListaContext _context;

        public TodoController(TodoListaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Dohvaća sve TodoListe iz baze
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    GET api/v1/TodoLista
        ///
        /// </remarks>
        /// <returns>TodoLista u bazi</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response> 
        [HttpGet]
        public IActionResult Get()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var TodoLista = _context.Todo_lista.Include(t=>t.Korisnik).ToList();
            if (TodoLista == null || TodoLista.Count == 0)
            {
                return new EmptyResult();
            }

            List<Todo_ListaDTO> vrati = new();

            TodoLista.ForEach(p =>
            {
                // ovo je ručno presipavanje, kasnije upogonimo automapper
                var pdto = new Todo_ListaDTO()
                {
                    Sifra = p.Sifra,
                    Naziv = p.Naziv,
                    korisnik=p.Korisnik.korisnicko_ime
                    
                    
                    
                };

                vrati.Add(pdto);


            });


            return Ok(vrati);

        }





        /// <summary>
        /// Dodaje TodoListu u bazu
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    POST api/v1/TodoLista
        ///    {naziv:"",Korisnik:""}
        ///
        /// </remarks>
        /// <returns>Kreirana TodoLista u bazi s svim podacima</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response> 
        [HttpPost]
        public IActionResult Post(Todo_ListaDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Todo_Lista p = new Todo_Lista()
                {
                    Naziv = dto.Naziv,
                    
                    
                    
                };

                _context.Todo_lista.Add(p);
                _context.SaveChanges();
                dto.Sifra = p.Sifra;
                return Ok(dto);

            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status503ServiceUnavailable, ex.Message);
            }
        }





        /// <summary>
        /// Mijenja podatke postojeće TodoListe u bazi
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    PUT api/v1/TodoLista/1
        ///
        /// {
        ///   "sifra": 0,
        ///   "naziv": "string",
        ///   "korisnik": "string",
        ///   
        /// }
        ///
        /// </remarks>
        /// <param name="sifra">Šifra TodoListe koja se mijenja</param>  
        /// <returns>Svi poslani podaci od TodoListe</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="204">Nema u bazi TodoListe koju želimo promijeniti</response>
        /// <response code="415">Nismo poslali JSON</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response> 
        [HttpPut]
        [Route("{sifra:int}")]
        public IActionResult Put(int sifra, Todo_ListaDTO pdto)
        {

            if (sifra <= 0 || pdto == null)
            {
                return BadRequest();
            }

            try
            {
                var TodolistaBaza = _context.Todo_lista.Find(sifra);
                if (TodolistaBaza == null)
                {
                    return BadRequest();
                }
                // inače se rade Mapper-i
                // mi ćemo za sada ručno
                TodolistaBaza.Naziv = pdto.Naziv;
                
                

                _context.Todo_lista.Update(TodolistaBaza);
                _context.SaveChanges();
                pdto.Sifra = TodolistaBaza.Sifra;
                return StatusCode(StatusCodes.Status200OK, pdto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                                  ex); // kada se vrati cijela instanca ex tada na klijentu imamo više podataka o grešci
                // nije dobro vraćati cijeli ex ali za dev je OK
            }


        }



        /// <summary>
        /// Briše TodoListu iz baze
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    DELETE api/v1/TodoLista/1
        ///    
        /// </remarks>
        /// <param name="sifra">Šifra TodoListe koja se briše</param>  
        /// <returns>Odgovor da li je obrisano ili ne</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="204">Nema u bazi TodoListe kojeu želimo obrisati</response>
        /// <response code="415">Nismo poslali JSON</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response> 
        [HttpDelete]
        [Route("{sifra:int}")]
        [Produces("application/json")]
        public IActionResult Delete(int sifra)
        {
            if (sifra <= 0)
            {
                return BadRequest();
            }

            var TodolistaBaza = _context.Todo_lista.Find(sifra);
            if (TodolistaBaza == null)
            {
                return BadRequest();
            }

            try
            {
                _context.Todo_lista.Remove(TodolistaBaza);
                _context.SaveChanges();

                return new JsonResult("{\"poruka\":\"Obrisano\"}");

            }
            catch (Exception ex)
            {

                return new JsonResult("{\"poruka\":\"Ne može se obrisati\"}");

            }

        }








    }
}