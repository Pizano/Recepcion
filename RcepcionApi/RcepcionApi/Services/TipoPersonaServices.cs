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
    public class TipoPersonaServices : Controller, ITipoPersonaServices
    {
        private readonly RecepcionDbContext _context;

        public TipoPersonaServices(RecepcionDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Create(TipoPersonaViewModel model)
        {
            TipoPersonaEntity tipoPersonaEntity = new TipoPersonaEntity();
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(404, "Modelo no válido");
                }
                tipoPersonaEntity.Tipo = model.Tipo;
                tipoPersonaEntity.FechaRegistro = DateTime.UtcNow;
                await _context.TipoPersona.AddAsync(tipoPersonaEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetById","TipoPersona", new { id = tipoPersonaEntity.Id});

            }
            catch (Exception ex)
            {
                return StatusCode(500,ex);
            }
            finally
            {
                tipoPersonaEntity = null;
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            TipoPersonaEntity tipoPersonaEntity = new TipoPersonaEntity();
            try
            {
                if (id == null)
                {
                    return StatusCode(400,"Identificador nulo");
                }
                tipoPersonaEntity = await _context.TipoPersona.FindAsync(id);
                if (tipoPersonaEntity == null)
                {
                    return StatusCode(404,"El tipo de persona no se encuentra");
                }
                _context.TipoPersona.Remove(tipoPersonaEntity);
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
            List<TipoPersonaEntity> tipoPersonaEntities = await _context.TipoPersona.ToListAsync();
            List<TipoPersonaViewModel> tipoPersonaViewModels = tipoPersonaEntities.ConvertAll(x => new TipoPersonaViewModel(x));
            return StatusCode(200,tipoPersonaViewModels);
        }

        public async Task<IActionResult> GetById(int? id)
        {
            List<TipoPersonaViewModel> tipoPersonaViewModels = new List<TipoPersonaViewModel>();
            TipoPersonaViewModel tipoPersonaViewModel = new TipoPersonaViewModel();
            List<TipoPersonaEntity> tipoPersonaEntities = new List<TipoPersonaEntity>();
            try
            {
                if (id == null)
                {
                    return StatusCode(400,"Identificador nulo");
                }
                tipoPersonaEntities = await _context.TipoPersona.Where(x => x.Id.Equals(id)).ToListAsync();
                if (tipoPersonaEntities.Count() == 0)
                {
                    return StatusCode(404,"Tipo de persona no encontrada");
                }
                tipoPersonaViewModels = tipoPersonaEntities.ConvertAll(x => new TipoPersonaViewModel(x));
                tipoPersonaViewModel = tipoPersonaViewModels.FirstOrDefault();
                return StatusCode(200, tipoPersonaViewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex);
            }
            finally
            {
                tipoPersonaEntities = null;
                tipoPersonaViewModels = null;
                tipoPersonaViewModel = null;
            }
        }

        public async Task<IActionResult> Update(TipoPersonaViewModel model)
        {
            TipoPersonaEntity tipoPersonaEntity = new TipoPersonaEntity();
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, "Modelo no válido");
                }
                tipoPersonaEntity = await _context.TipoPersona.FindAsync(model.Id);
                if (tipoPersonaEntity == null)
                {
                    return StatusCode(404, "Tipo de persona no encontrada");
                }
                tipoPersonaEntity.Tipo = model.Tipo;
                tipoPersonaEntity.FechaRegistro = DateTime.UtcNow;
                _context.TipoPersona.Update(tipoPersonaEntity);
                await _context.SaveChangesAsync();
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex);
            }
            finally
            {
                tipoPersonaEntity = null;
            }
        }
    }
}
