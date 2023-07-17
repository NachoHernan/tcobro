using tcobro_Utilidad;
using tcobro_WEB.Models.Dto;
using tcobro_WEB.Services.IServices;

namespace tcobro_WEB.Services
{
    public class UsuarioService : BaseService, IUsuarioService
    {
        public readonly IHttpClientFactory _httpClient; //Para realizar las conexiones
        private string _empresaUrl; //Ubicacion de URL de appsettings.json
        public UsuarioService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            _empresaUrl = configuration.GetValue<string>("ServiceUrls:API_URL");//Para poder acceder a la direccion de API(appsettings.json)
        }



        public Task<T> Login<T>(LoginRequestDTO loginRequestDTO)
        {
            return SendAsync<T>(new Models.APIRequest()
            {
                APITipo = DefinicionesEstaticas.APITipo.POST,
                Datos = loginRequestDTO, // Proviene de IUsuarioService
                Url = _empresaUrl + "/api/usuario/login"
            });
        }

        public Task<T> Registrar<T>(RegistroRequestDTO registroRequestDTO)
        {
            return SendAsync<T>(new Models.APIRequest()
            {
                APITipo = DefinicionesEstaticas.APITipo.POST,
                Datos = registroRequestDTO, // Proviene de IUsuarioService
                Url = _empresaUrl + "/api/usuario/registrar"
            });
        }
    }
}
