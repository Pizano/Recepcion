using Microsoft.AspNetCore.Mvc;
using RcepcionApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RcepcionApi.Services
{
    public interface IPersonaServices
    {
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(int? id);
        Task<IActionResult> Create(PersonaViewModel model);
        Task<IActionResult> delete(int? id);
        Task<IActionResult> Update(PersonaViewModel model);

    }
}
