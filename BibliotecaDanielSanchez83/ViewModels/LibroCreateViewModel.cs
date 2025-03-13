namespace BibliotecaDanielSanchez83.ViewModels
{
    public class LibroCreateViewModel
    {
        public int PkLibro { get; set; }  // Campo necesario para la edición
        public string Titulo { get; set; }
        public int FkAutor { get; set; }
        public int FechaPublicacion { get; set; }
        public string Descripcion { get; set; }

        public List<int> CategoriaIds { get; set; }  // Lista de IDs de categorías
    }
}
