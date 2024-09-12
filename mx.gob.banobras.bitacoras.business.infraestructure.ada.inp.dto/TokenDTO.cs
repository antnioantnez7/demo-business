namespace banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.ada.inp.dto
{
    public class TokenDto
    {
        public bool valid { get; set; }
        public string? type { get; set; }
        public string? token { get; set; }
        public string? refreshToken { get; set; }
    }

    public class ErrorMessageDto
    {
        public int statusCode { get; set; }
        public DateTime timestamp { get; set; }
        public string? message { get; set; }
    }
}
