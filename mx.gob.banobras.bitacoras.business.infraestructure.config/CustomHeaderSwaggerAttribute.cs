using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.config
{
    #region Methods
    public class CustomHeaderSwaggerAttribute : IOperationFilter
    {
        /// <summary>
        /// Customiza los headers que aparecerán en el Swagger
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "credentials",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                },
                Description = "Credenciales del usuario",
                Example = new OpenApiString("3FE000B477932C78E06CB6CC4BC1ECFB")
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "token-auth",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                },
                Description = "Token de Autenticación",
                Example = new OpenApiString("Bearer eyJ0eXAiOiJKV1Qi...")
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "app-name",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                },
                Description = "Nombre del sistema que consume el servicio",
                Example = new OpenApiString("SICOVI")
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "consumer-id",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                },
                Description = "Capa del sistema que consume el servicio",
                Example = new OpenApiString("UI SICOVI")
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "functional-id",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                },
                Description = "Funcionalidad que consume el servicio",
                Example = new OpenApiString("Login user")
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "transaction-id",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                },
                Description = "Identificador de la transacción, generado por UUID",
                Example = new OpenApiString("9680e51f-4766-4124-a3ff-02e9c3a5f9d6")
            });
        }
    }
    #endregion
}
