using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using tcobro_Utilidad;
using tcobro_WEB.Models;
using tcobro_WEB.Models.Dto;
using tcobro_WEB.Models.ViewModel;
using tcobro_WEB.Services.IServices;

namespace tcobro_WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmpresaService _empresaService;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IEmpresaService empresaService, IMapper mapper)
        {
            _logger = logger;
            _empresaService = empresaService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int numeroDePagina = 1)
        {
            List<EmpresaDTO> empresaList = new();
            EmpresaPaginadoViewModel empresaPaginadoViewModel = new EmpresaPaginadoViewModel();

            if (numeroDePagina < 1) numeroDePagina = 1;//No permite que el numero de pagina sea menor que 1

            var response = await _empresaService.ObtenerTodosPaginado<APIResponse>(HttpContext.Session.GetString(DefinicionesEstaticas.SessionToken), numeroDePagina, 4);

            if (response != null && response.IsExitoso)
            {
                empresaList = JsonConvert.DeserializeObject<List<EmpresaDTO>>(Convert.ToString(response.Resultado));

                //Llenado de EmpresaPaginadoViewModel

                empresaPaginadoViewModel = new EmpresaPaginadoViewModel()
                {
                    EmpresaList = empresaList,
                    NumeroDePagina = numeroDePagina,
                    PaginasTotales = JsonConvert.DeserializeObject<int>(Convert.ToString(response.PaginasTotales))
                };
                                
                //Validacion de Botones de Pagina
                if (numeroDePagina > 1) empresaPaginadoViewModel.Anterior = "";
                if (empresaPaginadoViewModel.PaginasTotales <= numeroDePagina) empresaPaginadoViewModel.Siguiente = "disabled";
            }

            return View(empresaPaginadoViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}