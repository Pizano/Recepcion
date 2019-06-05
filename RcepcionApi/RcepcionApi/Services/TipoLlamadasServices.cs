using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RcepcionApi.Data;
using RcepcionApi.EntityModels;
using RcepcionApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RcepcionApi.Services
{
    public class TipoLlamadasServices : Controller, ITipoLlamadasServices
    {
        private readonly RecepcionDbContext _context;

        public TipoLlamadasServices(RecepcionDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create(TipoLLamadasViewModel model)
        {
            TipoLlamadaEntity tipoLlamadaEntity = new TipoLlamadaEntity();
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400,"Modelo no válido");
                }
                tipoLlamadaEntity.FechaRegistro = DateTime.UtcNow;
                tipoLlamadaEntity.Tipo = model.Tipo;
                await _context.TipoLlamadas.AddAsync(tipoLlamadaEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetById","TipoLlamadas", new { id = tipoLlamadaEntity.Id});
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex);
            }
            finally
            {
                tipoLlamadaEntity = null;
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return StatusCode(400, "Identificador nulo.");
                }
                TipoLlamadaEntity tipoLlamadaEntity = await _context.TipoLlamadas.FindAsync(id);
                if (tipoLlamadaEntity == null)
                {
                    return StatusCode(404, "Tipo de llamada no encontrada");
                }
                _context.TipoLlamadas.Remove(tipoLlamadaEntity);
                await _context.SaveChangesAsync();
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex);
            }
        }

        public async Task<IActionResult> GetAll()
        {
            List<TipoLlamadaEntity> tipoLlamadaEntities = await _context.TipoLlamadas.ToListAsync();
            List<TipoLLamadasViewModel> tipoLLamadasViewModels = tipoLlamadaEntities.ConvertAll(x => new TipoLLamadasViewModel(x));
            return StatusCode(200, tipoLLamadasViewModels);
        }

        public async Task<IActionResult> GetById(int? id)
        {
            try
            {
                if (id == null)
                {
                    return StatusCode(400, "Identificador nulo");
                }
                List<TipoLlamadaEntity> tipoLlamadaEntity = await _context.TipoLlamadas.Where(x => x.Id.Equals(id)).ToListAsync();
                if (tipoLlamadaEntity == null || tipoLlamadaEntity.Count().Equals(0))
                {
                    return StatusCode(404, "No se encontro el tipo de llamada");
                }
                List<TipoLLamadasViewModel> tipoLLamadasViewModels = tipoLlamadaEntity.ConvertAll(x => new TipoLLamadasViewModel(x));
                TipoLLamadasViewModel tipoLLamadasViewModel = tipoLLamadasViewModels.FirstOrDefault();
                return StatusCode(200,tipoLLamadasViewModel);

            }
            catch (Exception ex)
            {
                return StatusCode(500,ex);
            }
        }

        public async Task<IActionResult> Update(TipoLLamadasViewModel model)
        {
            TipoLlamadaEntity tipoLlamadaEntity = new TipoLlamadaEntity();
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400,"Modelo no válido");
                }
                tipoLlamadaEntity = await _context.TipoLlamadas.FindAsync(model.Id);
                if (tipoLlamadaEntity == null)
                {
                    return StatusCode(404, "Tipo de llamada no encontrada.");
                }
                tipoLlamadaEntity.Tipo = model.Tipo == null || model.Tipo == "" ? tipoLlamadaEntity.Tipo : model.Tipo;
                tipoLlamadaEntity.FechaRegistro = DateTime.UtcNow;
                _context.Update(tipoLlamadaEntity);
                await _context.SaveChangesAsync();
                return StatusCode(200);

            }
            catch (Exception ex)
            {
                return StatusCode(500,ex);
            }
        }
    }
}
