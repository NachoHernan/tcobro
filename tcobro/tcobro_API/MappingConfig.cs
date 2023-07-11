//Instalar paquete Automapper y AutoMapper.Extensions.Microsoft.DependencyInjection

//Mapper convierte un objeto a otro diferente

//Configuracion de Mapper:
using AutoMapper;
using tcobro_API.Modelos;
using tcobro_API.Modelos.Dto;

namespace tcobro_API
{
    //En esta clase se ponen todos los mapeos necesarios agregando el servicio en Program.cs
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            //Empresa
            CreateMap<Empresa, EmpresaDTO>().ReverseMap(); //Fuente , Destino //ReverseMap para hacer tambien el mapeo a la inversa
            CreateMap<Empresa, EmpresaCreateDTO>().ReverseMap();
            CreateMap<Empresa, EmpresaUpdateDTO>().ReverseMap();

            //Maquina
            CreateMap<Maquina, MaquinaDTO>().ReverseMap();
            CreateMap<Maquina, MaquinaCreateDTO>().ReverseMap();
            CreateMap<Maquina, MaquinaUpdateDTO>().ReverseMap();

        }
    }
}
