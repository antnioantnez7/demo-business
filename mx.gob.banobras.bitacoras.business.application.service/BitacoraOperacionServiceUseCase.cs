using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.application.inputport;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.application.outport;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.dominio.model;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.inp.dto;

namespace banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.application.service
{
    public class BitacoraOperacionServiceUseCase: IBitacoraOperacionInputPort
    {
        #region Properties
        /// <summary>
        /// Instancia de la interfaz del cliente puerto de salida
        /// </summary>
        private readonly IBitacoraOperacionClientOutPort iBitacoraOperacionClientOutPort;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor que recibe la inyección de dependencias de la interfaz del cliente puerto de salida
        /// </summary>
        /// <param name="iBitacoraOperacionClientOutPort"></param>
        public BitacoraOperacionServiceUseCase(IBitacoraOperacionClientOutPort iBitacoraOperacionClientOutPort)
        {
            this.iBitacoraOperacionClientOutPort = iBitacoraOperacionClientOutPort;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Implementación del método que obtiene los registros de bitácoras de una aplicación que satisfacen las condiciones especificadas en los parámetros.
        /// </summary>
        /// <param name="bitacoraConsultaDTO"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<List<BitacoraOperacionDto>>> consultar(BitacoraConsultaDto bitacoraConsultaDTO)
        {
            return iBitacoraOperacionClientOutPort.consultar(bitacoraConsultaDTO);
        }

        /// <summary>
        /// Implementación del método para agregar un registro en la bitácora de operaciones correspondiente al seguimiento de un proceso o actividad y regresa un objeto de respuesta.
        /// </summary>
        /// <param name="bitacoraOperacionDTO"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> registrar(BitacoraOperacionDto bitacoraOperacionDTO)
        {
            return iBitacoraOperacionClientOutPort.registrar(bitacoraOperacionDTO);
        }
        #endregion
    }
}
