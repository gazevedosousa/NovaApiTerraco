using TerracoDaCida.Services.Interfaces;

namespace TerracoDaCida.Services
{
    public class InfoTokenUser : IInfoTokenUser
    {
        public InfoTokenUser(IHttpContextAccessor httpContextAccessor) 
        {
            var httpContext = httpContextAccessor.HttpContext;
            var claims = httpContext!.User.Claims;

            if (claims.Any(x => x.Type == "coUsuario"))
            {
                CoUsuario = Convert.ToInt32(claims.FirstOrDefault(x => x.Type == "coUsuario").Value);
            }
            if (claims.Any(x => x.Type == "coUsuario"))
            {
                NoUsuario = claims.FirstOrDefault(x => x.Type == "coUsuario").Value;
            }
            if (claims.Any(x => x.Type == "coUsuario"))
            {
                Admin = bool.Parse(claims.FirstOrDefault(x => x.Type == "admin").Value);
            }
        }
        public int CoUsuario { get; set; }
        public string NoUsuario { get; set; } = string.Empty;
        public bool Admin { get; set; }
    }
}
