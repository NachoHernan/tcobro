namespace tcobro_API.Modelos.Especificaciones
{
    public class PagedList<T> :List<T>
    {
        public MetaData MetaData { get; set; } //Trae las propiedades de MetaData


        public PagedList(List<T>items, int total, int numeroDePagina, int registrosPorPagina)
        {
            MetaData = new MetaData
            {
                RegistrosTotales = total,
                RegistrosPorPagina = registrosPorPagina,
                PaginasTotales = (int)Math.Ceiling(total / (double)registrosPorPagina) //Hace que el numero de pagina no pueda tener decimales
            };
            AddRange(items);
        }

        public static PagedList<T> ToPagedList(IEnumerable<T> entidad, int numeroDePagina, int registrosPorPagina)
        {            
            var total = entidad.Count(); //Cuenta la cantidad de registros

            total = entidad.Count();
                        
            var items = entidad.Skip((numeroDePagina -1) * registrosPorPagina)//Salto de pagina segun los registros
                               .Take(registrosPorPagina).ToList();//Obtiene la pagina con los registros 

            return new PagedList<T>(items, total, numeroDePagina, registrosPorPagina);
        }

    }
}
