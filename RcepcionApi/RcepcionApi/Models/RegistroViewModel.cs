using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RcepcionApi.Models
{
    public class RegistroViewModel
    {
        [Required]
        [StringLength(80)]
        public string Nombres { get; set; }

        [Required]
        [StringLength(80)]
        public string Apellidos { get; set; }

        [StringLength(12)]
        public string Telefono { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(80)]
        public string Correo { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }
    }
}
