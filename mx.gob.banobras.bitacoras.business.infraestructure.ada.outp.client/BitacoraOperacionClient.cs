using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.application.outport;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.dominio.model;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.inp.dto;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.outp.client;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.config;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.persistence.dominio.model;
using log4net;
using Newtonsoft.Json;

namespace banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.outp.client
{
    public class BitacoraOperacionClient: IBitacoraOperacionClientOutPort
    {
        #region Properties
        /// <summary>
        /// Instancia de la interfaz de archivo de configuración appsettings.json
        /// </summary>
        readonly IConfiguration configuration;
        /// <summary>
        /// Instancia de la interfaz de logueo
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(BitacoraOperacionClient));
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la implementación cliente puerto de salida que recibe la inyección de dependencias del archivo de configuración de Program.cs
        /// </summary>
        /// <param name="_configuration"></param>
        public BitacoraOperacionClient(IConfiguration _configuration)
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
        public async Task<BitacoraResponse<List<BitacoraOperacionDto>>> consultar(BitacoraConsultaDto bitacoraConsultaDTO)
        {
            var response = new BitacoraResponse<List<BitacoraOperacionDto>>();
            ResponseApiTokenizer token = new TokenClient(configuration).validarToken().Result;
            string message = string.Format("RESPONSE API TOKENIZER:\n {0}", JsonConvert.SerializeObject(token, Formatting.Indented));
            Console.WriteLine(message);
            if (token != null && token.statusCode == 200){
                _log.Info("BitacoraOperacionClient: MÉTODO consultar");
                string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(bitacoraConsultaDTO, Formatting.Indented));
                _log.Info(message1);
                var result = await new GenericRestService(configuration).CallServiceWithCertificate(bitacoraConsultaDTO,
                                new ResponseBaseMicroservicio<List<BitacoraOperacionDto>>(),
                                configuration.GetSection("URL_USUARIO_PERSISTENCE").Value!,
                                "persistence/operaciones/consultar",
                                EVerbType.POST,
                                new Dictionary<string, string>());
                string message2 = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
                _log.Info(message2);
                response = new BitacoraResponse<List<BitacoraOperacionDto>> { Codigo = result.Model != null ? result.Model.statusCode : 500, Contenido = result.Model != null ? result.Model.Contenido : new List<BitacoraOperacionDto> { }, Mensaje = result.Model?.message! };
            }
            else
            {
                if (token != null)
                {
                    if (token.errorMessageDTO != null)
                    {
                        response = new BitacoraResponse<List<BitacoraOperacionDto>> { Codigo = token.statusCode, Contenido = new List<BitacoraOperacionDto> { }, Mensaje = token.errorMessageDTO.message! };
                    }
                    else
                    {
                        response = new BitacoraResponse<List<BitacoraOperacionDto>> { Codigo = token.statusCode, Contenido = new List<BitacoraOperacionDto> { }, Mensaje = "Error al consumir el servicio del API Tokenizer." };
                    }
                }
                else
                    response = new BitacoraResponse<List<BitacoraOperacionDto>> { Codigo = 500, Contenido = new List<BitacoraOperacionDto> { }, Mensaje = "Error al consumir el servicio del API Tokenizer." };
            }
            return response;
        }

        /// <summary>
        /// Implementación del método 'registrar', se realiza la conexión a la capa de persistencia
        /// </summary>
        /// <param name="bitacoraOperacionDTO"></param>
        /// <returns></returns>
        public async Task<BitacoraResponse<BitacoraDtoResponse>> registrar(BitacoraOperacionDto bitacoraOperacionDTO)
        {
            var response = new BitacoraResponse<BitacoraDtoResponse>();
            ResponseApiTokenizer token = new TokenClient(configuration).validarToken().Result;
            string message = string.Format("RESPONSE API TOKENIZER:\n {0}", JsonConvert.SerializeObject(token, Formatting.Indented));
            Console.WriteLine(message);
            if (token != null && token.statusCode == 200)
            {
                _log.Info("BitacoraOperacionClient: MÉTODO registrar");
                string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(bitacoraOperacionDTO, Formatting.Indented));
                _log.Info(message1);
                var result = await new GenericRestService(configuration).CallServiceWithCertificate(bitacoraOperacionDTO,
                                new ResponseBaseMicroservicio<BitacoraDtoResponse>(),
                                configuration.GetSection("URL_USUARIO_PERSISTENCE").Value!,
                                "persistence/operaciones/registrar",
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
