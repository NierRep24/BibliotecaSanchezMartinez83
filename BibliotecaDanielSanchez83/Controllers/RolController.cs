using BibliotecaDanielSanchez83.Models.Domain;
using BibliotecaDanielSanchez83.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace BibliotecaDanielSanchez83.Controllers
{
    public class RolController : Controller
    {
        private readonly IRolServices _rolServices;

        public RolController(IRolServices rolServices)
        {
            _rolServices = rolServices;
        }

        public IActionResult Index()
        {
            // Obtener el token desde las cookies
            var token = Request.Cookies["JwtToken"];

            // Decodificar el token para obtener el rol
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var role = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role != "Administrador")
            {
                return RedirectToAction("Index", "Home"); // Redirige si no es administrador
            }

            var roles = _rolServices.ObtenerRoles();
            return View(roles);
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
                return RedirectToAction("Index", "Home"); // Redirige si no es administrador
            }

            return View();
        }

        [HttpPost]
        public IActionResult Crear(Rol rol)
        {
            // Obtener el token desde las cookies
            var token = Request.Cookies["JwtToken"];

            // Decodificar el token para obtener el rol
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var role = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role != "Administrador")
            {
                return RedirectToAction("Index", "Home"); // Redirige si no es administrador
            }

            if (ModelState.IsValid)
            {
                _rolServices.CrearRol(rol);
                return RedirectToAction("Index");
            }

            return View(rol);
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
                return RedirectToAction("Index", "Home"); // Redirige si no es administrador
            }

            var rol = _rolServices.GetRolesById(id);
            if (rol == null)
            {
                return NotFound();
            }

            return View(rol);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Rol rol)
        {
            // Obtener el token desde las cookies
            var token = Request.Cookies["JwtToken"];

            // Decodificar el token para obtener el rol
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var role = jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role != "Administrador")
            {
                return RedirectToAction("Index", "Home"); // Redirige si no es administrador
            }

            if (ModelState.IsValid)
            {
                bool isUpdated = await _rolServices.EditarRol(rol);
                if (isUpdated)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(rol);
        }

        [HttpDelete]
        public IActionResult Eliminar(int id)
        {
            bool result = _rolServices.EliminarRol(id);
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
