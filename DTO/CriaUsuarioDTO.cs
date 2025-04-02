using System.Text.Json.Serialization;

namespace TerracoDaCida.DTO
{
    public class CriaUsuarioDTO
    {
        [JsonPropertyName("usuario")]
        public string Usuario { get; set; } = string.Empty;
        [JsonPropertyName("perfil")]
        public short Perfil { get; set; }
        [JsonPropertyName("senha")]
        public string Senha { get; set; } = string.Empty;
    }
}
