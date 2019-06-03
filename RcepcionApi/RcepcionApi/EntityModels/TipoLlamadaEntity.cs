using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RcepcionApi.EntityModels
{
    public class TipoLlamadaEntity
    {
        [Key]
        public int Id { get; set; }
        public string Tipo { get; set; }
        public DateTimeOffset FechaRegistro { get; set; }
        public virtual List<LlamadaEntity> LlamadaEntities { get; set; }

    }
}
