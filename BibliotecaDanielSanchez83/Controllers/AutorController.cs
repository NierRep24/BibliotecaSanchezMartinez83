using BibliotecaDanielSanchez83.Models.Domain;
using BibliotecaDanielSanchez83.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace BibliotecaDanielSanchez83.Controllers
{
    public class AutorController : Controller
    {
        private readonly IAutorServices _autorServices;
        private readonly IUsuarioServices _usuarioServices;

        public AutorController(IAutorServices autorServices, IUsuarioServices usuarioServices)
        {
            _autorServices = autorServices;
            _usuarioServices = usuarioServices;
        }

        public IActionResult Index()
        {
            // Obtener el token desde las cookies
            var token = Request.Cookies["JwtToken"];

            // Decodificar el token para obtener el nombre de usuario y rol
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var userName = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var role = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            var usuarioAutenticado = _usuarioServices.GetByUserNameAsync(userName);

            switch (role)
            {
                case "Administrador":
                    var autores = _autorServices.ObtenerAutores();
                    return View(autores);

                case "Usuario":
                    return RedirectToAction("LibrosUser", "Libros");

                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Crear()
        {
            // Obtener el token desde las cookies
            var token = Request.Cookies["JwtToken"];

            // Decodificar el token para obtener el rol
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var role = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role != "Administrador")
            {
                return RedirectToAction("LibrosUser", "Home"); // Si no es administrador, redirige al Home
            }

            return View();
        }

        [HttpPost]
        public IActionResult Crear(Autor autor)
        {
            // Obtener el token desde las cookies
            var token = Request.Cookies["JwtToken"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home"); // Redirige si no hay token
            }

            // Decodificar el token para obtener el rol
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var role = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role != "Administrador")
            {
                return RedirectToAction("Index", "Home"); // Redirige si no es administrador
            }

                _autorServices.CrearAutor(autor);
                return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            // Obtener el token desde las cookies
            var token = Request.Cookies["JwtToken"];

            // Decodificar el token para obtener el rol
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var role = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role != "Administrador")
            {
                return RedirectToAction("Index", "Home"); // Si no es administrador, redirige al Home
            }

            var autor = _autorServices.GetAutorById(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Autor autor)
        {

            // Obtener el token desde las cookies
            var token = Request.Cookies["JwtToken"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home"); // Redirige si no hay token
            }

            // Decodificar el token para obtener el rol
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var role = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role != "Administrador")
            {
                return RedirectToAction("Index", "Home"); // Redirige si no es administrador
            }

            bool isUpdated = await _autorServices.EditarAutor(autor);
            if (isUpdated)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Hubo un error al actualizar el autor");
            return View(autor);
        }

        [HttpDelete]
        public IActionResult Eliminar(int id)
        {
            bool result = _autorServices.EliminarAutor(id);
            if (result == false)
            {
                return Json(new { succes = true });
            }
            else
            {
                return Json(new { succes = false });
            }
        }
    }
}
