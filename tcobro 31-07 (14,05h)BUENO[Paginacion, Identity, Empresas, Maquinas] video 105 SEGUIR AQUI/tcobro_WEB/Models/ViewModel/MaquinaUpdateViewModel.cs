using Microsoft.AspNetCore.Mvc.Rendering;
using tcobro_WEB.Models.Dto;

namespace tcobro_WEB.Models.ViewModel
{
    public class MaquinaUpdateViewModel
    {

        public MaquinaUpdateViewModel()
        {
            Maquina = new MaquinaUpdateDTO();
        }

        public MaquinaUpdateDTO Maquina { get; set; } //Modelo de MaquinaCreateDTO

        public IEnumerable<SelectListItem> MaquinaList { get; set; }

    }
}
