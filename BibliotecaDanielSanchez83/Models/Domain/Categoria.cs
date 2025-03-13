using System.ComponentModel.DataAnnotations;

namespace BibliotecaDanielSanchez83.Models.Domain
{
    public class Categoria
    {
        [Key]
        public int PkCategoria { get; set; }

        [Required]
        public string Nombre { get; set; }

        // Relación con Libros (muchos a muchos)
        public ICollection<LibroCategoria> LibroCategorias { get; set; }
    }
}
