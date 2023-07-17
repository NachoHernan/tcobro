using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using tcobro_WEB.Models;
using tcobro_WEB.Models.Dto;
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

        public async Task<IActionResult> Index()
        {
            List<EmpresaDTO> empresaList = new();

            var response = await _empresaService.ObtenerTodos<APIResponse>();

            if(response != null && response.IsExitoso)
            {
                empresaList = JsonConvert.DeserializeObject<List<EmpresaDTO>>(Convert.ToString(response.Resultado));
            }

            return View(empresaList);
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