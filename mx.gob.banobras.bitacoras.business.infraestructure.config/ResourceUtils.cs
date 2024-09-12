namespace banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.config
{
    /// <summary>
    /// Clase que contiene la variables globales utilizades en el proyecto
    /// </summary>
    public class ResourceUtils
    {
        private ResourceUtils()
        {
        }
        #region Properties
        /// <summary>
        /// Variable global que contiene los HEADERS para consumir el servicio de BITACORAS>API TOKENIZER
        /// </summary>
        public static Dictionary<string, string> HEADERS { get; set; } = new Dictionary<string, string>();
        #endregion
    }
}
