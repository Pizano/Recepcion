using Microsoft.AspNetCore.Mvc;
using RcepcionApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RcepcionApi.Services
{
    public interface ITipoLlamadasServices
    {
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(int? id);
        Task<IActionResult> Create(TipoLLamadasViewModel model);
        Task<IActionResult> Update(TipoLLamadasViewModel model);
        Task<IActionResult> Delete(int? id);
    }
}
