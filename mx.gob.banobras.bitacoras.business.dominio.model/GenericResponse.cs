namespace banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.dominio.model
{
    public class GenericResponse<T>
    {
        #region Properties

        /// <summary>
        /// Guid de las operaciones
        /// </summary>
        public string TransactionId { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }

        public T? Model { get; set; }

        #endregion

        #region Constructor

        public GenericResponse()
        {
            Success = false;
            Message = string.Empty;
            Model = default(T);
            TransactionId = string.Empty;
        }

        #endregion
    }
}
