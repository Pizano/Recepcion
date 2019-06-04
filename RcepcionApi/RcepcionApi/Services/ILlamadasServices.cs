using Microsoft.AspNetCore.Mvc;
using RcepcionApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RcepcionApi.Services
{
    public interface ILlamadasServices
    {
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(int? id);
        Task<IActionResult> Create(LlamadaViewModel model);
        Task<IActionResult> Update(LlamadaViewModel model);
        Task<IActionResult> Delete(int? id);
    }
}
