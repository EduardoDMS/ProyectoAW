using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoAW.Migrations
{
    /// <inheritdoc />
    public partial class LecturaGuia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LecturaGuia",
                columns: table => new
                {
                    IdLecturaGuia = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdGuia = table.Column<int>(type: "int", nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioOperador = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    CodigoLeido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Lote = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Serie = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Observacion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IdUbicacionOrigen = table.Column<int>(type: "int", nullable: true),
                    IdUbicacionDestino = table.Column<int>(type: "int", nullable: true),
                    FechaRegistroUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LecturaGuia", x => x.IdLecturaGuia);
                    table.CheckConstraint("CK_LecturaGuia_Cantidad", "Cantidad >= 0");
                    table.ForeignKey(
                        name: "FK_LecturaGuia_Guias_IdGuia",
                        column: x => x.IdGuia,
                        principalTable: "Guias",
                        principalColumn: "IdGuia",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LecturaGuia_Productos_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LecturaGuia_Ubicaciones_IdUbicacionDestino",
                        column: x => x.IdUbicacionDestino,
                        principalTable: "Ubicaciones",
                        principalColumn: "IdUbicacion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LecturaGuia_Ubicaciones_IdUbicacionOrigen",
                        column: x => x.IdUbicacionOrigen,
                        principalTable: "Ubicaciones",
                        principalColumn: "IdUbicacion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LecturaGuia_Usuarios_IdUsuarioOperador",
                        column: x => x.IdUsuarioOperador,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LecturaGuia_IdGuia_FechaRegistroUtc",
                table: "LecturaGuia",
                columns: new[] { "IdGuia", "FechaRegistroUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_LecturaGuia_IdProducto",
                table: "LecturaGuia",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_LecturaGuia_IdUbicacionDestino",
                table: "LecturaGuia",
                column: "IdUbicacionDestino");

            migrationBuilder.CreateIndex(
                name: "IX_LecturaGuia_IdUbicacionOrigen",
                table: "LecturaGuia",
                column: "IdUbicacionOrigen");

            migrationBuilder.CreateIndex(
                name: "IX_LecturaGuia_IdUsuarioOperador",
                table: "LecturaGuia",
                column: "IdUsuarioOperador");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LecturaGuia");
        }
    }
}
