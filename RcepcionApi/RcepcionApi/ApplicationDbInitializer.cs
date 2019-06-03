using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RcepcionApi.Data;
using RcepcionApi.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RcepcionApi
{
    public class ApplicationDbInitializer
    {
        public static void SeedUsers(UserManager<UsuarioEntity> userManager, DbContextOptions<RecepcionDbContext> dbContext)
        {


            using (var miDbContext = new RecepcionDbContext(dbContext))
            {
                if (miDbContext.TipoLlamadas.Count() <= 0)
                {
                    miDbContext.TipoLlamadas
                        .AddRange(
                        new List<TipoLlamadaEntity> {
                            new TipoLlamadaEntity{ Tipo = "Personal", FechaRegistro = DateTimeOffset.UtcNow},
                            new TipoLlamadaEntity{ Tipo = "AgendarJunta", FechaRegistro = DateTimeOffset.UtcNow},
                            new TipoLlamadaEntity{ Tipo = "Informacion", FechaRegistro = DateTimeOffset.UtcNow},
                            new TipoLlamadaEntity{ Tipo = "Proyecto", FechaRegistro = DateTimeOffset.UtcNow}
                        });

                }
                if (miDbContext.TipoPersona.Count() <= 0)
                {
                    miDbContext.TipoPersona
                        .AddRange(
                        new List<TipoPersonaEntity> {
                            new TipoPersonaEntity { Tipo = "Trabajador", FechaRegistro = DateTimeOffset.UtcNow },
                            new TipoPersonaEntity { Tipo = "Cliente", FechaRegistro = DateTimeOffset.UtcNow },
                            new TipoPersonaEntity { Tipo = "Persona", FechaRegistro = DateTimeOffset.UtcNow },
                        });
                }

                miDbContext.SaveChanges();
            }
            // Se genera el UsuarioAdministrador
            if (userManager.FindByEmailAsync("administrador@Mavi.com.mx").Result == null)
            {
                UsuarioEntity user = new UsuarioEntity
                {
                    UserName = "administrador@Mavi.com.mx",
                    Email = "administrador@Mavi.com.mx"
                };

                IdentityResult result = userManager.CreateAsync(user, "_okm265IJN").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrador").Wait();
                }
            }
        }
    }
}
