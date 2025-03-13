using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BibliotecaDanielSanchez83.Migrations
{
    /// <inheritdoc />
    public partial class AddFechaInsercion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Autor",
                columns: table => new
                {
                    PkAutor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Biografia = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autor", x => x.PkAutor);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    PkCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.PkCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    PkRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.PkRol);
                });

            migrationBuilder.CreateTable(
                name: "Libros",
                columns: table => new
                {
                    PkLibro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FkAutor = table.Column<int>(type: "int", nullable: false),
                    FechaPublicacion = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libros", x => x.PkLibro);
                    table.ForeignKey(
                        name: "FK_Libros_Autor_FkAutor",
                        column: x => x.FkAutor,
                        principalTable: "Autor",
                        principalColumn: "PkAutor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    PkUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FkRol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.PkUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_FkRol",
                        column: x => x.FkRol,
                        principalTable: "Roles",
                        principalColumn: "PkRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibroCategorias",
                columns: table => new
                {
                    PkLibro = table.Column<int>(type: "int", nullable: false),
                    PkCategoria = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibroCategorias", x => new { x.PkLibro, x.PkCategoria });
                    table.ForeignKey(
                        name: "FK_LibroCategorias_Categorias_PkCategoria",
                        column: x => x.PkCategoria,
                        principalTable: "Categorias",
                        principalColumn: "PkCategoria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibroCategorias_Libros_PkLibro",
                        column: x => x.PkLibro,
                        principalTable: "Libros",
                        principalColumn: "PkLibro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Autor",
                columns: new[] { "PkAutor", "Biografia", "Nombre" },
                values: new object[,]
                {
                    { 1, "Escritor colombiano, ganador del Premio Nobel de Literatura en 1982.", "Gabriel García Márquez" },
                    { 2, "Autora británica conocida por la serie de libros de Harry Potter.", "J.K. Rowling" }
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "PkCategoria", "Nombre" },
                values: new object[,]
                {
                    { 1, "Ciencia Ficción" },
                    { 2, "Drama" },
                    { 3, "Fantasía" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "PkRol", "Nombre" },
                values: new object[,]
                {
                    { 1, "Administrador" },
                    { 2, "Usuario" }
                });

            migrationBuilder.InsertData(
                table: "Libros",
                columns: new[] { "PkLibro", "Descripcion", "Fecha", "FechaPublicacion", "FkAutor", "Titulo" },
                values: new object[,]
                {
                    { 1, "Una obra maestra de la literatura latinoamericana.", new DateTime(2025, 3, 11, 18, 59, 32, 16, DateTimeKind.Utc).AddTicks(9210), 1967, 1, "Cien Años de Soledad" },
                    { 2, "El primer libro de la saga de Harry Potter, un niño mago que descubre su verdadero destino.", new DateTime(2025, 3, 11, 18, 59, 32, 16, DateTimeKind.Utc).AddTicks(9214), 1997, 2, "Harry Potter y la Piedra Filosofal" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "PkUsuario", "FkRol", "Nombre", "Password", "UserName" },
                values: new object[,]
                {
                    { 1, 1, "Administrador", "admin123", "admin" },
                    { 2, 2, "Usuario Normal", "user123", "user1" }
                });

            migrationBuilder.InsertData(
                table: "LibroCategorias",
                columns: new[] { "PkCategoria", "PkLibro" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LibroCategorias_PkCategoria",
                table: "LibroCategorias",
                column: "PkCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_FkAutor",
                table: "Libros",
                column: "FkAutor");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_FkRol",
                table: "Usuarios",
                column: "FkRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibroCategorias");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Libros");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Autor");
        }
    }
}
