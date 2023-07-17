namespace tcobro_API.Modelos.Dto
{
    public class LoginResponseDTO
    {
        public Usuario Usuario { get; set; } // Regresa todos los datos del modelo usuario
        public string Token { get; set; }    // Autenticar usuario

    }
}
