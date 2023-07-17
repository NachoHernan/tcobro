﻿using tcobro_Utilidad;
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


        public Task<T> Actualizar<T>(EmpresaUpdateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DefinicionesEstaticas.APITipo.PUT, //Define el metodo a ejecutar
                Datos = dto, //Recoge los datos del dto que le estamos pasando
                Url = _empresaUrl + "/api/Empresa/" + dto.Id //Se conecta con la API
            });
        }

        public Task<T> Crear<T>(EmpresaCreateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DefinicionesEstaticas.APITipo.POST, 
                Datos = dto, 
                Url = _empresaUrl + "/api/Empresa" 
            });
        }

        public Task<T> Obtener<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DefinicionesEstaticas.APITipo.GET, 
                Url = _empresaUrl + "/api/Empresa/" + id 
            });
        }

        public Task<T> ObtenerTodos<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DefinicionesEstaticas.APITipo.GET,                 
                Url = _empresaUrl + "/api/Empresa" 
            });
        }

        public Task<T> Remover<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DefinicionesEstaticas.APITipo.DELETE,
                Url = _empresaUrl + "/api/Empresa/"+ id
            });
        }
    }
}