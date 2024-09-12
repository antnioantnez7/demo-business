using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.application.outport;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.dominio.model;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.inp.dto;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.config;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.persistence.dominio.model;
using log4net;
using Newtonsoft.Json;

namespace banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.outp.client
{
    public class BitacoraAccesoClient: IBitacoraAccesoClientOutPort
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
        public BitacoraAccesoClient(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Implementación del método 'consultar', se realiza la conexión a la capa de persistencia
        /// </summary>
        /// <param name="bitacoraConsultaDTO"></param>
        /// <returns></returns>
        public async Task<BitacoraResponse<List<BitacoraAccesoDto>>> consultar(BitacoraConsultaDto bitacoraConsultaDTO)
        {
            var response = new BitacoraResponse<List<BitacoraAccesoDto>>();
            ResponseApiTokenizer token = new TokenClient(configuration).validarToken().Result;
            string message = string.Format("RESPONSE API TOKENIZER:\n {0}", JsonConvert.SerializeObject(token, Formatting.Indented));
            Console.WriteLine(message);
            if (token != null && token.statusCode == 200){
                if (ResourceUtils.HEADERS.SingleOrDefault(c => c.Key == "consumer-id").Value.ToUpper().Contains("DEMOHTTPS"))
                {
                    Console.WriteLine("BitacoraAccesoClient: MÉTODO consultar");
                    Console.WriteLine(string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(bitacoraConsultaDTO, Formatting.Indented)));
                    _log.Info("BitacoraAccesoClient: MÉTODO consultar");
                    string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(bitacoraConsultaDTO, Formatting.Indented));
                    _log.Info(message1);
                    var result = await new GenericRestService(configuration).CallServiceWithCertificateV2(bitacoraConsultaDTO,
                                    new ResponseBaseMicroservicio<List<BitacoraAccesoDto>>(),
                                    "https://backend-persistence-centralized-logs-develop.banobras.gob.mx/",
                                    "persistence/accesos/consultar",
                                    EVerbType.POST,
                                    new Dictionary<string, string>());
                    Console.WriteLine(string.Format("RESULT:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented)));
                    string message2 = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
                    _log.Info(message2);
                    if (result != null)
                    {
                            string messageX = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
                            response = new BitacoraResponse<List<BitacoraAccesoDto>> { Codigo = result.Model != null ? result.Model.statusCode : 500, Contenido = result.Model != null ? result.Model.Contenido : new List<BitacoraAccesoDto> { }, Mensaje = result.Model?.message! +"/"+ messageX };
                        
                    }
                    else
                    {
                            response = new BitacoraResponse<List<BitacoraAccesoDto>> { Codigo = 500, Contenido = new List<BitacoraAccesoDto> { }, Mensaje = "Result es nulo" };
                    }
                    
                    Console.WriteLine(string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(response, Formatting.Indented)));
                }
                else
                {
                    Console.WriteLine("BitacoraAccesoClient: MÉTODO consultar");
                    Console.WriteLine(string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(bitacoraConsultaDTO, Formatting.Indented)));
                    _log.Info("BitacoraAccesoClient: MÉTODO consultar");
                    string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(bitacoraConsultaDTO, Formatting.Indented));
                    _log.Info(message1);
                    var result = await new GenericRestService(configuration).CallServiceWithCertificate(bitacoraConsultaDTO,
                                    new ResponseBaseMicroservicio<List<BitacoraAccesoDto>>(),
                                    configuration.GetSection("URL_USUARIO_PERSISTENCE").Value!,
                                    "persistence/accesos/consultar",
                                    EVerbType.POST,
                                    new Dictionary<string, string>());
                    Console.WriteLine(string.Format("RESULT:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented)));
                    string message2 = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
                    _log.Info(message2);
                    response = new BitacoraResponse<List<BitacoraAccesoDto>> { Codigo = result.Model != null ? result.Model.statusCode : 500, Contenido = result.Model != null ? result.Model.Contenido : new List<BitacoraAccesoDto> { }, Mensaje = result.Model?.message! };
                    Console.WriteLine(string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(response, Formatting.Indented)));
                }
            }
            else
            {
                if (token != null)
                {
                    if (token.errorMessageDTO != null)
                    {
                        response = new BitacoraResponse<List<BitacoraAccesoDto>> { Codigo = token.statusCode, Contenido = new List<BitacoraAccesoDto> { }, Mensaje = token.errorMessageDTO.message! };
                    }
                    else
                    {
                        response = new BitacoraResponse<List<BitacoraAccesoDto>> { Codigo = token.statusCode, Contenido = new List<BitacoraAccesoDto> { }, Mensaje = "Error al consumir el servicio del API Tokenizer." };
                    }
                }
                else
                    response = new BitacoraResponse<List<BitacoraAccesoDto>> { Codigo = 500, Contenido = new List<BitacoraAccesoDto> { }, Mensaje = "Error al consumir el servicio del API Tokenizer. Respuesta nula." };
            }
            return response;
        }

        /// <summary>
        /// Implementación del método 'registrar', se realiza la conexión a la capa de persistencia
        /// </summary>
        /// <param name="bitacoraAccesoDTO"></param>
        /// <returns></returns>
        public async Task<BitacoraResponse<BitacoraDtoResponse>> registrar(BitacoraAccesoDto bitacoraAccesoDTO)
        {
            var response = new BitacoraResponse<BitacoraDtoResponse>();
            ResponseApiTokenizer token = new TokenClient(configuration).validarToken(bitacoraAccesoDTO.estatusOperacion).Result;
            string message = string.Format("RESPONSE API TOKENIZER:\n {0}", JsonConvert.SerializeObject(token, Formatting.Indented));
            Console.WriteLine(message);
            if (token != null && token.statusCode == 200){
                _log.Info("BitacoraAccesoClient: MÉTODO registrar");
                string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(bitacoraAccesoDTO, Formatting.Indented));
                _log.Info(message1);
                var result = await new GenericRestService(configuration).CallServiceWithCertificate(bitacoraAccesoDTO,
                                new ResponseBaseMicroservicio<BitacoraDtoResponse>(),
                                configuration.GetSection("URL_USUARIO_PERSISTENCE").Value!,
                                "persistence/accesos/registrar",
                                EVerbType.POST,
                                new Dictionary<string, string>());
                string message2 = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
                _log.Info(message2);
                response = new BitacoraResponse<BitacoraDtoResponse> { Codigo = result.Model != null ? result.Model.statusCode : 500, Contenido = result.Model != null ? result.Model.Contenido : new BitacoraDtoResponse { }, Mensaje = result.Model?.message! };
            }
            else
            {
                if (token != null)
                {
                    if (token.errorMessageDTO != null)
                    {
                        response = new BitacoraResponse<BitacoraDtoResponse> { Codigo = token.statusCode, Contenido = new BitacoraDtoResponse { }, Mensaje = token.errorMessageDTO.message! };
                    }
                    else
                    {
                        response = new BitacoraResponse<BitacoraDtoResponse> { Codigo = token.statusCode, Contenido = new BitacoraDtoResponse { }, Mensaje = "Error al consumir el servicio del API Tokenizer." };
                    }
                }
                else
                    response = new BitacoraResponse<BitacoraDtoResponse> { Codigo = 500, Contenido = new BitacoraDtoResponse { }, Mensaje = "Error al consumir el servicio del API Tokenizer." };
            }
            return response;
        }
        #endregion
    }
}
