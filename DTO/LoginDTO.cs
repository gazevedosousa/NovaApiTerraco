using System.Text.Json.Serialization;

namespace TerracoDaCida.DTO
{
    public class LoginDTO
    {
        [JsonPropertyName("usuario")]
        public string Usuario { get; set; } = string.Empty;

        [JsonPropertyName("senha")]
        public string Senha { get; set; } = string.Empty;
    }
}
