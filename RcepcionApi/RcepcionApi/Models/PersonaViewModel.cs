using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using RcepcionApi.EntityModels;

namespace RcepcionApi.Models
{
    public class PersonaViewModel
    {
        public PersonaViewModel()
        {
        }

        public PersonaViewModel(PersonaEntity x)
        {
            this.Id = x.Id;
            this.Nombre = x.Nombre;
            this.Apellido = x.Apellido;
            this.Direccion = x.Direccion;
            this.NumeroFijo = x.NumeroFijo;
            this.TelefonoCelular = x.TelefonoCelular;
            this.TipoPersonaEntityId = x.TipoPersonaEntityId;
            
        }

        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string TelefonoCelular { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string NumeroFijo { get; set; }
        public string Direccion { get; set; }
        [Required]
        public int TipoPersonaEntityId { get; set; }
    }
}
