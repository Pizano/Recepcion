using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RcepcionApi.EntityModels
{
    public class PersonaEntity
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string TelefonoCelular { get; set; }
        public string NumeroFijo { get; set; }
        public string Direccion { get; set; }
        public int TipoPersonaEntityId { get; set; }
        public virtual TipoPersonaEntity TipoPersonaEntity { get; set; }
    }
}
