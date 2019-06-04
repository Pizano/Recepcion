using Microsoft.AspNetCore.Identity;
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
    public class PersonaServices : Controller, IPersonaServices
    {
        private readonly RecepcionDbContext _context;

        public PersonaServices(RecepcionDbContext context )
        {
            _context = context;
        }

        public async Task<IActionResult> Create(PersonaViewModel model)
        {
            PersonaEntity personaEntity = new PersonaEntity();
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, "Modelo no válido.");
                }
                personaEntity.Nombre = model.Nombre;
                personaEntity.Apellido = model.Apellido;
                personaEntity.TelefonoCelular = model.TelefonoCelular;
                personaEntity.NumeroFijo = model.NumeroFijo == null ? "Sin numero fijo alv" : model.NumeroFijo;
                personaEntity.Direccion = model.Direccion == null ? "Sin direccion Alv" : model.Direccion;
                personaEntity.TipoPersonaEntityId = model.TipoPersonaEntityId;
                await _context.Personas.AddAsync(personaEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetById", "Personas", new { id = personaEntity.Id });

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrio un error al crear la persona." + ex.Message);
            }
            finally
            {
                personaEntity = null;
                model = null;
            }
        }

        public async Task<IActionResult> GetAll()
        {
            List<PersonaEntity> personaEntity = await _context.Personas.ToListAsync();
            List<PersonaViewModel> personaViewModels = personaEntity.ConvertAll(x => new PersonaViewModel(x));
            return StatusCode(200, personaViewModels);

        }

        public async Task<IActionResult> GetById(int? id)
        {
            try
            {
                if (id == null)
                {
                    return StatusCode(400, "El identificador nullo.");
                }
                List<PersonaEntity> personaEntity = await _context.Personas.Where(x => x.Id.Equals(id)).ToListAsync();
                if (personaEntity == null || personaEntity.Count() == 0)
                {
                    return StatusCode(404, "Persona no encontrada.");
                }
                List<PersonaViewModel> personaViewModel = personaEntity.ConvertAll(x => new PersonaViewModel(x));
                PersonaViewModel personaViewModels = personaViewModel.FirstOrDefault();
                return StatusCode(200, personaViewModels);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }      
        }
        public async Task<IActionResult> delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return StatusCode(400, "El identificador nullo.");
                }
                PersonaEntity personaEntity = await _context.Personas.FindAsync(id);
                if (personaEntity == null)
                {
                    return StatusCode(404, "Persona no encontrada.");
                }
                _context.Personas.Remove(personaEntity);
                await _context.SaveChangesAsync();
                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        public async Task<IActionResult> Update(PersonaViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, "Modelo no válido.");
                }
                PersonaEntity personaEntity = await _context.Personas.FindAsync(model.Id);
                if (personaEntity == null)
                {
                    return StatusCode(404, "Persona no encontrada.");
                }
                personaEntity.Nombre = model.Nombre == null ? personaEntity.Nombre : model.Nombre;
                personaEntity.Apellido = model.Apellido == null ? personaEntity.Apellido : model.Apellido;
                personaEntity.TelefonoCelular = model.TelefonoCelular == null ? personaEntity.TelefonoCelular : model.TelefonoCelular;
                personaEntity.NumeroFijo = model.NumeroFijo == null ? personaEntity.NumeroFijo : model.NumeroFijo;
                personaEntity.Direccion = model.Direccion == null ? personaEntity.Direccion : model.Direccion;
                personaEntity.TipoPersonaEntityId = model.TipoPersonaEntityId == 0 ? personaEntity.TipoPersonaEntityId : model.TipoPersonaEntityId;

                _context.Personas.Update(personaEntity);
                await _context.SaveChangesAsync();
                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}
