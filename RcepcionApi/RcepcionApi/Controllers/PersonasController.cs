using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation;
using RcepcionApi.Data;
using RcepcionApi.EntityModels;

namespace RcepcionApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly RecepcionDbContext _context;
        private readonly UserManager<UsuarioEntity> _userManager;

        public PersonasController( RecepcionDbContext context , UserManager<UsuarioEntity> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var user = _userManager.GetUserId(HttpContext.User);
            if (User == null) {
                return StatusCode(404, "Usuario no registrado");
            }
            return StatusCode(200, await _context.Personas.ToListAsync());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int? id) {
            try
            {
                var user = _userManager.GetUserId(HttpContext.User);
                if (User == null)
                {
                    return StatusCode(404, "Usuario no registrado");
                }

                if (id == null) {
                    return StatusCode(404,"El identificador es nulo.");
                }
                var Persona = await _context.Personas.FindAsync(id);
                if (Persona == null) {
                    return StatusCode(404, "La persona no fue encontrada.");
                }

                return StatusCode(200, Persona);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PersonaEntity model) {
            try
            {
                var user = _userManager.GetUserId(HttpContext.User);
                if (User == null)
                {
                    return StatusCode(404, "Usuario no registrado");
                }

                if (!ModelState.IsValid) {
                    return StatusCode(400, "Modelo no válido.");
                }
                await _context.Personas.AddAsync(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetById", "Personas", new { id = model.Id});
            }
            catch (Exception e) 
            {
                return StatusCode(500, e);                
            }

        }

    }
}