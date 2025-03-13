using BibliotecaDanielSanchez83.Context;
using BibliotecaDanielSanchez83.Models.Domain;
using BibliotecaDanielSanchez83.Services.IServices;

namespace BibliotecaDanielSanchez83.Services.Services
{
    public class CategoriaService : ICategoriaService
    {
        public readonly ApplicationDbContext _context;

        public CategoriaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Categoria> ObtenerCategoria() 
        {
            return _context.Categorias.ToList();
        }
        public Categoria ObtenerCategoriaByID(int id) 
        {
            return _context.Categorias.Find(id);
        }

        public bool CrearCategoria(Categoria categoria) 
        {
            try
            {
                Categoria categoria1 = new Categoria()
                { Nombre = categoria.Nombre };

                _context.Categorias.Add(categoria1);
                int result = _context.SaveChanges();
                if (result > 0) 
                { return true; } return false;
            }
            catch (Exception ex) 
            {
                throw new Exception("Error al crear el autor: " + ex.Message);
            }
        }

        

    }
}
