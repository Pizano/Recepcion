using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RcepcionApi.EntityModels;
using RcepcionApi.Models;

namespace RcepcionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroController : ControllerBase
    {
        private readonly UserManager<UsuarioEntity> _userManager;
        private readonly SignInManager<UsuarioEntity> _signInManager;

        public RegistroController(UserManager<UsuarioEntity> userManager, SignInManager<UsuarioEntity> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // POST api/<controller>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new UsuarioEntity
                {
                    UserName = model.Correo,
                    Email = model.Correo,
                    Nombres = model.Nombres,
                    Apellidos = model.Apellidos,
                    Telefono = model.Telefono,
                    FechaRegistro = DateTimeOffset.UtcNow
                };
                try
                {
                    var result = await _userManager.CreateAsync(user, model.Contrasena);
                    if (result.Succeeded)
                    {
                        // Se asigna Role de Usuario
                        _userManager.AddToRoleAsync(user, "Usuario").Wait();
                        // Para obtener más información sobre cómo habilitar la confirmación de cuenta y el restablecimiento de contraseña, visite http://go.microsoft.com/fwlink/?LinkID=320771
                        // Enviar correo electrónico con este vínculo
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // await UserManager.SendEmailAsync(user.Id, "Confirmar cuenta", "Para confirmar la cuenta, haga clic <a href=\"" + callbackUrl + "\">aquí</a>");
                        return Ok("Usuario se registro correctamente");
                    }
                    else
                    {
                        List<IdentityError> errores = new List<IdentityError>();
                        if (result.Errors != null)
                        {
                            foreach (var error in result.Errors)
                            {
                                errores.Add(error);
                            }
                            return BadRequest("Fallo registro,  " + errores[0].Code + ", " + errores[0].Description);
                        }
                        return BadRequest("Fallo el registro");
                    }
                }
                catch
                {
                    return BadRequest("Fallo al intentar grabar el registro.");
                }
            }
            else
            {
                return BadRequest("El modelo no es valido");

            }
        }
    }
}