using BibliotecaDanielSanchez83.Models.Domain;

namespace BibliotecaDanielSanchez83.Services.IServices
{
    public interface ICategoriaService
    {
        public List<Categoria> ObtenerCategoria();
        public Categoria ObtenerCategoriaByID(int id);
        public bool CrearCategoria(Categoria categoria);
    }
}
