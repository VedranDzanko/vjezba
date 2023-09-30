using TodoLista.Data;
using TodoLista.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using TodoLista.Models.DTO;

namespace TodoLista.Controllers
{
    /// <summary>
    /// Namijenjeno za CRUD operacije na entitetom smjer u bazi
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class KorisnikController : ControllerBase
    {

        // Dependency injection u controller

        private readonly TodoListaContext _context;

        public KorisnikController(TodoListaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Dohvaća sve korisnike iz baze
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    GET api/v1/Korisnik
        ///
        /// </remarks>
        /// <returns>Korisnik u bazi</returns>
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
            var korisnici = _context.Korisnik.ToList();
            if (korisnici == null || korisnici.Count == 0)
            {
                return new EmptyResult();
            }
            List<KorisnikDTO> korisnik = new();
            korisnici.ForEach(k =>
            {
                var dto = new KorisnikDTO()
                {
                    ime = k.ime,
                    prezime = k.prezime,
                    korisnicko_ime = k.korisnicko_ime,
                    sifra = k.Sifra
                    
                };
                korisnik.Add(dto);
            });
            return Ok(korisnik);
            

            


        }


        /// <summary>
        /// Dodaje korisnika u bazu
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    POST api/v1/Korisnik
        ///    {ime:"",prezime:"",korisnicko ime:"",lozinka:}
        ///
        /// </remarks>
        /// <returns>Kreirani korisnik u bazi s svim podacima</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response> 
        [HttpPost]
        public IActionResult Post(Korisnik korisnik)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Korisnik.Add(korisnik);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status201Created, korisnik);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                                   ex.Message);
            }



        }




        /// <summary>
        /// Mijenja podatke postojećeg korisnika u bazi
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    PUT api/v1/korisnik/1
        ///
        /// {
        ///  "sifra": 0,
        ///  "ime": "",
        ///  "prezime": "",
        ///  "korisnicko ime": "",
        ///  "lozinka": 0,
        ///  
        /// }
        ///
        /// </remarks>
        /// <param name="sifra">Šifra korisnika koji se mijenja</param>  
        /// <returns>Svi poslani podaci od korisnika</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="204">Nema u bazi korisnika kojeg želimo promijeniti</response>
        /// <response code="415">Nismo poslali JSON</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response> 
        [HttpPut]
        [Route("{sifra:int}")]
        public IActionResult Put(int sifra, Korisnik korisnik)
        {

            if (sifra <= 0 || korisnik == null)
            {
                return BadRequest();
            }

            try
            {
                var korisnikBaza = _context.Korisnik.Find(sifra);
                if (korisnikBaza == null)
                {
                    return BadRequest();
                }

                korisnikBaza.ime = korisnik.ime;
                korisnikBaza.prezime = korisnik.prezime;
                korisnikBaza.korisnicko_ime = korisnik.korisnicko_ime;


                _context.Korisnik.Update(korisnikBaza);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, korisnikBaza);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                                  ex);
            }

        }


        /// <summary>
        /// Briše korisnika iz baze
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    DELETE api/v1/korisnik/1
        ///    
        /// </remarks>
        /// <param name="sifra">Šifra korisnika koji se briše</param>  
        /// <returns>Odgovor da li je obrisano ili ne</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="204">Nema u bazi korisnika kojeg želimo obrisati</response>
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

            var korisnikBaza = _context.Korisnik.Find(sifra);
            if (korisnikBaza == null)
            {
                return BadRequest();
            }

            try
            {
                _context.Korisnik.Remove(korisnikBaza);
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