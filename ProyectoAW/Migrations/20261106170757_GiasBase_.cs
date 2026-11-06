using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoAW.Migrations
{
    /// <inheritdoc />
    public partial class GiasBase_ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guias",
                columns: table => new
                {
                    IdGuia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroGuia = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IdTipoOperacionGuia = table.Column<int>(type: "int", nullable: false),
                    IdEstadoGuia = table.Column<int>(type: "int", nullable: false),
                    IdProveedor = table.Column<int>(type: "int", nullable: false),
                    FechaProgramada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guias", x => x.IdGuia);
                    table.ForeignKey(
                        name: "FK_Guias_EstadoGuias_IdEstadoGuia",
                        column: x => x.IdEstadoGuia,
                        principalTable: "EstadoGuias",
                        principalColumn: "IdEstadoGuia",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Guias_Proveedores_IdProveedor",
                        column: x => x.IdProveedor,
                        principalTable: "Proveedores",
                        principalColumn: "IdProveedor",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Guias_TipoOperacionGuias_IdTipoOperacionGuia",
                        column: x => x.IdTipoOperacionGuia,
                        principalTable: "TipoOperacionGuias",
                        principalColumn: "IdTipoOperacionGuia",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetallesGuia",
                columns: table => new
                {
                    IdDetalleGuia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdGuia = table.Column<int>(type: "int", nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesGuia", x => x.IdDetalleGuia);
                    table.ForeignKey(
                        name: "FK_DetallesGuia_Guias_IdGuia",
                        column: x => x.IdGuia,
                        principalTable: "Guias",
                        principalColumn: "IdGuia",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesGuia_Productos_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallesGuia_IdGuia",
                table: "DetallesGuia",
                column: "IdGuia");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesGuia_IdProducto",
                table: "DetallesGuia",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Guias_IdEstadoGuia",
                table: "Guias",
                column: "IdEstadoGuia");

            migrationBuilder.CreateIndex(
                name: "IX_Guias_IdProveedor",
                table: "Guias",
                column: "IdProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Guias_IdTipoOperacionGuia",
                table: "Guias",
                column: "IdTipoOperacionGuia");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesGuia");

            migrationBuilder.DropTable(
                name: "Guias");
        }
    }
}
