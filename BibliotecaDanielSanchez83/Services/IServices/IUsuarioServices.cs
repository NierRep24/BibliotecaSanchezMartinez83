using BibliotecaDanielSanchez83.Models.Domain;

namespace BibliotecaDanielSanchez83.Services.IServices
{
    public interface IUsuarioServices
    {
        public IEnumerable<Usuario> ObtenerUsuario();
        public bool CrearUsuario(Usuario request);
        public Usuario GetUsuarioId(int id);
        public Task<Usuario> GetByUserNameAsync(string userName);
        public Task<bool> EditarUsuario(Usuario request);
        public bool EliminarUsuario(int id);
        public Task<Usuario> ValidateUser(string userName, string password);
    }
}
