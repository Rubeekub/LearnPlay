// Controllers/UtilisateursController.cs
using LearnPlay.DTO;
using LearnPlay.Interfaces;
using LearnPlay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient; // pour SqlException (duplicate key)
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
            ActionResult<List<Utilisateurs>> result = Ok(list);
            return result;
        }

        // GET: api/utilisateurs/5
        [HttpGet("{idUti:int}")]
        public ActionResult<Utilisateurs> GetById(int idUti)
        {
            Utilisateurs u = _mt.Lire(idUti);

            if (u == null)
            {
                ActionResult<Utilisateurs> notFound = NotFound();
                return notFound;
            }

            ActionResult<Utilisateurs> ok = Ok(u);
            return ok;
        }

        // GET: api/utilisateurs/by-email?email=...
        [HttpGet("by-email")]
        public ActionResult<Utilisateurs> GetByEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                object payload = new { error = "email_required" };
                ActionResult<Utilisateurs> bad = BadRequest(payload);
                return bad;
            }

            Utilisateurs u = _mt.LireParEmail(email);

            if (u == null)
            {
                ActionResult<Utilisateurs> notFound = NotFound();
                return notFound;
            }

            ActionResult<Utilisateurs> ok = Ok(u);
            return ok;
        }

        // POST: api/utilisateurs
        // Remarque : on attend un hash déjà calculé (byte[]). Le JSON peut l'envoyer en Base64.
        [HttpPost]
        public ActionResult<Utilisateurs> Create([FromBody] CreateUtilisateurDto dto)
        {
            if (dto == null)
            {
                ActionResult<Utilisateurs> bad = BadRequest();
                return bad;
            }

            if (string.IsNullOrWhiteSpace(dto.MailUti))
            {
                object payload = new { error = "email_required" };
                ActionResult<Utilisateurs> bad = BadRequest(payload);
                return bad;
            }

            if (dto.MdpUti == null || dto.MdpUti.Length == 0)
            {
                object payload = new { error = "password_hash_required" };
                ActionResult<Utilisateurs> bad = BadRequest(payload);
                return bad;
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

            try
            {
                Utilisateurs created = _mt.Enregistrer(nom, prenom, dto.MailUti, dto.MdpUti);
                object routeValues = new { idUti = created.IdUti };
                ActionResult<Utilisateurs> result = CreatedAtAction(nameof(GetById), routeValues, created);
                return result;
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                // Violation d'unicité (mailUti UNIQUE)
                object conflict = new { error = "email_deja_utilise" };
                ActionResult<Utilisateurs> r = Conflict(conflict);
                return r;
            }
        }

        // PUT: api/utilisateurs/5/identite
        [HttpPut("{idUti:int}/identite")]
        public IActionResult UpdateIdentite(int idUti, [FromBody] UpdateIdentiteDto dto)
        {
            if (dto == null)
            {
                IActionResult bad = BadRequest();
                return bad;
            }

            string? nom = null;
            if (!string.IsNullOrWhiteSpace(dto.NomUti))
            {
                nom = dto.NomUti.Trim();
            }

            string? prenom = null;
            if (!string.IsNullOrWhiteSpace(dto.PrenomUti))
            {
                prenom = dto.PrenomUti.Trim();
            }

            bool okUpdate = _mt.ModifierIdentite(idUti, nom, prenom);
            if (!okUpdate)
            {
                object payload = new { error = "maj_identite_impossible" };
                IActionResult bad = BadRequest(payload);
                return bad;
            }

            IActionResult no = NoContent();
            return no;
        }

        // PUT: api/utilisateurs/5/email
        [HttpPut("{idUti:int}/email")]
        public IActionResult UpdateEmail(int idUti, [FromBody] UpdateEmailDto dto)
        {
            if (dto == null)
            {
                IActionResult bad = BadRequest();
                return bad;
            }

            if (string.IsNullOrWhiteSpace(dto.MailUti))
            {
                object payload = new { error = "email_required" };
                IActionResult bad = BadRequest(payload);
                return bad;
            }

            string email = dto.MailUti.Trim();

            try
            {
                bool okUpdate = _mt.ModifierEmail(idUti, email);
                if (!okUpdate)
                {
                    object payload = new { error = "maj_email_impossible" };
                    IActionResult bad = BadRequest(payload);
                    return bad;
                }

                IActionResult no = NoContent();
                return no;
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                object conflict = new { error = "email_deja_utilise" };
                IActionResult r = Conflict(conflict);
                return r;
            }
        }

        // PUT: api/utilisateurs/5/password
        [HttpPut("{idUti:int}/password")]
        public IActionResult UpdatePassword(int idUti, [FromBody] UpdatePasswordDto dto)
        {
            if (dto == null)
            {
                IActionResult bad = BadRequest();
                return bad;
            }

            if (dto.MdpUti == null || dto.MdpUti.Length == 0)
            {
                object payload = new { error = "password_hash_required" };
                IActionResult bad = BadRequest(payload);
                return bad;
            }

            bool okUpdate = _mt.ModifierMotDePasse(idUti, dto.MdpUti);
            if (!okUpdate)
            {
                object payload = new { error = "maj_password_impossible" };
                IActionResult bad = BadRequest(payload);
                return bad;
            }

            IActionResult no = NoContent();
            return no;
        }

        // DELETE: api/utilisateurs/5
        [HttpDelete("{idUti:int}")]
        public IActionResult Delete(int idUti)
        {
            bool okDelete = _mt.Supprimer(idUti);
            if (!okDelete)
            {
                object payload = new { error = "suppression_impossible" };
                IActionResult bad = BadRequest(payload);
                return bad;
            }

            IActionResult no = NoContent();
            return no;
        }
    }
}
