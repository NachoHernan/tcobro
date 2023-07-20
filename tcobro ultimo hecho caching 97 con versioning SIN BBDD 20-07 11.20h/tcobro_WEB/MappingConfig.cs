using AutoMapper;
using tcobro_WEB.Models.Dto;

namespace tcobro_WEB
{
    //Agregar paquetes Automapper y Automapper DependencyInjection
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            //Empresa
            CreateMap<EmpresaDTO, EmpresaCreateDTO>().ReverseMap();
            CreateMap<EmpresaDTO, EmpresaUpdateDTO>().ReverseMap();
            //Maquina
            CreateMap<MaquinaDTO, MaquinaCreateDTO>().ReverseMap();
            CreateMap<MaquinaDTO, MaquinaUpdateDTO>().ReverseMap();

        }
    }
}
