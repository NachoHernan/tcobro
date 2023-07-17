using NuGet.Common;
using tcobro_Utilidad;
using tcobro_WEB.Models;
using tcobro_WEB.Models.Dto;
using tcobro_WEB.Services.IServices;

namespace tcobro_WEB.Services
{
    public class EmpresaService : BaseService, IEmpresaService
    {
        //Servicio para Empresa que contiene las peticiones a manejar por controlador

        public readonly IHttpClientFactory _httpClient;
        private string _empresaUrl;
        public EmpresaService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient; //Servicio para envio de solicitudes HTTP, configuraciones aplicadas a todas las solicitudes ejecutadas
            _empresaUrl = configuration.GetValue<string>("ServiceUrls:API_URL"); //Obtiene la URL de la API en appsetting.json
        }

        
        public Task<T> Actualizar<T>(EmpresaUpdateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DefinicionesEstaticas.APITipo.PUT, //Define el metodo a ejecutar
                Datos = dto, //Recoge los datos del dto que le estamos pasando
                Url = _empresaUrl + "/api/Empresa/" + dto.Id, //Se conecta con la API
                Token = token //Recoge el token para mantener la sesion
            });
        }

        public Task<T> Crear<T>(EmpresaCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DefinicionesEstaticas.APITipo.POST, 
                Datos = dto, 
                Url = _empresaUrl + "/api/Empresa",
                Token = token

            });
        }

        public Task<T> Obtener<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DefinicionesEstaticas.APITipo.GET, 
                Url = _empresaUrl + "/api/Empresa/" + id,
                Token = token
            });
        }

        public Task<T> ObtenerTodos<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DefinicionesEstaticas.APITipo.GET,                 
                Url = _empresaUrl + "/api/Empresa",
                Token = token
            });
        }

        public Task<T> Remover<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DefinicionesEstaticas.APITipo.DELETE,
                Url = _empresaUrl + "/api/Empresa/"+ id,
                Token = token
            });
        }
    }
}
