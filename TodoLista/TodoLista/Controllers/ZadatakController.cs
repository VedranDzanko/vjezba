﻿using TodoLista.Data;
using TodoLista.Models;
using TodoLista.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TodoLista.Controllers
{
    /// <summary>
    /// Namijenjeno za CRUD operacije na entiteu zadatak u bazi
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ZadatakController : ControllerBase
    {

        private readonly TodoListaContext _context;

        public ZadatakController(TodoListaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Dohvaća sve zadatke iz baze
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    GET api/v1/Zadatak
        ///
        /// </remarks>
        /// <returns>Polaznici u bazi</returns>
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

            var Zadatak = _context.Zadatak.Include(t => t.Kategorija).ToList();
            if (Zadatak == null || Zadatak.Count == 0)
            {
                return new EmptyResult();
            }

            List<ZadatakDTO> vrati = new();

            Zadatak.ForEach(p =>
            {
                // ovo je ručno presipavanje, kasnije upogonimo automapper
                var pdto = new ZadatakDTO()
                {
                    Sifra = p.Sifra,
                    Naziv = p.Naziv,
                    Kategorija = p.Kategorija?.Naziv



                };

                vrati.Add(pdto);


            });


            return Ok(vrati);

        }





        /// <summary>
        /// Dodaje zadatak u bazu
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    POST api/v1/Zadatak
        ///    {Ime:"",Prezime:""}
        ///
        /// </remarks>
        /// <returns>Kreirani zadatak u bazi s svim podacima</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="400">Zahtjev nije valjan (BadRequest)</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response> 
        [HttpPost]
        public IActionResult Post(ZadatakDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Zadatak p = new Zadatak()
                {
                    Naziv = dto.Naziv,
                    Datum = dto.Datum,
                    Status = dto.Status
                   


                };

                _context.Zadatak.Add(p);
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
        /// Mijenja podatke postojećeg zadatka u bazi
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    PUT api/v1/Zadatak/1
        ///
        /// {
        ///   sifra,
        ///naziv,
	    ///datum datetime,
        // status ,
	    //todo_lista ,
	    //kategorija 
        /// }
        ///
        /// </remarks>
        /// <param name="sifra">Šifra zadatka koji se mijenja</param>  
        /// <returns>Svi poslani podaci od zadatka</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="204">Nema u bazi zadatak kojeg želimo promijeniti</response>
        /// <response code="415">Nismo poslali JSON</response> 
        /// <response code="503">Na azure treba dodati IP u firewall</response> 
        [HttpPut]
        [Route("{sifra:int}")]
        public IActionResult Put(int sifra, Zadatak pdto)
        {

            if (sifra <= 0 || pdto == null)
            {
                return BadRequest();
            }

            try
            {
                var ZadatakBaza = _context.Zadatak.Find(sifra);
                if (ZadatakBaza == null)
                {
                    return BadRequest();
                }
               
                ZadatakBaza.Naziv = pdto.Naziv;



                _context.Zadatak.Update(ZadatakBaza);
                _context.SaveChanges();
                pdto.Sifra = ZadatakBaza.Sifra;
                return StatusCode(StatusCodes.Status200OK, pdto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable,
                                  ex); 
            }


        }



        /// <summary>
        /// Briše zadatak iz baze
        /// </summary>
        /// <remarks>
        /// Primjer upita:
        ///
        ///    DELETE api/v1/Zadatak/1
        ///    
        /// </remarks>
        /// <param name="sifra">Šifra zadatka koji se briše</param>  
        /// <returns>Odgovor da li je obrisano ili ne</returns>
        /// <response code="200">Sve je u redu</response>
        /// <response code="204">Nema u bazi zadatka kojeg želimo obrisati</response>
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

            var ZadatakBaza = _context.Zadatak.Find(sifra);
            if (ZadatakBaza == null)
            {
                return BadRequest();
            }

            try
            {
                _context.Zadatak.Remove(ZadatakBaza);
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