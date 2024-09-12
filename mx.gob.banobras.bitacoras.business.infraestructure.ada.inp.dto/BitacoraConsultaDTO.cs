namespace banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.inp.dto
{
    public class BitacoraConsultaDto
    {
        public BitacoraConsultaDto()
        {
            aplicativoId = string.Empty;
        }
        #region Properties
        /// <summary>
        /// Acrónimo e identificador de aplicativo.
        /// </summary>
        public string aplicativoId { get; set; }
        /// <summary>
        /// Fecha hora inicio de la transacción.
        /// </summary>
        public DateTime? fechaHoraIni { get; set; }
        /// <summary>
        /// Fecha hora fin de la transacción.
        /// </summary>
        public DateTime? fechaHoraFin { get; set; }
        /// <summary>
        /// Valor booleano para bitacora historica o no.
        /// </summary>
        public bool historico { get; set; } = false;
        #endregion
    }
}
