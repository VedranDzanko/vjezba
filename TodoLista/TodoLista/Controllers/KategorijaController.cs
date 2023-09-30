using TodoLista.Data;
using TodoLista.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace TodoLista.Controllers
{
    /// <summary>
    /// Namijenjeno za CRUD operacije na entitetom kategorija u bazi
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class KategorijaController : ControllerBase
    {

        // Dependency injection u controller
        
        private readonly TodoListaContext _context;

        public KategorijaController(TodoListaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Dohvaća sve kategorije iz baze
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    GET api/v1/Kategorija
        ///
        /// </remarks>
        /// <returns>Kategorije u bazi</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response> 
        [HttpGet]
        public IActionResult Get()
        {
            // kontrola ukoliko upit nije dobar
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var kategorija = _context.Kategorija.ToList();
                if (kategorija == null || kategorija.Count == 0)
                {
                    return new EmptyResult();
                }
                return new JsonResult(_context.Kategorija.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                                    ex.Message);
            }



        }


        /// <summary>
        /// Dodaje kategoriju u bazu
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    POST api/v1/Kategorija
        ///    {naziv:""}
        ///
        /// </remarks>
        /// <returns>Kreirana kategorija u bazi s svim podacima</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response> 
        [HttpPost]
        public IActionResult Post(Kategorija kategorija)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Kategorija.Add(kategorija);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status201Created, kategorija);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                                   ex.Message);
            }



        }




        /// <summary>
        /// Mijenja podatke postojeće kategorije u bazi
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    PUT api/v1/kategorija/1
        ///
        /// {
        ///  "sifra": 0,
        ///  "naziv": "Novi naziv",
        ///  
        /// }
        ///
        /// </remarks>
        /// <param name="sifra">Šifra kategorije koja se mijenja</param>  
        /// <returns>Svi poslani podaci od kategorije</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="204">Nema u bazi kategorije koje želimo promijeniti</response>
        /// <response code="415">Nismo poslali JSON</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response> 
        [HttpPut]
        [Route("{sifra:int}")]
        public IActionResult Put(int sifra, Kategorija kategorija)
        {

            if (sifra <= 0 || kategorija == null)
            {
                return BadRequest();
            }

            try
            {
                var kategorijaBaza = _context.Kategorija.Find(sifra);
                if (kategorijaBaza == null)
                {
                    return BadRequest();
                }
                
                kategorijaBaza.Naziv = kategorija.Naziv;
               

                _context.Kategorija.Update(kategorijaBaza);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, kategorijaBaza);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                                  ex); 
            }

        }


        /// <summary>
        /// Briše kategoriju iz baze
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    DELETE api/v1/kategorija/1
        ///    
        /// </remarks>
        /// <param name="sifra">Šifra kategorije koji se briše</param>  
        /// <returns>Odgovor da li je obrisano ili ne</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="204">Nema u bazi kategorije koje želimo obrisati</response>
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

            var kategorijaBaza = _context.Kategorija.Find(sifra);
            if (kategorijaBaza == null)
            {
                return BadRequest();
            }

            try
            {
                _context.Kategorija.Remove(kategorijaBaza);
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

