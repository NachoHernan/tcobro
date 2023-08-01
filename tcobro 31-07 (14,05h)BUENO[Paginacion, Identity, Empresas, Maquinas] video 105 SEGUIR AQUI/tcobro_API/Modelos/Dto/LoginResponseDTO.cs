namespace tcobro_API.Modelos.Dto
{
    public class LoginResponseDTO
    {
        public UsuarioDTO Usuario { get; set; } // Regresa todos los datos del modelo Usuario
        public string Token { get; set; }    // Devuelve el Token
        public string Rol { get; set; }

    }
}
