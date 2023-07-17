using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using tcobro_Utilidad;
using tcobro_WEB.Models;
using tcobro_WEB.Models.Dto;
using tcobro_WEB.Models.ViewModel;
using tcobro_WEB.Services.IServices;

namespace tcobro_WEB.Controllers
{
    public class MaquinaController : Controller
    {
        private readonly IMaquinaService _maquinaService;
        private readonly IEmpresaService _empresaService;
        private readonly IMapper _mapper;

        public MaquinaController(IMaquinaService maquinaService, IMapper mapper, IEmpresaService empresaService)
        {
            _maquinaService = maquinaService;
            _mapper = mapper;
            _empresaService = empresaService;
        }



        /*Carga la lista de Maquinas*/
        public async Task<IActionResult> IndexMaquina()
        {

            List<MaquinaDTO> maquinaList = new();

            var response = await _maquinaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DefinicionesEstaticas.SessionToken));

            if(response != null && response.IsExitoso)
            {
                maquinaList = JsonConvert.DeserializeObject<List<MaquinaDTO>>(Convert.ToString(response.Resultado));
            }

            return View(maquinaList);
        }



        /*Crear maquina*/
        public async Task<IActionResult> CrearMaquina()
        {
            //Enviar a la Vista la lista de Empresas para que se pueda elegir la empresa para la maquina

            //Llenar MaquinaViewModel
            MaquinaViewModel maquinaViewModel = new();

            var response = await _empresaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DefinicionesEstaticas.SessionToken));

            if(response != null && response.IsExitoso)
            {
                maquinaViewModel.MaquinaList = JsonConvert.DeserializeObject<List<EmpresaDTO>>(Convert.ToString(response.Resultado))
                                               .Select(e => new SelectListItem
                                               {
                                                   Text = e.Nombre,
                                                   Value = e.Id.ToString()
                                               });
            }

            return View(maquinaViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearMaquina(MaquinaViewModel modelo)
        {
            if(ModelState.IsValid)
            {
                var response = await _maquinaService.Crear<APIResponse>(modelo.Maquina, HttpContext.Session.GetString(DefinicionesEstaticas.SessionToken));

                if(response != null && response.IsExitoso)
                {
                    TempData["exitoso"] = "Maquina creada exitosamente"; //Datos en carpeta Shared
                    return RedirectToAction(nameof(IndexMaquina));
                }
                else
                {
                    if(response.ErrorMessages.Count > 0)
                    {
                        TempData["error"] = "Ha ocurrido un ERROR al crear maquina"; //Datos en carpeta Shared
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }

            //En caso de que algo falle se vuelve a cargar la lista de Empresas
            var res = await _empresaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DefinicionesEstaticas.SessionToken));

            if (res != null && res.IsExitoso)
            {
                modelo.MaquinaList = JsonConvert.DeserializeObject<List<EmpresaDTO>>(Convert.ToString(res.Resultado))
                                               .Select(e => new SelectListItem
                                               {
                                                   Text = e.Nombre,
                                                   Value = e.Id.ToString()
                                               });
            }

            return View(modelo);

        }

        /*Actualizar maquina*/
        public async Task<IActionResult> ActualizarMaquina(int Id)
        {
            MaquinaUpdateViewModel maquinaUpdateViewModel = new();

            var response = await _maquinaService.Obtener<APIResponse>(Id, HttpContext.Session.GetString(DefinicionesEstaticas.SessionToken));

            if(response != null && response.IsExitoso)
            {
                MaquinaDTO modelo = JsonConvert.DeserializeObject<MaquinaDTO>(Convert.ToString(response.Resultado));

                maquinaUpdateViewModel.Maquina = _mapper.Map<MaquinaUpdateDTO>(modelo);
            }

            response = await _empresaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DefinicionesEstaticas.SessionToken));

            if (response != null && response.IsExitoso)
            {
                maquinaUpdateViewModel.MaquinaList = JsonConvert.DeserializeObject<List<EmpresaDTO>>(Convert.ToString(response.Resultado))
                                               .Select(e => new SelectListItem
                                               {
                                                   Text = e.Nombre,
                                                   Value = e.Id.ToString()
                                               });
                return View(maquinaUpdateViewModel);
            }

             return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarMaquina(MaquinaUpdateViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                var response = await _maquinaService.Actualizar<APIResponse>(modelo.Maquina, HttpContext.Session.GetString(DefinicionesEstaticas.SessionToken));

                if (response != null && response.IsExitoso)
                {
                    TempData["exitoso"] = "Maquina actualizada exitosamente"; //Datos en carpeta Shared
                    return RedirectToAction(nameof(IndexMaquina));
                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        TempData["error"] = "Ha ocurrido un ERROR al actualizar maquina"; //Datos en carpeta Shared
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }

            //En caso de que algo falle se vuelve a cargar la lista de Empresas
            var res = await _empresaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DefinicionesEstaticas.SessionToken));

            if (res != null && res.IsExitoso)
            {
                modelo.MaquinaList = JsonConvert.DeserializeObject<List<EmpresaDTO>>(Convert.ToString(res.Resultado))
                                               .Select(e => new SelectListItem
                                               {
                                                   Text = e.Nombre,
                                                   Value = e.Id.ToString()
                                               });
            }

            return View(modelo);
        }

        /*Borrar maquina*/
        public async Task<IActionResult> RemoverMaquina(int Id)
        {
            MaquinaDeleteViewModel maquinaUpdateViewModel = new();

            var response = await _maquinaService.Obtener<APIResponse>(Id, HttpContext.Session.GetString(DefinicionesEstaticas.SessionToken));

            if (response != null && response.IsExitoso)
            {
                MaquinaDTO modelo = JsonConvert.DeserializeObject<MaquinaDTO>(Convert.ToString(response.Resultado));

                maquinaUpdateViewModel.Maquina =modelo;
            }

            response = await _empresaService.ObtenerTodos<APIResponse>(HttpContext.Session.GetString(DefinicionesEstaticas.SessionToken));

            if (response != null && response.IsExitoso)
            {
                maquinaUpdateViewModel.MaquinaList = JsonConvert.DeserializeObject<List<EmpresaDTO>>(Convert.ToString(response.Resultado))
                                               .Select(e => new SelectListItem
                                               {
                                                   Text = e.Nombre,
                                                   Value = e.Id.ToString()
                                               });
                return View(maquinaUpdateViewModel);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoverMaquina(MaquinaDeleteViewModel modelo)
        {
            var response = await _maquinaService.Remover<APIResponse>(modelo.Maquina.Id, HttpContext.Session.GetString(DefinicionesEstaticas.SessionToken));

            if(response != null && response.IsExitoso)
            {
                TempData["exitoso"] = "Maquina eliminada exitosamente"; //Datos en carpeta Shared
                return RedirectToAction(nameof(IndexMaquina));
            }
            TempData["error"] = "Ha ocurrido un ERROR al eliminar maquina"; //Datos en carpeta Shared
            return View(modelo);
        }

    }
}
