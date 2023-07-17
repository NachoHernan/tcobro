using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using tcobro_Utilidad;
using tcobro_WEB.Models;
using tcobro_WEB.Services.IServices;

namespace tcobro_WEB.Services
{
    //Servicio base generico centralizado necesario para conectarnos con la API para la utilizacion de todas las llamadas (GET,POST,PUT,DELETE,PATCH)
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory _httpClient { get; set; } //Servicio para envio de solicitudes HTTP, configuraciones aplicadas a todas las solicitudes ejecutadas

        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            _httpClient = httpClient;
        }



        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = _httpClient.CreateClient("tcobroAPI"); //Necesario crear httpClient con un nombre como ("tcobroAPI")
                HttpRequestMessage message = new HttpRequestMessage(); // Mensaje HTTP

                //Configuracion de message:
                message.Headers.Add("Accept", "application/json"); // El mensaje necesita trabajar con requisitos que necesiten archivos JSON
                message.RequestUri = new Uri(apiRequest.Url); //Obtiene la URL por la cual nos queremos conectar

                if (apiRequest.Datos != null)//Si es diferente de null se trata de un POST o PUT que son los que envian datos
                {
                    //Necesidad de enviar el contenido serializado(JSON) y descomprimirlo a un formato de texto facil
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Datos),
                                Encoding.UTF8, "application/json");
                }

                switch (apiRequest.APITipo)
                {
                    case DefinicionesEstaticas.APITipo.POST:
                        message.Method = HttpMethod.Post;
                        break;

                    case DefinicionesEstaticas.APITipo.PUT:
                        message.Method = HttpMethod.Put;
                        break;

                    case DefinicionesEstaticas.APITipo.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;


                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                //Invocacion de servicio enviando la solicitud
                HttpResponseMessage apiResponse = null;

                //Recibe el token para autorizacion y muestra de web
                if(!string.IsNullOrEmpty(apiRequest.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);
                }

                apiResponse = await client.SendAsync(message);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();//Almacena el contenido de la respuesta


                try
                {
                    APIResponse response = JsonConvert.DeserializeObject<APIResponse>(apiContent);

                    if(apiResponse.StatusCode == HttpStatusCode.BadRequest || apiResponse.StatusCode == HttpStatusCode.NotFound)
                    {
                        response.statusCode = HttpStatusCode.BadRequest;
                        response.IsExitoso = false;
                        
                        var res = JsonConvert.SerializeObject(response);
                        var obj = JsonConvert.DeserializeObject<T>(res);

                        return obj;
                    }

                }
                catch (Exception ex)
                {
                    var errorResponse = JsonConvert.DeserializeObject<T>(apiContent);

                    return errorResponse;
                }

                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent); 

                return APIResponse;

            }
            catch (Exception ex)
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsExitoso = false
                };

                var res = JsonConvert.SerializeObject(dto); //Serializa el DTO
                var responseEx = JsonConvert.DeserializeObject<T>(res);//Deserializa la variable 'res'

                return responseEx; //Devuelve los errores
            }
        }
    }
}
