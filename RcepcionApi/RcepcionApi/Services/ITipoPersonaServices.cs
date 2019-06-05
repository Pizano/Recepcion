using Microsoft.AspNetCore.Mvc;
using RcepcionApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RcepcionApi.Services
{
    public interface ITipoPersonaServices
    {
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(int? id);
        Task<IActionResult> Create(TipoPersonaViewModel model);
        Task<IActionResult> Update(TipoPersonaViewModel model);
        Task<IActionResult> Delete(int? id);
    }
}
