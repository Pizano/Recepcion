using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation;
using RcepcionApi.Data;
using RcepcionApi.Models;
using RcepcionApi.Services;

namespace RcepcionApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
    [ApiController]
    public class LlamadasController : ControllerBase
    {
        private readonly RecepcionDbContext _context;
        private readonly ILlamadasServices _services;

        public LlamadasController(RecepcionDbContext context, ILlamadasServices services)
        {
            _context = context;
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            return await _services.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int? id)
        {
            return await _services.GetById(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LlamadaViewModel model) {
            return await _services.Create(model);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            return await _services.Delete(id);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LlamadaViewModel model)
        {
            return await _services.Update(model);
        }
    }
}