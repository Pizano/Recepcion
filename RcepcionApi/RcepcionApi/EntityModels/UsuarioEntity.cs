using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RcepcionApi.EntityModels
{
    public class UsuarioEntity : IdentityUser
    {
        // Datos del registro
        public DateTimeOffset FechaRegistro { get; set; }
        [Required]
        [StringLength(80)]
        public string Nombres { get; set; }
        [Required]
        [StringLength(80)]
        public string Apellidos { get; set; }
        [StringLength(12)]
        public string Telefono { get; set; }
    }
}
