using BibliotecaDanielSanchez83.Context;
using BibliotecaDanielSanchez83.Models.Domain;
using BibliotecaDanielSanchez83.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaDanielSanchez83.Services.Services
{
    public class UsuarioServices : IUsuarioServices
    {
        private readonly ApplicationDbContext _context;
        public UsuarioServices( ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<Usuario> ValidateUser(string userName, string password)
        {
            var user = await _context.Usuarios
                .Include(u => u.Roles) // Asegurar que incluimos la relación con Roles
                .FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                return null;
            }

            var passwordHasher = new PasswordHasher<Usuario>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, password);

            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return user; 
        }

        public IEnumerable<Usuario> ObtenerUsuario()
        {
            try
            {
                return _context.Usuarios.Include(u => u.Roles).ToList(); 
            }
            catch (Exception ex)
            {
                throw new Exception("Sucedió un error al obtener los usuarios: " + ex.Message);
            }
        }


        public Usuario GetUsuarioId(int id) 
        {
            try
            {
                Usuario result = _context.Usuarios.Find(id);
                //Usuario res = _context.Usuarios.FirstOrDefault(x => x.PkUsuario == id);

                return result;
            }
            catch (Exception ex) 
            { 
                throw new Exception("Sucedio un error: " + ex.Message);
            }
        }

        public async Task<Usuario> GetByUserNameAsync(string userName)
        {
            try
            {
                var usuario = await _context.Usuarios.Include(u => u.Roles)
                                                     .FirstOrDefaultAsync(u => u.UserName == userName);

                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception("Sucedió un error al obtener el usuario: " + ex.Message);
            }
        }

        public bool CrearUsuario(Usuario request)
        {
            try
            {
                Usuario usuario = new Usuario()
                {
                    Nombre = request.Nombre,
                    UserName = request.UserName,
                    // Usa BCrypt para hashear la contraseña
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    FkRol = request.FkRol
                };

                _context.Usuarios.Add(usuario);
                int result = _context.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Sucedió un error: " + ex.Message);
            }
        }

        public async Task<bool> EditarUsuario(Usuario request)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(request.PkUsuario);
                if (usuario == null)
                {
                    return false;
                }

                usuario.Nombre = request.Nombre;
                usuario.UserName = request.UserName;
                usuario.FkRol = request.FkRol; 

                Console.WriteLine($" Nuevo rol asignado: {request.FkRol}");

                if (!string.IsNullOrEmpty(request.Password))
                {
                    var passwordHasher = new PasswordHasher<Usuario>();
                    usuario.Password = passwordHasher.HashPassword(usuario, request.Password);
                }

                _context.Entry(usuario).State = EntityState.Modified;
                int result = await _context.SaveChangesAsync();


                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error al editar usuario: {ex.Message}");
                throw new Exception("Sucedió un error: " + ex.Message);
            }
        }
        public bool EliminarUsuario(int id)
        {
            try
            {
                var usuario = _context.Usuarios.Find(id);
                if(usuario != null)
                {
                    _context.Usuarios.Remove(usuario);
                    int result = _context.SaveChanges();
                    if (result > 0)
                    { return false; }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Sucedió un error: " + ex.Message);
            }
        }

    }
}
