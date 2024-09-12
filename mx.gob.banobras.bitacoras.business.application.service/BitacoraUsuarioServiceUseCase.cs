using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.application.inputport;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.application.outport;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.dominio.model;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.inp.dto;

namespace banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.application.service
{
    public class BitacoraUsuarioServiceUseCase : IBitacoraUsuarioInputPort
    {
        #region Properties
        /// <summary>
        /// Instancia de la interfaz del cliente puerto de salida
        /// </summary>
        private readonly IBitacoraUsuarioClientOutPort iBitacoraUsuarioClientOutPort;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor que recibe la inyección de dependencias de la interfaz del cliente puerto de salida
        /// </summary>
        /// <param name="iBitacoraUsuarioClientOutPort"></param>
        public BitacoraUsuarioServiceUseCase(IBitacoraUsuarioClientOutPort iBitacoraUsuarioClientOutPort)
        {
            this.iBitacoraUsuarioClientOutPort = iBitacoraUsuarioClientOutPort;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Implementación del método para crear un registro en la bitácora de cambios a usuarios y regresa un objeto de respuesta.
        /// </summary>
        /// <param name="bitacoraConsultaDTO"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<List<BitacoraUsuarioDto>>> consultar(BitacoraConsultaDto bitacoraConsultaDTO)
        {
            return iBitacoraUsuarioClientOutPort.consultar(bitacoraConsultaDTO);
        }

        /// <summary>
        /// Implementación del método que obtiene los registros de bitácoras de una aplicación que satisfacen las condiciones especificadas en los parámetros.
        /// </summary>
        /// <param name="bitacoraUsuarioDTO"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> registrar(BitacoraUsuarioDto bitacoraUsuarioDTO)
        {
            return iBitacoraUsuarioClientOutPort.registrar(bitacoraUsuarioDTO);
        }
        #endregion
    }
}
