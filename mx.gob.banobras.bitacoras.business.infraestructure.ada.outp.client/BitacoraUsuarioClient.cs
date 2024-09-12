using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.application.outport;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.dominio.model;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.inp.dto;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.config;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.persistence.dominio.model;
using log4net;
using Newtonsoft.Json;

namespace banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.outp.client
{
    public class BitacoraUsuarioClient : IBitacoraUsuarioClientOutPort
    {
        #region Properties
        /// <summary>
        /// Instancia de la interfaz de archivo de configuración appsettings.json
        /// </summary>
        readonly IConfiguration configuration;
        /// <summary>
        /// Instancia de la interfaz de logueo
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(BitacoraUsuarioClient));
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la implementación cliente puerto de salida que recibe la inyección de dependencias del archivo de configuración de Program.cs
        /// </summary>
        /// <param name="_configuration"></param>
        public BitacoraUsuarioClient(IConfiguration _configuration)
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
        public async Task<BitacoraResponse<List<BitacoraUsuarioDto>>> consultar(BitacoraConsultaDto bitacoraConsultaDTO)
        {
            var response = new BitacoraResponse<List<BitacoraUsuarioDto>>();
            ResponseApiTokenizer token = new TokenClient(configuration).validarToken().Result;
            string message = string.Format("RESPONSE API TOKENIZER:\n {0}", JsonConvert.SerializeObject(token, Formatting.Indented));
            Console.WriteLine(message);
            if (token != null && token.statusCode == 200)
            {
                _log.Info("BitacoraUsuarioClient: MÉTODO consultar");
                string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(bitacoraConsultaDTO, Formatting.Indented));
                _log.Info(message1);
                var result = await new GenericRestService(configuration).CallServiceWithCertificate(bitacoraConsultaDTO,
                                new ResponseBaseMicroservicio<List<BitacoraUsuarioDto>>(),
                                configuration.GetSection("URL_USUARIO_PERSISTENCE").Value!,
                                "persistence/usuarios/consultar",
                                EVerbType.POST,
                                new Dictionary<string, string>());
                string message2 = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
                _log.Info(message2);
                response = new BitacoraResponse<List<BitacoraUsuarioDto>> { Codigo = result.Model != null ? result.Model.statusCode : 500, Contenido = result.Model != null ? result.Model.Contenido : new List<BitacoraUsuarioDto> { }, Mensaje = result.Model?.message! };
            }
            else
            {
                if (token != null)
                {
                    if (token.errorMessageDTO != null)
                    {
                        response = new BitacoraResponse<List<BitacoraUsuarioDto>> { Codigo = token.statusCode, Contenido = new List<BitacoraUsuarioDto> { }, Mensaje = token.errorMessageDTO.message! };
                    }
                    else
                    {
                        response = new BitacoraResponse<List<BitacoraUsuarioDto>> { Codigo = token.statusCode, Contenido = new List<BitacoraUsuarioDto> { }, Mensaje = "Error al consumir el servicio del API Tokenizer." };
                    }
                }
                else
                    response = new BitacoraResponse<List<BitacoraUsuarioDto>> { Codigo = 500, Contenido = new List<BitacoraUsuarioDto> { }, Mensaje = "Error al consumir el servicio del API Tokenizer." };
            }
            return response;
        }

        /// <summary>
        /// Implementación del método 'registrar', se realiza la conexión a la capa de persistencia
        /// </summary>
        /// <param name="bitacoraUsuarioDTO"></param>
        /// <returns></returns>
        public async Task<BitacoraResponse<BitacoraDtoResponse>> registrar(BitacoraUsuarioDto bitacoraUsuarioDTO)
        {
            var response = new BitacoraResponse<BitacoraDtoResponse>();
            ResponseApiTokenizer token = new TokenClient(configuration).validarToken().Result;
            string message = string.Format("RESPONSE API TOKENIZER:\n {0}", JsonConvert.SerializeObject(token, Formatting.Indented));
            Console.WriteLine(message);
            if (token != null && token.statusCode == 200)
            {
                _log.Info("BitacoraUsuarioClient: MÉTODO registrar");
                string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(bitacoraUsuarioDTO, Formatting.Indented));
                _log.Info(message1);
                var result = await new GenericRestService(configuration).CallServiceWithCertificate(bitacoraUsuarioDTO,
                                new ResponseBaseMicroservicio<BitacoraDtoResponse>(),
                                configuration.GetSection("URL_USUARIO_PERSISTENCE").Value!,
                                "persistence/usuarios/registrar",
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
