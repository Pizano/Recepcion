using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RcepcionApi.EntityModels
{
    public class LlamadaEntity
    {
        [Key]
        public int Id { get; set; }
        public string Mensaje { get; set; }
        public int TipoLlamadaEntityId { get; set; }
        public int TipoPersonaEntityId { get; set; }
        public virtual TipoLlamadaEntity TipoLlamadaEntity { get; set; }
        public virtual TipoPersonaEntity TipoPersonaEntity { get; set; }
    }
}
