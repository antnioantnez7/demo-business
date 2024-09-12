namespace banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.dominio.model
{
    public class ResponseBaseMicroservicio<T> : ResponseBase where T : class, new()
    {
        #region Properties
        public T Contenido { get; set; }
        #endregion

        #region Constructor
        public ResponseBaseMicroservicio()
        {
            Contenido = new T();
        }
        #endregion
    }
}
