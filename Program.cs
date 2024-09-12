using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.application.inputport;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.application.outport;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.application.service;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.outp.client;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.config;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace banobras_bitacoras_business
{
    internal class Program
    {
        private Program()
        {
        }

        private static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo("log4net.xml"));

            var builder = WebApplication.CreateBuilder(args);

            //Se agrega validación CORS
            builder.Services.AddCors(policyBuilder =>
                policyBuilder.AddDefaultPolicy(policy =>
                    policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod())
            );
            /*builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ConfigureEndpointDefaults(listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                });
            });*/

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            // Duplicate here any configuration sources you use.
            configurationBuilder.AddJsonFile("appsettings.json");
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //Customiza el Swagger para agregar los headers
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Bitácora Centralizada", Version = "v1" });
                options.OperationFilter<CustomHeaderSwaggerAttribute>();
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            //Validaci�n por filtro de headers
            builder.Services.AddScoped<ValidacionHeaders>();

            //Realiza la inyecci�n de dependencias
            /*Implementaci�n de bit�coras de usuarios*/
            builder.Services.AddSingleton<IBitacoraUsuarioInputPort, BitacoraUsuarioServiceUseCase>();
            builder.Services.AddSingleton<IBitacoraUsuarioClientOutPort, BitacoraUsuarioClient>();
            /*Implementaci�n de bit�coras de accesos*/
            builder.Services.AddSingleton<IBitacoraAccesoInputPort, BitacoraAccesoServiceUseCase>();
            builder.Services.AddSingleton<IBitacoraAccesoClientOutPort, BitacoraAccesoClient>();
            /*Implementaci�n de bit�coras de accesos*/
            builder.Services.AddSingleton<IBitacoraOperacionInputPort, BitacoraOperacionServiceUseCase>();
            builder.Services.AddSingleton<IBitacoraOperacionClientOutPort, BitacoraOperacionClient>();
            /*Implementaci�n de bit�coras de catalogos*/
            builder.Services.AddSingleton<ICatalogoInputPort, CatalogoServiceUseCase>();
            builder.Services.AddSingleton<ICatalogoClientOutPort, CatalogoClient>();

            //Se realiza inyecci�n de dependencias de log
            builder.Services.AddScoped(factory => LogManager.GetLogger(typeof(Program)));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // if (app.Environment.IsDevelopment()) // T0DO: REGRESAR A ESTE VALOR, SE MODIFICÓ PAR APRUEBAS DE CICD
            if (app.Environment.IsProduction() || app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHsts();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}