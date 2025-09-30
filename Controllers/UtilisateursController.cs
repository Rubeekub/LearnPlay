using LearnPlay.DTO;
using LearnPlay.Interfaces;
using LearnPlay.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LearnPlay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtilisateursController : ControllerBase
    {
        private readonly IUtilisateursMt _mt;

        public UtilisateursController(IUtilisateursMt mt)
        {
            _mt = mt;
        }

        // GET: api/utilisateurs
        [HttpGet]
        public ActionResult<List<Utilisateurs>> GetAll()
        {
            List<Utilisateurs> list = _mt.Lister();
            return Ok(list);
        }

        // GET: api/utilisateurs/5
        [HttpGet("{idUti:int}")]
        public ActionResult<Utilisateurs> GetById(int idUti)
        {
            Utilisateurs u = _mt.Lire(idUti);
            if (u == null)
            {
                return NotFound();
            }

            return Ok(u);
        }

        // GET: api/utilisateurs/by-email?email=...
        [HttpGet("by-email")]
        public ActionResult<Utilisateurs> GetByEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                object payload = new { error = "email_required" };
                return BadRequest(payload);
            }

            Utilisateurs u = _mt.LireParEmail(email);
            if (u == null)
            {
                return NotFound();
            }

            return Ok(u);
        }

        // POST: api/utilisateurs
        // Remarque : pour rester aligné avec ta couche métier actuelle,
        // on attend un hash déjà calculé (byte[]). Le JSON peut l'envoyer en base64.
        [HttpPost]
        public ActionResult<Utilisateurs> Create([FromBody] CreateUtilisateurDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(dto.MailUti))
            {
                object payload = new { error = "email_required" };
                return BadRequest(payload);
            }

            if (dto.MdpUti == null || dto.MdpUti.Length == 0)
            {
                object payload = new { error = "password_hash_required" };
                return BadRequest(payload);
            }

            string nom = string.Empty;
            if (!string.IsNullOrWhiteSpace(dto.NomUti))
            {
                nom = dto.NomUti.Trim();
            }

            string prenom = string.Empty;
            if (!string.IsNullOrWhiteSpace(dto.PrenomUti))
            {
                prenom = dto.PrenomUti.Trim();
            }

            Utilisateurs created = _mt.Enregistrer(nom, prenom, dto.MailUti, dto.MdpUti);

            object routeValues = new { idUti = created.IdUti };
            return CreatedAtAction(nameof(GetById), routeValues, created);
        }

        // PUT: api/utilisateurs/5/identite
        [HttpPut("{idUti:int}/identite")]
        public IActionResult UpdateIdentite(int idUti, [FromBody] UpdateIdentiteDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            string nom = null;
            if (!string.IsNullOrWhiteSpace(dto.NomUti))
            {
                nom = dto.NomUti.Trim();
            }

            string prenom = null;
            if (!string.IsNullOrWhiteSpace(dto.PrenomUti))
            {
                prenom = dto.PrenomUti.Trim();
            }

            bool ok = _mt.ModifierIdentite(idUti, nom, prenom);
            if (!ok)
            {
                object payload = new { error = "maj_identite_impossible" };
                return BadRequest(payload);
            }

            return NoContent();
        }

        // PUT: api/utilisateurs/5/email
        [HttpPut("{idUti:int}/email")]
        public IActionResult UpdateEmail(int idUti, [FromBody] UpdateEmailDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(dto.MailUti))
            {
                object payload = new { error = "email_required" };
                return BadRequest(payload);
            }

            string email = dto.MailUti.Trim();
            bool ok = _mt.ModifierEmail(idUti, email);
            if (!ok)
            {
                object conflict = new { error = "email_deja_utilise" };
                return Conflict(conflict);
            }

            return NoContent();
        }

        // PUT: api/utilisateurs/5/password
        // On attend un hash (byte[]). Si tu veux accepter le mot de passe "clair",
        // on fera évoluer la couche Métier pour hasher là-bas (sans rien changer ici).
        [HttpPut("{idUti:int}/password")]
        public IActionResult UpdatePassword(int idUti, [FromBody] UpdatePasswordDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            if (dto.MdpUti == null || dto.MdpUti.Length == 0)
            {
                object payload = new { error = "password_hash_required" };
                return BadRequest(payload);
            }

            bool ok = _mt.ModifierMotDePasse(idUti, dto.MdpUti);
            if (!ok)
            {
                object payload = new { error = "maj_password_impossible" };
                return BadRequest(payload);
            }

            return NoContent();
        }

        // DELETE: api/utilisateurs/5
        [HttpDelete("{idUti:int}")]
        public IActionResult Delete(int idUti)
        {
            bool ok = _mt.Supprimer(idUti);
            if (!ok)
            {
                object payload = new { error = "suppression_impossible" };
                return BadRequest(payload);
            }

            return NoContent();
        }
    }
}
