using BibliotecaDanielSanchez83.Models.Domain;

namespace BibliotecaDanielSanchez83.Services.IServices
{
    public interface IJwtService
    {
        public string GenerateToken(Usuario usuario);
    }
}
