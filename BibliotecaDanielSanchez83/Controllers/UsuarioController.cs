using BibliotecaDanielSanchez83.Context;
using BibliotecaDanielSanchez83.Models.Domain;
using BibliotecaDanielSanchez83.Services.IServices;
using BibliotecaDanielSanchez83.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BibliotecaDanielSanchez83.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioServices _usuarioServices;
        private readonly IRolServices _rolServices;
        private readonly ApplicationDbContext _context;

        public UsuarioController(IUsuarioServices usuarioServices, IRolServices rolServices, ApplicationDbContext context)
        {
            _usuarioServices = usuarioServices;
            _rolServices = rolServices;
            _context = context;
        }

        /// <summary>
        /// Obtiene la lista de usuarios
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            // Obtener el token desde las cookies
            var token = Request.Cookies["JwtToken"];

            // Decodificar el token para obtener el rol
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var role = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Verificar el rol del usuario con un switch
            switch (role)
            {
                case "Administrador":
                    var usuarios = _usuarioServices.ObtenerUsuario();
                    return View(usuarios);

                case "Usuario":
                    return RedirectToAction("LibrosUser", "Libros"); // Redirigir a LibrosUser para el rol Usuario

                default:
                    return RedirectToAction("Index", "Home"); // Redirigir a Home si el rol no es válido
            }
        }

        /// <summary>
        /// Mostrar vista para crear un nuevo usuario
        /// </summary>
        [HttpGet]
        public IActionResult Crear()
        {
            // Obtener el token y decodificar el rol
            var token = Request.Cookies["JwtToken"];

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var role = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Verificar el rol
            switch (role)
            {
                case "Administrador":
                    var roles = _rolServices.ObtenerRoles();
                    ViewBag.Roles = new SelectList(roles, "PkRol", "Nombre");
                    return View();
                case "Usuario":
                    return RedirectToAction("LibrosUser", "Libros");
                default:
                    return RedirectToAction("Index", "Home");
            }

        }

        /// <summary>
        /// Procesa la creación de un nuevo usuario
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Crear(Usuario request)
        {
            // Obtener el token y decodificar el rol
            var token = Request.Cookies["JwtToken"];

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var role = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Verificar el rol
            if (role != "Administrador")
            {
                return RedirectToAction("Index", "Home"); // Si el rol no es Administrador, redirigir a Home
            }

            bool isCreated = _usuarioServices.CrearUsuario(request);

            if (isCreated)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "No se pudo crear el usuario.");
            }

            var roles = _rolServices.ObtenerRoles();
            ViewBag.Roles = new SelectList(roles, "PkRol", "Nombre");
            return View(request);
        }

        /// <summary>
        /// Editar usuario
        /// </summary>
        [HttpGet]
        public IActionResult Editar(int id)
        {
            // Obtener el token y decodificar el rol
            var token = Request.Cookies["JwtToken"];

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var role = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Verificar el rol
            if (role != "Administrador")
            {
                return RedirectToAction("Index", "Home"); // Si el rol no es Administrador, redirigir a Home
            }

            var usuario = _usuarioServices.GetUsuarioId(id);

            if (usuario == null)
            {
                return NotFound();
            }

            // Obtener los roles
            var roles = _rolServices.ObtenerRoles();

            // Crear la lista de roles para el dropdown
            ViewData["Roles"] = new SelectList(roles, "PkRol", "Nombre", usuario.FkRol);

            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, Usuario request)
        {
            // Obtener el token y decodificar el rol
            var token = Request.Cookies["JwtToken"];

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var role = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Verificar el rol
            if (role != "Administrador")
            {
                return RedirectToAction("Index", "Home"); // Si el rol no es Administrador, redirigir a Home
            }

            if (id != request.PkUsuario)
            {
                return BadRequest();
            }

            bool isUpdated = await _usuarioServices.EditarUsuario(request);

            if (isUpdated)
            {
                Console.WriteLine("Usuario editado correctamente");
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Hubo un error al actualizar el usuario.");

            // Recargar los roles para la vista
            var roles = _rolServices.ObtenerRoles();
            ViewBag.Roles = new SelectList(roles, "PkRol", "Nombre", request.FkRol);

            return View(request);
        }

        /// <summary>
        /// Eliminar usuario
        /// </summary>
        [HttpDelete]
        public IActionResult Eliminar(int id)
        {
            bool result = _usuarioServices.EliminarUsuario(id);
            if (result)
                return Ok(new { success = true });

            return BadRequest(new { success = false });
        }
    }

}
