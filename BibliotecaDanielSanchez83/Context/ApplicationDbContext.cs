using BibliotecaDanielSanchez83.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaDanielSanchez83.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        // Modelos a mapear
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Autor> Autor { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<LibroCategoria> LibroCategorias { get; set; }  // Tabla de unión

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Libro>()
                .HasOne(l => l.Autor)
                .WithMany(a => a.Libros)
                .HasForeignKey(l => l.FkAutor);

            // Configuración de la relación de muchos a muchos entre Libro y Categoria
            modelBuilder.Entity<LibroCategoria>()
                .HasKey(lc => new { lc.PkLibro, lc.PkCategoria });

            modelBuilder.Entity<LibroCategoria>()
                .HasOne(lc => lc.Libro)
                .WithMany(l => l.LibroCategorias)
                .HasForeignKey(lc => lc.PkLibro);

            modelBuilder.Entity<LibroCategoria>()
                .HasOne(lc => lc.Categoria)
                .WithMany(c => c.LibroCategorias)
                .HasForeignKey(lc => lc.PkCategoria);

            // Seed de datos para Autor
            modelBuilder.Entity<Autor>().HasData(
                new Autor
                {
                    PkAutor = 1,
                    Nombre = "Gabriel García Márquez",
                    Biografia = "Escritor colombiano, ganador del Premio Nobel de Literatura en 1982."
                },
                new Autor
                {
                    PkAutor = 2,
                    Nombre = "J.K. Rowling",
                    Biografia = "Autora británica conocida por la serie de libros de Harry Potter."
                }
            );

            // Seed de datos para Libro
            modelBuilder.Entity<Libro>().HasData(
                new Libro
                {
                    PkLibro = 1,
                    Titulo = "Cien Años de Soledad",
                    FkAutor = 1,
                    FechaPublicacion = 1967,
                    Descripcion = "Una obra maestra de la literatura latinoamericana."
                },
                new Libro
                {
                    PkLibro = 2,
                    Titulo = "Harry Potter y la Piedra Filosofal",
                    FkAutor = 2,
                    FechaPublicacion = 1997,
                    Descripcion = "El primer libro de la saga de Harry Potter, un niño mago que descubre su verdadero destino."
                }
            );

            // Seed para Categorías
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { PkCategoria = 1, Nombre = "Ciencia Ficción" },
                new Categoria { PkCategoria = 2, Nombre = "Drama" },
                new Categoria { PkCategoria = 3, Nombre = "Fantasía" }
            );

            // Seed para la relación Libro-Categoría
            modelBuilder.Entity<LibroCategoria>().HasData(
                new LibroCategoria { PkLibro = 1, PkCategoria = 1 },
                new LibroCategoria { PkLibro = 1, PkCategoria = 2 },
                new LibroCategoria { PkLibro = 2, PkCategoria = 3 }
            );

            // Seed para Roles
            modelBuilder.Entity<Rol>().HasData(
                new Rol
                {
                    PkRol = 1,
                    Nombre = "Administrador"
                },
                new Rol
                {
                    PkRol = 2,
                    Nombre = "Usuario"
                }
            );

            // Seed para Usuarios
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    PkUsuario = 1,
                    Nombre = "Administrador",
                    UserName = "admin",
                    Password = "admin123", // Asegúrate de usar un hash en un entorno real
                    FkRol = 1 // Rol de Administrador
                },
                new Usuario
                {
                    PkUsuario = 2,
                    Nombre = "Usuario Normal",
                    UserName = "user1",
                    Password = "user123", // Asegúrate de usar un hash en un entorno real
                    FkRol = 2 // Rol de Usuario
                }
            );
        }
    }
}
