using BibliotecaDanielSanchez83.Models.Domain;

namespace BibliotecaDanielSanchez83.Services.IServices
{
    public interface IRolServices
    {
        public List<Rol> ObtenerRoles();
        public bool CrearRol(Rol rol);
        public Task<bool> EditarRol(Rol rol);
        public bool EliminarRol(int id);
        public Rol GetRolesById(int id);
    }
}
