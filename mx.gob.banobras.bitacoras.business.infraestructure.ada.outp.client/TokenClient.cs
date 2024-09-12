using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.inp.dto;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.config;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.persistence.dominio.model;
using log4net;
using Newtonsoft.Json;

namespace banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.outp.client
{
    public class TokenClient
    {
        #region Properties
        /// <summary>
        /// Instancia de la interfaz de archivo de configuración appsettings.json
        /// </summary>
        readonly IConfiguration configuration;
        /// <summary>
        /// Instancia de la interfaz de logueo
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(BitacoraAccesoClient));
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la implementación cliente puerto de salida que recibe la inyección de dependencias del archivo de configuración de Program.cs
        /// </summary>
        /// <param name="_configuration"></param>
        public TokenClient(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Método que invoca el API Tokenizer para la validación del Bearer Token
        /// a través de los headers que recibe
        /// </summary>
        /// Ejemplo de headers:
        /// credentials:20043908CE821A09B1370B68267D25AB
        /// token-auth:Bearer eyJ0eXA.....-WGqbu8I4w
        /// app-name:sicovi
        /// consumer-id:banobras-nameapplication-backend
        /// functional-id:validar-usuario
        /// transaction-id:1a23d232-0def-4349-9ecc-288f53fab742
        /// Ejemplo de response:
        /// {
        ///     "statusCode": 401,
        ///     "tokenDTO": null,
        ///     "errorMessageDTO": {
        ///         "statusCode": 401,
        ///          "timestamp": "2024-07-12T23:43:14.778+00:00",
        ///          "message": "Token expirado."
        ///     }
        /// }
        /// 200: Exito
        /// 401: Token expirado (Usuario no autorizado)
        /// 500: Error interno en el servidor
        /// <returns></returns>
        public async Task<ResponseApiTokenizer> validarToken(string estatusOperacion = "")
        {
            _log.Info("TokenClient: MÉTODO consultar");
            ResponseApiTokenizer response = new ResponseApiTokenizer();
            string message1 = string.Format("TokenClient: VALIDAR_API_TOKENIZER {0}", configuration.GetSection("VALIDAR_API_TOKENIZER").Value);
            _log.Info(message1);
            string messageToken = string.Empty;
            bool validarToken = configuration.GetSection("VALIDAR_API_TOKENIZER").Value != null && bool.Parse(configuration.GetSection("VALIDAR_API_TOKENIZER").Value!);
            if (estatusOperacion.ToUpper() == "I" && ResourceUtils.HEADERS.SingleOrDefault(c => c.Key == "functional-id").Value.ToUpper().Contains("LOGIN"))
            {
                validarToken = false;
                messageToken = string.Format("TokenClient: SE SALTÓ LA VALIDACIÓN DEL API TOKENIZER - functional-id={0} y estatusOperacion={1}", ResourceUtils.HEADERS.SingleOrDefault(c => c.Key == "functional-id").Value.ToUpper(), estatusOperacion);
            }
            if (validarToken)
            {
                try
                {
                    if (ResourceUtils.HEADERS.SingleOrDefault(c => c.Key == "app-name").Value.ToUpper().Contains("DEMOHTTPS"))
                    {
                        var result = await new GenericRestService(configuration).CallServiceWithCertificateV2(string.Empty,
                                        new ResponseApiTokenizer(),
                                        "https://banobras-api-tokenizer-common-apps-develop.banobras.gob.mx/",
                                        "tokenizer/v1/valid",
                                        EVerbType.POST,
                                        ResourceUtils.HEADERS);
                        string message2 = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
                        if (result != null)
                        {
                            if (result!.Model != null)
                                response = result.Model!;
                            else
                            {
                                string messageX = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result.Model, Formatting.Indented));
                                response = new ResponseApiTokenizer { errorMessageDTO = new ErrorMessageDto { message = message2 + "/" + messageX } };
                            }
                        }
                        else
                        {
                            if (result!.Model != null)
                                response = result.Model!;
                            else
                                response = new ResponseApiTokenizer { errorMessageDTO = new ErrorMessageDto { message = message2 } };
                        }

                        _log.Info(message2);
                        Console.WriteLine(message2);
                    }
                    else
                    {
                        var result = await new GenericRestService(configuration).CallServiceWithCertificate(string.Empty,
                                       new ResponseApiTokenizer(),
                                       configuration.GetSection("URL_API_TOKENIZER").Value!,
                                       "tokenizer/v1/valid",
                                       EVerbType.POST,
                                       ResourceUtils.HEADERS);
                        response = result.Model!;
                        string message2 = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
                        _log.Info(message2);
                        Console.WriteLine(message2);
                    }
                }
                catch (Exception ex)
                {
                    response = new ResponseApiTokenizer { statusCode = 500 };
                    _log.Error("EXCEPCIÓN:\n", ex);
                    Console.WriteLine("EXCEPCIÓN:\n" + ex);
                }
            }
            else
            {
                messageToken = string.IsNullOrEmpty(messageToken) ? string.Format("TokenClient: SE SALTÓ LA VALIDACIÓN DEL API TOKENIZER - appsettings.json->VALIDAR_API_TOKENIZER={0}", configuration.GetSection("VALIDAR_API_TOKENIZER").Value) : messageToken;
                response = new ResponseApiTokenizer { statusCode = 200 };
            }
            _log.Info(messageToken);
            Console.WriteLine(messageToken);
            return await Task.FromResult(response);
        }
        #endregion
    }
}
