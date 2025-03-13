using BibliotecaDanielSanchez83.Context;
using BibliotecaDanielSanchez83.Models.Domain;
using BibliotecaDanielSanchez83.Services.IServices;

namespace BibliotecaDanielSanchez83.Services.Services
{
    public class RolServices : IRolServices
    {
        private readonly ApplicationDbContext _context;

        public RolServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Rol> ObtenerRoles()
        {
            return _context.Roles.ToList();
        }

        public Rol GetRolesById(int id)
        {
            return _context.Roles.Find(id);
        }

        public bool CrearRol(Rol rol)
        {
            try
            {
                Rol roles = new Rol()
                {
                    Nombre = rol.Nombre
                };
                _context.Roles.Add(roles);
                int result = _context.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el rol: " + ex.Message);
            }

        }

        public async Task<bool> EditarRol(Rol rol)
        {
            try
            {
                var roles = await _context.Roles.FindAsync(rol.PkRol);
                if (roles != null)
                {
                    roles.Nombre = rol.Nombre;
                    int result = _context.SaveChanges();
                    return result > 0; 
                }
                return false; 
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar el rol: " + ex.Message);
            }
        }
        
        public bool EliminarRol(int id)
        {
            try
            {
                var rol = _context.Roles.Find(id);
                if (rol != null)
                {
                    _context.Roles.Remove(rol);
                    int result = _context.SaveChanges();
                    return result > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el rol: " + ex.Message);
            }
        }

    }
}
