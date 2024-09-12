namespace banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.inp.dto
{
    public class CatalogoAplicativoDto
    {
        public CatalogoAplicativoDto()
        {
            aplicativoId = string.Empty;
            nombre = string.Empty;
        }
        #region Properties
        /// <summary>
        /// Identificador del catálogo, es un acrónimo como (MAC, SIGEVI)
        /// </summary>
        public string aplicativoId { get; set; }
        /// <summary>
        /// Nombre completo del acrónimo
        /// </summary>
        public string nombre { get; set; }
        /// <summary>
        /// Identificador del catálogo de usuario (debe estar registrado)
        /// </summary>
        public required int usuarioRegistro { get; set; }
        /// <summary>
        /// Fecha de registro
        /// </summary>
        public DateTime? fechaRegistro { get; set; }
        /// <summary>
        /// Identificador del catálogo de usuario (debe estar registrado)
        /// </summary>
        public required int usuarioModifica { get; set; }
        /// <summary>
        /// Fecha de modificación
        /// </summary>
        public DateTime? fechaModifica { get; set; }
        #endregion
    }
}
