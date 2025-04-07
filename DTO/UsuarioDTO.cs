using System.Text.Json.Serialization;

namespace TerracoDaCida.DTO
{
    public class UsuarioDTO
    {
        [JsonPropertyName("codigo")]
        public int Codigo { get; set; }
        [JsonPropertyName("usuario")]
        public string Usuario { get; set; } = string.Empty;
        [JsonPropertyName("perfil")]
        public string Perfil { get; set; } = string.Empty;
    }
}
