using Azure.Core;
using BibliotecaDanielSanchez83.Context;
using BibliotecaDanielSanchez83.Models.Domain;
using BibliotecaDanielSanchez83.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaDanielSanchez83.Services.Services
{
    public class AutorServices : IAutorServices
    {
        private readonly ApplicationDbContext _context;

        public AutorServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Autor> ObtenerAutores()
        {
            return _context.Autor.ToList();
        }

        public Autor GetAutorById(int id)
        {
            return _context.Autor.Find(id);
        }

        public bool CrearAutor(Autor autor)
        {
            try
            {
                Autor autores = new Autor()
                {
                    Nombre = autor.Nombre,
                    Biografia = autor.Biografia,
                };

                _context.Autor.Add(autores);
                int result = _context.SaveChanges();
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el autor: " + ex.Message);
            }
        }

        public async Task<bool> EditarAutor(Autor autor)
        {
            try
            {
                var autores = await _context.Autor.FindAsync(autor.PkAutor);
                if (autores == null) return false;

                autores.Nombre = autor.Nombre;
                autores.Biografia = autor.Biografia;

                _context.Autor.Update(autores);
                int result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar el autor: " + ex.Message);
            }
        }

        public bool EliminarAutor(int id)
        {
            try
            {
                var autor = _context.Autor.Find(id);
                if (autor != null)
                {
                    _context.Autor.Remove(autor);
                    int result = _context.SaveChanges();
                    if (result > 0)
                    { return false; }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el autor: " + ex.Message);
            }
        }
    }
}
