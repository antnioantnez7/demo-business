using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.dominio.model;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.config
{
    public class ValidacionHeaders : IActionFilter
    {
        #region Properties
        /// <summary>
        /// Instancia de la interfaz de logueo
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(ValidacionHeaders));
        /// <summary>
        /// Lista para almancenar los headers obligatorios
        /// </summary>
        private List<string> headersObligatorios = new List<string>();
        #endregion

        #region Methods
        /// <summary>
        /// Implementación del método de IActionFilter
        /// <remarks>Se ejecuta cuando se termina de ejecutar el filtro para validar los headers</remarks>
        /// <remarks>La clase ValidacionHeaders debe colocarse en el Controller a utilizar</remarks>
        /// <remarks>De la siguiente manera: [ServiceFilter(typeof(ValidacionHeaders))] </remarks>
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            string message1 = string.Format("VALIDACIÓN DE HEADERS COMPLETADA - :\n {0}", "ValidacionHeaders: OnActionExecuted");
            _log.Info(message1);
            Dictionary<string, string> listHeaders = new Dictionary<string, string>();
            var request = context.HttpContext.Request;
            foreach (var header in request.Headers)
            {
                listHeaders.Add(header.Key, header.Value.ToString());
            }
            string message2 = string.Format("HEADERS RECIBIDOS:\n {0}", JsonConvert.SerializeObject(listHeaders, Formatting.Indented));
            _log.Info(message2);
            string message3 = string.Format("HEADERS ALMACENADOS:\n {0}", JsonConvert.SerializeObject(ResourceUtils.HEADERS, Formatting.Indented));
            _log.Info(message3);
        }

        /// <summary>
        /// Implementación del método de IActionFilter
        /// <remarks>Se ejecuta cuando se invoca un Microservicio, y sirve como filtro para validar los headers</remarks>
        /// <remarks>La clase ValidacionHeaders debe colocarse en el Controller a utilizar</remarks>
        /// <remarks>De la siguiente manera: [ServiceFilter(typeof(ValidacionHeaders))] </remarks>
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            headersObligatorios = new List<string> { "credentials", "token-auth", "app-name", "consumer-id", "functional-id", "transaction-id" };
            ResourceUtils.HEADERS.Clear();
            string message1 = string.Format("VALIDACIÓN DE HEADERS EJECUTANDOSE - :\n {0}", "ValidacionHeaders: OnActionExecuting");
            _log.Info(message1);
            int headersObligatoriosFaltantes = headersObligatorios.Count;
            string mensaje = string.Empty;
            var request = context.HttpContext.Request;
            foreach (var header in request.Headers)
            {

                switch (header.Key.ToLower())
                {
                    case "credentials":
                    case "token-auth":
                    case "app-name":
                    case "consumer-id":
                    case "functional-id":
                    case "transaction-id":
                        headersObligatoriosFaltantes--;
                        ResourceUtils.HEADERS.Add(header.Key, header.Value.ToString());
                        headersObligatorios.RemoveAll(r => r.Equals(header.Key, StringComparison.CurrentCultureIgnoreCase));
                        break;
                    default:
                        break;
                }
            }
            if (headersObligatoriosFaltantes > 0)
            {
                mensaje = headersObligatorios[0];
                var resopnse = new ResponseBaseMicroservicio<object>() { statusCode = 400, message = string.Format("El header '{0}' es obligatorio.", mensaje), responseType = ResponseType.BadRequest };
                context.Result = new BadRequestObjectResult(resopnse);
            }
        }
        #endregion
    }
}
