using banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.dominio.model;
using log4net;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Unicode;

namespace banobras_bitacoras_business.mx.gob.banobras.bitacoras.business.infraestructure.config
{
    public class GenericRestService
    {
        #region Properties
        /// <summary>
        /// Instancia de la interfaz de archivo de configuración appsettings.json
        /// </summary>
        readonly IConfiguration configuration;
        /// <summary>
        /// Instancia de la interfaz de logueo
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(GenericRestService));
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor de la implementación cliente puerto de salida que recibe la inyección de dependencias del archivo de configuración de Program.cs
        /// </summary>
        /// <param name="_configuration"></param>
        public GenericRestService(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region REST Service
        public async Task<GenericResponse<U>> CallServiceWithCertificate<T, U>(T modelRequest, U modelResponse, string uriBase, string controller, EVerbType verb, Dictionary<string, string> headers)
        {
            GenericResponse<U> resultService = new()
            {
                Success = false,
                Message = string.Empty
            };

            try
            {
                var handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
                var clientResponse = new HttpClient(handler);

                var client = clientResponse;

                client.BaseAddress = new Uri(uriBase);
                Console.WriteLine(string.Format("uriBase: {0}", uriBase));
                string message = $"uriBase: {uriBase}";
                _log.Info(message);

                // Se agregan los Headers default
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var method = new HttpMethod(verb.ToString());
                var request = new HttpRequestMessage(method, controller);

                // Se obtienen los headers
                foreach (var header in headers)
                {
                    if (!string.IsNullOrEmpty(header.Key) && !string.IsNullOrEmpty(header.Value))
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }
                string message2 = string.Format("HEADERS SendAsync:\n{0}", request.Headers);
                _log.Info(message2);

                // Se obtinenen las propiedades del modelo de Request
                var keyValues = modelRequest;

                // Se codifican las propiedades que serán enviadas 
                request.Content = new StringContent(JsonConvert.SerializeObject(keyValues), Encoding.UTF8, "application/json");
                string message3 = string.Format("REQUEST SendAsync:\n{0}", request);
                _log.Info(message3);
                // Se realiza la petición al servicio
                var response = client.SendAsync(request).Result;

                // Se valida el resultado del servicio
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(string.Format("Response sin realizar binding: {0}", jsonResponse));
                    string message4 = string.Format("RESPONSE sin realizar binding:\n{0}", jsonResponse);
                    _log.Info(message4);
                    var model = JsonConvert.DeserializeObject<U>(jsonResponse);
                    resultService.Model = model!;
                    resultService.Success = true;

                    Console.WriteLine(string.Format("La comunicación con el servicio fué exitosa model: {0}", model));
                }
                else
                {
                    if (response != null)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(string.Format("Response sin realizar binding: {0}", jsonResponse));

                        var model = JsonConvert.DeserializeObject<U>(jsonResponse);
                        resultService.Model = model!;
                        resultService.Success = true;
                        Console.WriteLine(string.Format("La comunicación con el servicio fué exitosa model: {0}", model));

                    }
                    else
                    {
                        resultService.Success = false;
                        resultService.Message = "Error, respuesta nula";
                        resultService.Model = default(U)!;
                    }
                }
                if (!resultService.Success && string.IsNullOrEmpty(resultService.Message))
                    resultService.Message = $"Error al consumir el servicio: {uriBase}{controller}";

                client.Dispose();
            }
            catch (Exception ex)
            {
                resultService.Success = false;
                resultService.Message = string.Format("Message:{0}", ex.Message);
                resultService.Model = default(U)!;
                _log.Error("Excepción en la petición al servicio RestService: CallServiceWithCertificate: ");
                _log.Error(ex);

            }
            return resultService;
        }

        public async Task<GenericResponse<U>> CallServiceWithCertificateV2<T, U>(T modelRequest, U modelResponse, string uriBase, string controller, EVerbType verb, Dictionary<string, string> headers)
        {
            GenericResponse<U> resultService = new()
            {
                Success = false,
                Message = string.Empty
            };

            try
            {

                var handler = new HttpClientHandler();
                //X509Certificate2 certificate = new X509Certificate2(configuration.GetSection("RUTA_CERTIFICADO_BANOBRAS").Value!);
                //var clientCertificate = X509Certificate2.CreateFromSignedFile(configuration.GetSection("RUTA_CERTIFICADO_BANOBRAS_PEM").Value!);
                //var certificate = new X509Certificate2(clientCertificate.Export(X509ContentType.Pfx), "", X509KeyStorageFlags.DefaultKeySet);

                /*Console.WriteLine("{0}Subject: {1}{0}", Environment.NewLine, certificate.Subject);
                Console.WriteLine("{0}Issuer: {1}{0}", Environment.NewLine, certificate.Issuer);
                Console.WriteLine("{0}Version: {1}{0}", Environment.NewLine, certificate.Version);
                Console.WriteLine("{0}Valid Date: {1}{0}", Environment.NewLine, certificate.NotBefore);
                Console.WriteLine("{0}Expiry Date: {1}{0}", Environment.NewLine, certificate.NotAfter);
                Console.WriteLine("{0}Thumbprint: {1}{0}", Environment.NewLine, certificate.Thumbprint);
                Console.WriteLine("{0}Serial Number: {1}{0}", Environment.NewLine, certificate.SerialNumber);
                Console.WriteLine("{0}Friendly Name: {1}{0}", Environment.NewLine, certificate.PublicKey.Oid.FriendlyName);
                Console.WriteLine("{0}Public Key Format: {1}{0}", Environment.NewLine, certificate.PublicKey.EncodedKeyValue.Format(true));
                Console.WriteLine("{0}Raw Data Length: {1}{0}", Environment.NewLine, certificate.RawData.Length);

                handler.ClientCertificates.Add(certificate);
                handler.ClientCertificateOptions = ClientCertificateOption.Automatic;*/
                handler.ServerCertificateCustomValidationCallback = ValidateCertificate;
                var clientResponse = new HttpClient(handler);
                var client = clientResponse;
                client.BaseAddress = new Uri(uriBase);
                Console.WriteLine(string.Format("uriBase: {0}", uriBase));
                string message = $"uriBase: {uriBase}";
                _log.Info(message);

                // Se agregan los Headers default
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var method = new HttpMethod(verb.ToString());
                var request = new HttpRequestMessage(method, controller);

                // Se obtienen los headers
                foreach (var header in headers)
                {
                    if (!string.IsNullOrEmpty(header.Key) && !string.IsNullOrEmpty(header.Value))
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }
                string message2 = string.Format("HEADERS SendAsync:\n{0}", request.Headers);
                _log.Info(message2);
                Console.WriteLine(message2);

                // Se obtinenen las propiedades del modelo de Request
                var keyValues = modelRequest;

                // Se codifican las propiedades que serán enviadas 
                request.Content = new StringContent(JsonConvert.SerializeObject(keyValues), Encoding.UTF8, "application/json");
                string message3 = string.Format("REQUEST SendAsync:\n{0}", request);
                _log.Info(message3);
                Console.WriteLine(message3);
                // Se realiza la petición al servicio
                var response = client.SendAsync(request).Result;

                // Se valida el resultado del servicio
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(string.Format("Response sin realizar binding: {0}", jsonResponse));
                    string message4 = string.Format("RESPONSE sin realizar binding:\n{0}", jsonResponse);
                    _log.Info(message4);
                    var model = JsonConvert.DeserializeObject<U>(jsonResponse);
                    resultService.Model = model!;
                    resultService.Success = true;

                    Console.WriteLine(string.Format("La comunicación con el servicio fué exitosa model: {0}", model));
                }
                else
                {
                    if (response != null)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(string.Format("Response sin realizar binding: {0}", jsonResponse));
                        string messageX = string.Format("RESPONSE:\n {0}", JsonConvert.SerializeObject(jsonResponse, Formatting.Indented));
                        var model = default(U)!;// JsonConvert.DeserializeObject<U>(jsonResponse);
                        resultService.Model = model!;
                        resultService.Success = true;
                        resultService.Message = messageX;
                        Console.WriteLine(string.Format("La comunicación con el servicio fué exitosa model: {0}", model));

                    }
                    else
                    {
                        resultService.Success = false;
                        resultService.Message = "Error, respuesta nula";
                        resultService.Model = default(U)!;
                    }
                }
                if (!resultService.Success && string.IsNullOrEmpty(resultService.Message))
                    resultService.Message = $"Error al consumir el servicio: {uriBase}{controller}";

                client.Dispose();
            }
            catch (Exception ex)
            {
                X509Certificate2 certificate = new X509Certificate2(configuration.GetSection("RUTA_CERTIFICADO_BANOBRAS").Value!, configuration.GetSection("PASS_CERTIFICADO_BANOBRAS").Value!);

                Console.WriteLine("{0}Subject: {1}{0}", Environment.NewLine, certificate.Subject);
                string messageDetalleExcepcion = JsonConvert.SerializeObject(ex);
                resultService.Success = false;
                resultService.Message = string.Format("Message:{0}", ex.Message+"-/-"+ certificate.Subject+"\nRutaCertificado:"+ configuration.GetSection("RUTA_CERTIFICADO_BANOBRAS").Value)+"\n\n Detalle excepción: \n"+ messageDetalleExcepcion;
                resultService.Model = default(U)!;
                _log.Error("Excepción en la petición al servicio RestService: CallServiceWithCertificate: ");
                _log.Error(ex);
                Console.WriteLine("Excepción en la petición al servicio RestService: CallServiceWithCertificateV2:");
                Console.WriteLine(ex);

            }
            return resultService;
        }

        private static bool ServerCertificateCustomValidation(HttpRequestMessage requestMessage, X509Certificate2? certificate, X509Chain? chain, SslPolicyErrors sslErrors)
        {
            // It is possible to inspect the certificate provided by the server.
            Console.WriteLine($"Requested URI: {requestMessage.RequestUri}");
            Console.WriteLine($"Effective date: {certificate?.GetEffectiveDateString()}");
            Console.WriteLine($"Exp date: {certificate?.GetExpirationDateString()}");
            Console.WriteLine($"Issuer: {certificate?.Issuer}");
            Console.WriteLine($"Subject: {certificate?.Subject}");

            // Based on the custom logic it is possible to decide whether the client considers certificate valid or not
            Console.WriteLine($"Errors: {sslErrors}");
            if (certificate is { Subject: "CN=*.banobras.gob.mx" }) // or your productionwebsite.com , you can read it from the appsettings.json
            {
                return true;
            }
            return sslErrors == SslPolicyErrors.None;
        }

        private static bool ValidateCertificate(HttpRequestMessage request, X509Certificate2 certificate, X509Chain certificateChain, SslPolicyErrors policy)
        {
            var validRootCertificates = new[]
            {
        Convert.FromBase64String(@"MIIGWTCCBUGgAwIBAgIMVA7OcSTvpaKCZS2bMA0GCSqGSIb3DQEBCwUAMFMxCzAJ
BgNVBAYTAkJFMRkwFwYDVQQKExBHbG9iYWxTaWduIG52LXNhMSkwJwYDVQQDEyBH
bG9iYWxTaWduIEdDQyBSMyBEViBUTFMgQ0EgMjAyMDAeFw0yNDAzMjAwMDAzMDda
Fw0yNTA0MjEwMDAzMDZaMBwxGjAYBgNVBAMMESouYmFub2JyYXMuZ29iLm14MIIB
IjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAs/0i2A+mOHn9nXPikBMpyVeb
/WJipa6OmgOGZvKZEJg1PvneV5lgo2gX3eJy7KkY68xi89vGpgTCB/6Acfm2l9Il
o5wZA11Kf8456MWaJA4Cz7MeEKUM8TYnOcuFVOpRByuZXKGX/Z9Bo+LX4uMc2Fhx
wuRlxydh3VLo9yJ0Ks1d/lbmGDcOq9aBwYiMvnEv9twJlVgvr39TTFF05MqWcriz
6pWrjTVhBinEgN/zBftKXGKmx4v6twqNSVW02KqTs8HMrCBsbzKhtA8rkXRMU395
VCdgEkWobQPtFkedwgnCs/AM6OWJrxLrNphfLxNwuzAZKtHp0Fjh0Lby4pjJHQID
AQABo4IDYjCCA14wDgYDVR0PAQH/BAQDAgWgMAwGA1UdEwEB/wQCMAAwgZMGCCsG
AQUFBwEBBIGGMIGDMEYGCCsGAQUFBzAChjpodHRwOi8vc2VjdXJlLmdsb2JhbHNp
Z24uY29tL2NhY2VydC9nc2djY3IzZHZ0bHNjYTIwMjAuY3J0MDkGCCsGAQUFBzAB
hi1odHRwOi8vb2NzcC5nbG9iYWxzaWduLmNvbS9nc2djY3IzZHZ0bHNjYTIwMjAw
VgYDVR0gBE8wTTBBBgkrBgEEAaAyAQowNDAyBggrBgEFBQcCARYmaHR0cHM6Ly93
d3cuZ2xvYmFsc2lnbi5jb20vcmVwb3NpdG9yeS8wCAYGZ4EMAQIBMEEGA1UdHwQ6
MDgwNqA0oDKGMGh0dHA6Ly9jcmwuZ2xvYmFsc2lnbi5jb20vZ3NnY2NyM2R2dGxz
Y2EyMDIwLmNybDAtBgNVHREEJjAkghEqLmJhbm9icmFzLmdvYi5teIIPYmFub2Jy
YXMuZ29iLm14MB0GA1UdJQQWMBQGCCsGAQUFBwMBBggrBgEFBQcDAjAfBgNVHSME
GDAWgBQNmMBzf6u9vdlHS0mtCkoMrD7HfDAdBgNVHQ4EFgQUg1JpImu2ONoF0lg2
6WMFVdUCeiswggF9BgorBgEEAdZ5AgQCBIIBbQSCAWkBZwB2AKLjCuRF772tm344
7Udnd1PXgluElNcrXhssxLlQpEfnAAABjlkrQ7QAAAQDAEcwRQIhAMao2VczShoi
Ly6yZKXLdLMUWyMrdekyCjvEnWhv/IyiAiBh95iOF54TaBOlzMPgqjlQFKFemsut
X8W5dbCDPXXn1wB1AE51oydcmhDDOFts1N8/Uusd8OCOG41pwLH6ZLFimjnfAAAB
jlkrRMoAAAQDAEYwRAIgS37PP681K6W6cH9z6+u+HcLYJfmHCe0xRtpTccwFYuUC
IAdSLiVlDzHeq4y3gZkZX2DSj6n12HToP5hczPNW8c6iAHYA4JKz/AwdyOdoNh/e
YbmWTQpSeBmKctZyxLBNpW1vVAQAAAGOWStEmQAABAMARzBFAiAMD1XlW5/10Du8
dM1lfVYXrovsuL5AyEkvKKJbROJGCQIhAO7ps9GymiW6ijW3CK7IytKlG1beafrm
s/J2wtTSzlkNMA0GCSqGSIb3DQEBCwUAA4IBAQCIAXF8QDv8X3XYfLtCoW8hNpa3
6aZ1fDD3uhOXDmTxJwv3DUpTWoyaDbY3uM7EDtvTZ/QndrYSdy9PQfokP975UwQV
xHjJPa7R5h00go/NXQ+hz75cqsmwnQuScxDJPCzhHDrfNW0Ekm3obuOdzg1ga6UH
LhsEXo4vR+N9YFyf6PoaCeVIRj8urFjrHIf+cW8CSI6PE4ctE7iD2wEUN6wuzcXS
ZHMRqV3sxODbQMf0gPCmzzcmlz/21e+LAv5dE0mpGI61l5MBkXLuZxB43UteaVcJ
F+/Qslu4a7HOBcXcSQ7++lRrkZGAU0j4/oU10JWS2NTs8/OHzMF7iJYEzhR5"), // Set your own root certificates (format CER)
    };

            if (certificateChain.ChainStatus.Any(status => status.Status != X509ChainStatusFlags.UntrustedRoot))
                return false;

            foreach (var element in certificateChain.ChainElements)
            {
                foreach (var status in element.ChainElementStatus)
                {
                    if (status.Status == X509ChainStatusFlags.UntrustedRoot)
                    {
                        // improvement: we could validate that the request matches an internal domain by using request.RequestUri in addition to the certicate validation

                        // Check that the root certificate matches one of the valid root certificates
                        if (validRootCertificates.Any(cert => cert.SequenceEqual(element.Certificate.RawData)))
                            continue; // Process the next status
                    }

                    return false;
                }
            }

            // Return true only if all certificates of the chain are valid
            return true;
        }
        #endregion
    }

    public enum EVerbType
    {
        GET = 1,
        POST = 2,
        PUT = 3,
        DELETE = 4
    }
}
