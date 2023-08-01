namespace tcobro_Utilidad
{
    //Clase para guardado de datos y acceso rapido entre API y WEB
    public static class DefinicionesEstaticas
    {

        public enum APITipo
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        //Valor de Token guardado en la sesion 
        public static string SessionToken = "JWToken";

    }
}