using Microsoft.AspNetCore.Mvc.Rendering;
using tcobro_WEB.Models.Dto;

namespace tcobro_WEB.Models.ViewModel
{
    public class MaquinaDeleteViewModel
    {

        public MaquinaDeleteViewModel()
        {
            Maquina = new MaquinaDTO();
        }

        public MaquinaDTO Maquina { get; set; } //Modelo de MaquinaCreateDTO

        public IEnumerable<SelectListItem> MaquinaList { get; set; }

    }
}
