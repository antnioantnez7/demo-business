using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.application.inputport;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.application.outport;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.dominio.model;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.inp.dto;

namespace banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.application.service
{
    public class CatalogoServiceUseCase: ICatalogoInputPort
    {
        #region Properties
        /// <summary>
        /// Instancia de la interfaz del repositorio puerto de salida
        /// </summary>
        private readonly ICatalogoClientOutPort iCatalogoClientOutPort;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor que recibe la inyección de dependencias de la interfaz del repositorio puerto de salida
        /// </summary>
        /// <param name="iCatalogoClientOutPort"></param>
        public CatalogoServiceUseCase(ICatalogoClientOutPort iCatalogoClientOutPort)
        {
            this.iCatalogoClientOutPort = iCatalogoClientOutPort;
        }
        #endregion

        #region Methods - Catálogo Aplicativos
        /// <summary>
        /// Implementación del método que obtiene los registros del catálogo de Aplicativos que satisfacen las condiciones especificadas en los parámetros.
        /// </summary>
        /// <returns></returns>
        public Task<BitacoraResponse<List<CatalogoAplicativoDto>>> consultarAplicativos()
        {
            return iCatalogoClientOutPort.consultarAplicativos();
        }

        /// <summary>
        /// Implementación del método que elimina el registro del catálogo de Aplicativos.
        /// </summary>
        /// <param name="aplicativoId"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> eliminarAplicativo(string aplicativoId)
        {
            return iCatalogoClientOutPort.eliminarAplicativo(aplicativoId);
        }

        /// <summary>
        /// Implementación del método para agregar un registro en el catálogo de Aplicativos.
        /// </summary>
        /// <param name="catalogoAplicativoDTO"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> registrarAplicativo(CatalogoAplicativoDto catalogoAplicativoDTO)
        {
            return iCatalogoClientOutPort.registrarAplicativo(catalogoAplicativoDTO);
        }

        /// <summary>
        /// Implementación del método para actualizar un registro en el catálogo de Aplicativos.
        /// </summary>
        /// <param name="catalogoAplicativoDTO"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> actualizarAplicativo(CatalogoAplicativoDto catalogoAplicativoDTO)
        {
            return iCatalogoClientOutPort.actualizarAplicativo(catalogoAplicativoDTO);
        }
        #endregion

        #region Methods - Catálogo Usuarios
        /// <summary>
        /// Implementación del método que obtiene los registros del catálogo de Usuarios que satisfacen las condiciones especificadas en los parámetros.
        /// </summary>
        /// <returns></returns>
        public Task<BitacoraResponse<List<CatalogoUsuarioDto>>> consultarUsuarios(CatalogoUsuarioDto catalogoUsuarioDto)
        {
            return iCatalogoClientOutPort.consultarUsuarios(catalogoUsuarioDto);
        }

        /// <summary>
        /// Implementación del método que elimina el registro del catálogo de Usuarios.
        /// </summary>
        /// <param name="identificador"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> eliminarUsuario(int identificador)
        {
            return iCatalogoClientOutPort.eliminarUsuario(identificador);
        }

        /// <summary>
        /// Implementación del método para guardar (agregar o actualizar) un registro en el catálogo de Usuarios.
        /// </summary>
        /// <param name="catalogoUsuarioDto"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> guardarUsuario(CatalogoUsuarioDto catalogoUsuarioDto)
        {
            return iCatalogoClientOutPort.guardarUsuario(catalogoUsuarioDto);
        }
        #endregion
    }
}
