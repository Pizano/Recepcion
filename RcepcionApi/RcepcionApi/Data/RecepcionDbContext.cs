using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RcepcionApi.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RcepcionApi.Data
{
    public class RecepcionDbContext : IdentityDbContext
    {
        public RecepcionDbContext(DbContextOptions<RecepcionDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Role Adinistrador
            modelBuilder.Entity<UsuarioRoleEntity>().HasData(new UsuarioRoleEntity { Name = "Admininistrador", NormalizedName = "ADMINISTRADOR".ToUpper() });
            // Role Usurio
            modelBuilder.Entity<UsuarioRoleEntity>().HasData(new UsuarioRoleEntity { Name = "Usuario", NormalizedName = "USUARIO".ToUpper() });
        }

        public DbSet<LlamadaEntity> Llamadas { get; set; }
        public DbSet<PersonaEntity> Personas { get; set; }
        public DbSet<TipoLlamadaEntity> TipoLlamadas { get; set; }
        public DbSet<TipoPersonaEntity> TipoPersona { get; set; }

        public DbSet<UsuarioEntity> Usuarios { get; set; }

    }
}
