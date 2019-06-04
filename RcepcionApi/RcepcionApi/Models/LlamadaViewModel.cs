using RcepcionApi.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RcepcionApi.Models
{
    public class LlamadaViewModel
    {

        public LlamadaViewModel()
        {
        }

        public LlamadaViewModel(LlamadaEntity x)
        {
            this.Id = x.Id;
            this.Mensaje = x.Mensaje;
            this.TipoLlamadaEntityId = x.TipoLlamadaEntityId;
            this.TipoPersonaEntityId = x.TipoPersonaEntityId;

        }

        public int Id { get; set; }
        public string Mensaje { get; set; }
        [Required]
        public int TipoLlamadaEntityId { get; set; }
        [Required]
        public int TipoPersonaEntityId { get; set; }

    }
}
