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
    public class LlamadaServices : Controller, ILlamadasServices
    {
        private readonly RecepcionDbContext _context;

        public LlamadaServices(RecepcionDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create(LlamadaViewModel model)
        {
            LlamadaEntity llamadaEntity = new LlamadaEntity();
            try
            {
                if (!ModelState.IsValid) {
                    return StatusCode(400, "Modelo no válido.");
                }
                llamadaEntity.Mensaje = model.Mensaje;
                llamadaEntity.TipoLlamadaEntityId = model.TipoLlamadaEntityId;
                llamadaEntity.TipoPersonaEntityId = model.TipoPersonaEntityId;
                await _context.Llamadas.AddAsync(llamadaEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetById","Llamadas", new { id = llamadaEntity.Id });

            }
            catch (Exception ex)
            {
                return StatusCode(500,ex);
            }finally
            {
                llamadaEntity = null;
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null) {
                    return StatusCode(400, "Identificador nullo");
                }
                LlamadaEntity llamadaEntity = await _context.Llamadas.FindAsync(id);
                if (llamadaEntity == null) {
                    return StatusCode(404, "No se encontro la llamada");
                }
                _context.Llamadas.Remove(llamadaEntity);
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
            List<LlamadaViewModel> llamadaViewModel = new List<LlamadaViewModel>();
            try
            {
                List<LlamadaEntity> llamadaEntities = await _context.Llamadas.ToListAsync();
                llamadaViewModel = llamadaEntities.ConvertAll(x => new LlamadaViewModel(x));
                return StatusCode(200, llamadaViewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            finally
            {
                llamadaViewModel = null;
            }
        }

        public async Task<IActionResult> GetById(int? id)
        {
            LlamadaViewModel llamadaViewModel = new LlamadaViewModel();
            try
            {
                if (id == null)
                {
                    return StatusCode(400, "Identificador nullo.");
                }
                List<LlamadaEntity> llamadaEntity = await _context.Llamadas.Where(x => x.Id.Equals(id)).ToListAsync();
                if (llamadaEntity == null || llamadaEntity.Count() == 0) {
                    return StatusCode(404,"No se encontro la llamada");
                }
                List<LlamadaViewModel> llamadaViewModels = llamadaEntity.ConvertAll(x => new LlamadaViewModel(x));
                llamadaViewModel = llamadaViewModels.FirstOrDefault();
                return StatusCode(200,llamadaViewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex);
            }
            finally{
                llamadaViewModel = null;
            }
        }

        public async Task<IActionResult> Update(LlamadaViewModel model)
        {

            try
            {
                if (!ModelState.IsValid) {
                    return StatusCode(400, "Modelo no válido.");
                }
                LlamadaEntity llamadaEntity = await _context.Llamadas.FindAsync(model.Id);
                if (llamadaEntity == null) {
                    return StatusCode(404, "Llamada no existe.");
                }
                llamadaEntity.Mensaje = model.Mensaje == null || model.Mensaje == "" ? llamadaEntity.Mensaje : model.Mensaje;
                llamadaEntity.TipoLlamadaEntityId = model.TipoLlamadaEntityId == 0 ? llamadaEntity.TipoLlamadaEntityId : model.TipoLlamadaEntityId;
                llamadaEntity.TipoPersonaEntityId = model.TipoPersonaEntityId == 0 ? llamadaEntity.TipoPersonaEntityId : model.TipoPersonaEntityId;
                _context.Llamadas.Update(llamadaEntity);
                await _context.SaveChangesAsync();
                return StatusCode(200);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
