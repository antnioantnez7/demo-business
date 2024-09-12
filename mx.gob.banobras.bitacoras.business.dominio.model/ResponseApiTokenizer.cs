using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.inp.dto;

namespace banobras_bitacoras_business.mx.gob.banobras.bitacoras.persistence.dominio.model
{
    public class ResponseApiTokenizer
    {
        public int statusCode { get; set; }
        public TokenDto? tokenDTO { get; set; }
        public ErrorMessageDto? errorMessageDTO { get; set; }
    }
}
