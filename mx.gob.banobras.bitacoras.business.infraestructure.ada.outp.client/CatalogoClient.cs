using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.application.outport;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.dominio.model;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.inp.dto;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.config;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.persistence.dominio.model;
using log4net;
using Newtonsoft.Json;

namespace banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.outp.client
{
    public class CatalogoClient : ICatalogoClientOutPort
    {
        #region Properties
        /// <summary>
        /// Instancia de la interfaz de archivo de configuración appsettings.json
        /// </summary>
        readonly IConfiguration configuration;
        /// <summary>
        /// Instancia de la interfaz de logueo
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(CatalogoClient));
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la implementación cliente puerto de salida que recibe la inyección de dependencias del archivo de configuración de Program.cs
        /// </summary>
        /// <param name="_configuration"></param>
        public CatalogoClient(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region Methods - Catálogo Aplicativos
        /// <summary>
        /// Implementación del método que obtiene los registros del catálogo de Aplicativos.
        /// </summary>
        /// <returns></returns>
        public async Task<BitacoraResponse<List<CatalogoAplicativoDto>>> consultarAplicativos()
        {
            var response = new BitacoraResponse<List<CatalogoAplicativoDto>>();
            ResponseApiTokenizer token = new TokenClient(configuration).validarToken().Result;
            string message = string.Format("RESPONSE API TOKENIZER:\n {0}", JsonConvert.SerializeObject(token, Formatting.Indented));
            Console.WriteLine(message);
            if (token != null && token.statusCode == 200)
            {
                Console.WriteLine("CatalogoClient: MÉTODO consultarAplicativos");
                Console.WriteLine(string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject("GET - Sin REQUEST", Formatting.Indented)));
                _log.Info("CatalogoClient: MÉTODO consultarAplicativos");
                string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject("GET - Sin REQUEST", Formatting.Indented));
                _log.Info(message1);
                var result = await new GenericRestService(configuration).CallServiceWithCertificate(string.Empty,
                                new ResponseBaseMicroservicio<List<CatalogoAplicativoDto>>(),
                                configuration.GetSection("URL_USUARIO_PERSISTENCE").Value!,
                                "persistence/catalogos/aplicativos/consultar",
                                EVerbType.GET,
                                new Dictionary<string, string>());
                Console.WriteLine(string.Format("RESULT:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented)));
                string message2 = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
                _log.Info(message2);
                response = new BitacoraResponse<List<CatalogoAplicativoDto>> { Codigo = result.Model != null ? result.Model.statusCode : 500, Contenido = result.Model != null ? result.Model.Contenido : new List<CatalogoAplicativoDto> { }, Mensaje = result.Model?.message! };
                Console.WriteLine(string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(response, Formatting.Indented)));
            }
            else
            {
                if (token != null)
                {
                    if (token.errorMessageDTO != null)
                    {
                        response = new BitacoraResponse<List<CatalogoAplicativoDto>> { Codigo = token.statusCode, Contenido = new List<CatalogoAplicativoDto> { }, Mensaje = token.errorMessageDTO.message! };
                    }
                    else
                    {
                        response = new BitacoraResponse<List<CatalogoAplicativoDto>> { Codigo = token.statusCode, Contenido = new List<CatalogoAplicativoDto> { }, Mensaje = "Error al consumir el servicio del API Tokenizer." };
                    }
                } 
                else
                    response = new BitacoraResponse<List<CatalogoAplicativoDto>> { Codigo = 500, Contenido = new List<CatalogoAplicativoDto> { }, Mensaje = "Error al consumir el servicio del API Tokenizer." };
            }
            return response;
        }

        /// <summary>
        /// Implementación del método que elimina el registro del catálogo de Aplicativos.
        /// </summary>
        /// <param name="aplicativoId"></param>
        /// <returns></returns>
        public async Task<BitacoraResponse<BitacoraDtoResponse>> eliminarAplicativo(string aplicativoId)
        {
            var response = new BitacoraResponse<BitacoraDtoResponse>();
            ResponseApiTokenizer token = new TokenClient(configuration).validarToken().Result;
            string message = string.Format("RESPONSE API TOKENIZER:\n {0}", JsonConvert.SerializeObject(token, Formatting.Indented));
            Console.WriteLine(message);
            if (token != null && token.statusCode == 200)
            {
                _log.Info("CatalogoClient: MÉTODO eliminarAplicativo");
                string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(aplicativoId, Formatting.Indented));
                _log.Info(message1);
                var result = await new GenericRestService(configuration).CallServiceWithCertificate(aplicativoId,
                                new ResponseBaseMicroservicio<BitacoraDtoResponse>(),
                                configuration.GetSection("URL_USUARIO_PERSISTENCE").Value!,
                                string.Format("persistence/catalogos/aplicativos/eliminar/{0}", aplicativoId),
                                EVerbType.DELETE,
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
                        response = new BitacoraResponse<BitacoraDtoResponse> { Codigo = token.statusCode, Contenido = new BitacoraDtoResponse { }, Mensaje = token.errorMessageDTO.message!};
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

        /// <summary>
        /// Implementación del método para agregar un registro en el catálogo de Aplicativos.
        /// </summary>
        /// <param name="catalogoAplicativoDTO"></param>
        /// <returns></returns>
        public async Task<BitacoraResponse<BitacoraDtoResponse>> registrarAplicativo(CatalogoAplicativoDto catalogoAplicativoDTO)
        {
            var response = new BitacoraResponse<BitacoraDtoResponse>();
            ResponseApiTokenizer token = new TokenClient(configuration).validarToken().Result;
            string message = string.Format("RESPONSE API TOKENIZER:\n {0}", JsonConvert.SerializeObject(token, Formatting.Indented));
            Console.WriteLine(message);
            if (token != null && token.statusCode == 200)
            {
                _log.Info("CatalogoClient: MÉTODO registrarAplicativo");
                string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(catalogoAplicativoDTO, Formatting.Indented));
                _log.Info(message1);
                var result = await new GenericRestService(configuration).CallServiceWithCertificate(catalogoAplicativoDTO,
                                new ResponseBaseMicroservicio<BitacoraDtoResponse>(),
                                configuration.GetSection("URL_USUARIO_PERSISTENCE").Value!,
                                "persistence/catalogos/aplicativos/registrar",
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
                        response = new BitacoraResponse<BitacoraDtoResponse> { Codigo = token.statusCode, Contenido = new BitacoraDtoResponse { }, Mensaje = token.errorMessageDTO.message!};
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

        /// <summary>
        /// Implementación del método para actualizar un registro en el catálogo de Aplicativos.
        /// </summary>
        /// <param name="catalogoAplicativoDTO"></param>
        /// <returns></returns>
        public async Task<BitacoraResponse<BitacoraDtoResponse>> actualizarAplicativo(CatalogoAplicativoDto catalogoAplicativoDTO)
        {
            var response = new BitacoraResponse<BitacoraDtoResponse>();
            ResponseApiTokenizer token = new TokenClient(configuration).validarToken().Result;
            string message = string.Format("RESPONSE API TOKENIZER:\n {0}", JsonConvert.SerializeObject(token, Formatting.Indented));
            Console.WriteLine(message);
            if (token != null && token.statusCode == 200)
            {
                _log.Info("CatalogoClient: MÉTODO actualizarAplicativo");
                string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(catalogoAplicativoDTO, Formatting.Indented));
                _log.Info(message1);
                var result = await new GenericRestService(configuration).CallServiceWithCertificate(catalogoAplicativoDTO,
                                new ResponseBaseMicroservicio<BitacoraDtoResponse>(),
                                configuration.GetSection("URL_USUARIO_PERSISTENCE").Value!,
                                "persistence/catalogos/aplicativos/actualizar",
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
                        response = new BitacoraResponse<BitacoraDtoResponse> { Codigo = token.statusCode, Contenido = new BitacoraDtoResponse { }, Mensaje = token.errorMessageDTO.message!};
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

        #region Methods- Catálogo Usuarios
        /// <summary>
        /// Implementación del método que obtiene los registros del catálogo de Aplicativos.
        /// </summary>
        /// <returns></returns>
        public async Task<BitacoraResponse<List<CatalogoUsuarioDto>>> consultarUsuarios(CatalogoUsuarioDto catalogoUsuarioDto)
        {
            var response = new BitacoraResponse<List<CatalogoUsuarioDto>>();
            ResponseApiTokenizer token = new TokenClient(configuration).validarToken().Result;
            string message = string.Format("RESPONSE API TOKENIZER:\n {0}", JsonConvert.SerializeObject(token, Formatting.Indented));
            Console.WriteLine(message);
            if (token != null && token.statusCode == 200)
            {
                Console.WriteLine("CatalogoClient: MÉTODO consultarUsuarios");
                Console.WriteLine(string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(catalogoUsuarioDto, Formatting.Indented)));
                _log.Info("CatalogoClient: MÉTODO consultarUsuarios");
                string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(catalogoUsuarioDto, Formatting.Indented));
                _log.Info(message1);
                var result = await new GenericRestService(configuration).CallServiceWithCertificate(catalogoUsuarioDto,
                                new ResponseBaseMicroservicio<List<CatalogoUsuarioDto>>(),
                                configuration.GetSection("URL_USUARIO_PERSISTENCE").Value!,
                                "persistence/catalogos/usuarios/consultar",
                                EVerbType.POST,
                                new Dictionary<string, string>());
                Console.WriteLine(string.Format("RESULT:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented)));
                string message2 = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(result, Formatting.Indented));
                _log.Info(message2);
                response = new BitacoraResponse<List<CatalogoUsuarioDto>> { Codigo = result.Model != null ? result.Model.statusCode : 500, Contenido = result.Model != null ? result.Model.Contenido : new List<CatalogoUsuarioDto> { }, Mensaje = result.Model?.message! };
                Console.WriteLine(string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(response, Formatting.Indented)));
            }
            else
            {
                if (token != null)
                {
                    if (token.errorMessageDTO != null)
                    {
                        response = new BitacoraResponse<List<CatalogoUsuarioDto>> { Codigo = token.statusCode, Contenido = new List<CatalogoUsuarioDto> { }, Mensaje = token.errorMessageDTO.message! };
                    }
                    else
                    {
                        response = new BitacoraResponse<List<CatalogoUsuarioDto>> { Codigo = token.statusCode, Contenido = new List<CatalogoUsuarioDto> { }, Mensaje = "Error al consumir el servicio del API Tokenizer." };
                    }
                }
                else
                    response = new BitacoraResponse<List<CatalogoUsuarioDto>> { Codigo = 500, Contenido = new List<CatalogoUsuarioDto> { }, Mensaje = "Error al consumir el servicio del API Tokenizer." };
            }
            return response;
        }

        /// <summary>
        /// Implementación del método para guardar (registrar o actualizar) un registro en el catálogo de Usuarios.
        /// </summary>
        /// <param name="catalogoUsuarioDto"></param>
        /// <returns></returns>
        public async Task<BitacoraResponse<BitacoraDtoResponse>> guardarUsuario(CatalogoUsuarioDto catalogoUsuarioDto)
        {
            var response = new BitacoraResponse<BitacoraDtoResponse>();
            ResponseApiTokenizer token = new TokenClient(configuration).validarToken().Result;
            string message = string.Format("RESPONSE API TOKENIZER:\n {0}", JsonConvert.SerializeObject(token, Formatting.Indented));
            Console.WriteLine(message);
            if (token != null && token.statusCode == 200)
            {
                _log.Info("CatalogoClient: MÉTODO guardarUsuario");
                string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(catalogoUsuarioDto, Formatting.Indented));
                _log.Info(message1);
                var result = await new GenericRestService(configuration).CallServiceWithCertificate(catalogoUsuarioDto,
                                new ResponseBaseMicroservicio<BitacoraDtoResponse>(),
                                configuration.GetSection("URL_USUARIO_PERSISTENCE").Value!,
                                "persistence/catalogos/usuarios/guardar",
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

        /// <summary>
        /// Implementación del método que elimina el registro del catálogo de Usuarios.
        /// </summary>
        /// <param name="identificador"></param>
        /// <returns></returns>
        public async Task<BitacoraResponse<BitacoraDtoResponse>> eliminarUsuario(int identificador)
        {
            var response = new BitacoraResponse<BitacoraDtoResponse>();
            ResponseApiTokenizer token = new TokenClient(configuration).validarToken().Result;
            string message = string.Format("RESPONSE API TOKENIZER:\n {0}", JsonConvert.SerializeObject(token, Formatting.Indented));
            Console.WriteLine(message);
            if (token != null && token.statusCode == 200)
            {
                _log.Info("CatalogoClient: MÉTODO eliminarUsuario");
                string message1 = string.Format("REQUEST:\n {0}", JsonConvert.SerializeObject(identificador, Formatting.Indented));
                _log.Info(message1);
                var result = await new GenericRestService(configuration).CallServiceWithCertificate(identificador,
                                new ResponseBaseMicroservicio<BitacoraDtoResponse>(),
                                configuration.GetSection("URL_USUARIO_PERSISTENCE").Value!,
                                string.Format("persistence/catalogos/usuarios/eliminar/{0}", identificador),
                                EVerbType.DELETE,
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
