using System.Text.Json.Serialization;

namespace TerracoDaCida.DTO
{
    public class UsuarioDTO
    {
        [JsonPropertyName("usuario")]
        public string Usuario { get; set; } = string.Empty;
        [JsonPropertyName("perfil")]
        public string Perfil { get; set; } = string.Empty;
    }
}
