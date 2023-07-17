using Microsoft.AspNetCore.Mvc.Rendering;
using tcobro_WEB.Models.Dto;

namespace tcobro_WEB.Models.ViewModel
{
    public class MaquinaViewModel
    {

        public MaquinaViewModel()
        {
            Maquina = new MaquinaCreateDTO();
        }

        public MaquinaCreateDTO Maquina { get; set; } //Modelo de MaquinaCreateDTO

        public IEnumerable<SelectListItem> MaquinaList { get; set; }

    }
}
