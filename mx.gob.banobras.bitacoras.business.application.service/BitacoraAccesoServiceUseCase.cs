using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.application.inputport;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.application.outport;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.dominio.model;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.inp.dto;

namespace banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.application.service
{
    public class BitacoraAccesoServiceUseCase: IBitacoraAccesoInputPort
    {
        #region Properties
        /// <summary>
        /// Instancia de la interfaz del cliente puerto de salida
        /// </summary>
        private readonly IBitacoraAccesoClientOutPort iBitacoraAccesoClientOutPort;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor que recibe la inyección de dependencias de la interfaz del cliente puerto de salida
        /// </summary>
        /// <param name="iBitacoraAccesoClientInputPort"></param>
        public BitacoraAccesoServiceUseCase(IBitacoraAccesoClientOutPort iBitacoraAccesoClientInputPort)
        {
            this.iBitacoraAccesoClientOutPort = iBitacoraAccesoClientInputPort;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Implementación del método que obtiene los registros de bitácoras de una aplicación que satisfacen las condiciones especificadas en los parámetros.
        /// </summary>
        /// <param name="bitacoraConsultaDTO"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<List<BitacoraAccesoDto>>> consultar(BitacoraConsultaDto bitacoraConsultaDTO)
        {
            return iBitacoraAccesoClientOutPort.consultar(bitacoraConsultaDTO);
        }

        /// <summary>
        /// Implementación del método para agregar el registro en la bitácora de accesos cuando se intenta ingresar a una aplicación y regresa un objeto de respuesta.
        /// </summary>
        /// <param name="bitacoraAccesoDTO"></param>
        /// <returns></returns>
        public Task<BitacoraResponse<BitacoraDtoResponse>> registrar(BitacoraAccesoDto bitacoraAccesoDTO)
        {
            return iBitacoraAccesoClientOutPort.registrar(bitacoraAccesoDTO);
        }
        #endregion
    }
}
