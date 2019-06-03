using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenIddict.Validation;
using RcepcionApi.Data;
using RcepcionApi.EntityModels;

namespace RcepcionApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Contexto de la base de datos
            services.AddDbContext<RecepcionDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("MaviConnection"))
                .EnableSensitiveDataLogging(true).UseLazyLoadingProxies();
                options.UseOpenIddict();
            });



            // Add OpenIddict services
            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<RecepcionDbContext>();
                })
                .AddServer(options =>
                {
                    options.UseMvc();

                    options.EnableTokenEndpoint("/api/token");
                    options.SetAccessTokenLifetime(TimeSpan.FromDays(720));
                    options.SetRefreshTokenLifetime(TimeSpan.FromDays(720));
                    options.AllowPasswordFlow().AllowRefreshTokenFlow();
                    options.AcceptAnonymousClients();
                })
                .AddValidation();

            // ASP.NET Core Identity should use the same claim names as OpenIddict
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                //options.Password.RequiredUniqueChars = 1;

            });

            // Schhema de Autenticación
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OpenIddictValidationDefaults.AuthenticationScheme;
            });

            services.AddDefaultIdentity<UsuarioEntity>()
                .AddRoles<UsuarioRoleEntity>()
                .AddEntityFrameworkStores<RecepcionDbContext>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_0);
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });

            services.AddCors();

            // Swagger configuración
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Trascender API",
                    Description = "Pruebas documentación Swagger"
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
            // Swagger Configuracion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<UsuarioEntity> userManager, DbContextOptions<RecepcionDbContext> dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors(builder =>
                // This will allow any request from any server. Tweak to fit your needs!
                // The fluent API is pretty pleasant to work with.
                //builder.WithOrigins("https://localhost:5001", "http://localhost:5000", "https://localhost:44300")
                //.AllowAnyHeader()

                builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
            );
            // Se hace el Seed del usuario administrador
            ApplicationDbInitializer.SeedUsers(userManager, dbContext);
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
            // app de swagger
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Trascender");
            });
            // app de swager
        }
    }
}
