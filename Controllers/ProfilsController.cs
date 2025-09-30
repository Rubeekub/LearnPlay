using LearnPlay.Interfaces;
using LearnPlay.Models;
using LearnPlay.Metiers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LearnPlay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilsController : ControllerBase
    {
        private readonly IProfilsMt _mt;

        public ProfilsController(IProfilsMt mt)
        {
            _mt = mt;
        }

        // GET api/profils/42
        [HttpGet("{idProf:int}")]
        public ActionResult<Profils> GetProfil(int idProf)
        {
            Profils profil = _mt.GetProfil(idProf);
            if (profil == null)
            {
                return NotFound();
            }

            return Ok(profil);
        }

        // GET api/profils/utilisateurs/7
        [HttpGet("utilisateurs/{idUti:int}")]
        public ActionResult<List<Profils>> GetProfilsUtilisateur(int idUti)
        {
            List<Profils> list = _mt.GetProfilsUtilisateur(idUti);
            return Ok(list);
        }

        // POST api/profils
        [HttpPost]
        public ActionResult<Profils> CreerProfil([FromBody] CreerProfilDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            DateOnly date = DateOnly.MinValue;
            if (dto.dateNaissanceProf.HasValue)
            {
                date = dto.dateNaissanceProf.Value;
            }

            Profils profil = new Profils(
                0,
                dto.numProf,
                dto.pseudoProf,
                dto.pointsProf,
                dto.nivProf,
                date,
                dto.idUtiProf,
                dto.idRoleProf
            );

            Profils created = _mt.CreerProfil(profil);

            object routeValues = new { idProf = created.idProf };
            return CreatedAtAction(nameof(GetProfil), routeValues, created);
        }

        // PUT api/profils/42/pseudo
        [HttpPut("{idProf:int}/pseudo")]
        public IActionResult ModifierPseudo(int idProf, [FromBody] UpdatePseudoDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(dto.pseudo))
            {
                object payload = new { error = "pseudo_required" };
                return BadRequest(payload);
            }

            string newPseudo = dto.pseudo.Trim();
            bool ok = _mt.ModifierPseudo(idProf, newPseudo);

            if (!ok)
            {
                object conflict = new { error = "pseudo_deja_utilise" };
                return Conflict(conflict);
            }

            return NoContent();
        }

        // PUT api/profils/42/points   body: { "delta": 10 }  (mettre une valeur négative pour retirer)
        [HttpPut("{idProf:int}/points")]
        public IActionResult MettreAJourPoints(int idProf, [FromBody] UpdatePointsDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            int delta = dto.delta;
            bool success;

            if (delta > 0)
            {
                success = _mt.AjouterPoints(idProf, delta);
            }
            else if (delta < 0)
            {
                int valeur = -delta;
                success = _mt.RetirerPoints(idProf, valeur);
            }
            else
            {
                object payload = new { error = "delta_zero_interdit" };
                return BadRequest(payload);
            }

            if (!success)
            {
                object payload = new { error = "maj_points_impossible" };
                return BadRequest(payload);
            }

            return NoContent();
        }

        // PUT api/profils/42
        // Mise à jour cœur : pseudo/niv/dateNaissance + themeId + avatarId (tous optionnels)
        [HttpPut("{idProf:int}")]
        public IActionResult MettreAJourProfil(int idProf, [FromBody] UpdateCoreDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            string pseudo = null;
            if (!string.IsNullOrWhiteSpace(dto.pseudo))
            {
                pseudo = dto.pseudo.Trim();
            }

            int? niveau = dto.nivProf;
            DateOnly? date = dto.dateNaissanceProf;
            int? themeId = dto.themeId;
            int? avatarId = dto.avatarId;

            bool ok = _mt.MettreAJourProfil(idProf, pseudo, niveau, date, themeId, avatarId);

            if (!ok)
            {
                object payload = new { error = "maj_impossible" };
                return BadRequest(payload);
            }

            return NoContent();
        }

        // PUT api/profils/utilisateurs/7/profil-actif  body: { "idProf": 42 }
        [HttpPut("utilisateurs/{idUti:int}/profil-actif")]
        public IActionResult ChangerProfilActif(int idUti, [FromBody] SetProfilActifDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            int cible = dto.idProf;
            bool ok = _mt.ChangerProfilActif(idUti, cible);

            if (!ok)
            {
                object payload = new { error = "maj_profil_actif_impossible" };
                return BadRequest(payload);
            }

            return NoContent();
        }

        // GET api/profils/42/theme-actif
        [HttpGet("{idProf:int}/theme-actif")]
        public ActionResult<object> LireThemeActif(int idProf)
        {
            int? themeId = _mt.LireThemeActif(idProf);
            var result = new { themeId = themeId };
            return Ok(result);
        }

        // GET api/profils/42/avatar-actif
        [HttpGet("{idProf:int}/avatar-actif")]
        public ActionResult<object> LireAvatarActif(int idProf)
        {
            int? avatarId = _mt.LireAvatarActif(idProf);
            var result = new { avatarId = avatarId };
            return Ok(result);
        }
    }

    // DTO 

    public record CreerProfilDto(
        int numProf,
        string pseudoProf,
        int pointsProf,
        int nivProf,
        DateOnly? dateNaissanceProf,
        int idUtiProf,
        int idRoleProf
    );

    public record UpdatePseudoDto(string pseudo);

    public record UpdatePointsDto(int delta);

    public class UpdateCoreDto
    {
        public string? pseudo { get; set; }
        public int? nivProf { get; set; }
        public DateOnly? dateNaissanceProf { get; set; }
        public int? themeId { get; set; }
        public int? avatarId { get; set; }
    }

    public record SetProfilActifDto(int idProf);
}
