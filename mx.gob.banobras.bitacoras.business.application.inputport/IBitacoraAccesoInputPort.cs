﻿using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.dominio.model;
using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.inp.dto;

namespace banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.application.inputport
{
    public interface IBitacoraAccesoInputPort
    {
        #region Methods
        /// <summary>
        /// Declaración del método para agregar el registro en la bitácora de accesos cuando se intenta ingresar a una aplicación y regresa un objeto de respuesta.
        /// </summary>
        /// <param name="bitacoraAccesoDTO"></param>
        /// <returns></returns>
        Task<BitacoraResponse<BitacoraDtoResponse>> registrar(BitacoraAccesoDto bitacoraAccesoDTO);
        /// <summary>
        /// Declaración del método que obtiene los registros de bitácoras de una aplicación que satisfacen las condiciones especificadas en los parámetros.
        /// </summary>
        /// <param name="bitacoraConsultaDTO"></param>
        /// <returns></returns>
        Task<BitacoraResponse<List<BitacoraAccesoDto>>> consultar(BitacoraConsultaDto bitacoraConsultaDTO);
        #endregion
    }
}
