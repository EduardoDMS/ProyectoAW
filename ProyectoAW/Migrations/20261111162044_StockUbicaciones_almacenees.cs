using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoAW.Migrations
{
    /// <inheritdoc />
    public partial class StockUbicaciones_almacenees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovimientoStock",
                columns: table => new
                {
                    IdMovimientoStock = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    IdAlmacen = table.Column<int>(type: "int", nullable: false),
                    IdTipoOperacion = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    FchMovimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdGuia = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioOpe = table.Column<int>(type: "int", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    CantidadDiferenciada = table.Column<int>(type: "int", nullable: false),
                    IdUbicacionOrigen = table.Column<int>(type: "int", nullable: true),
                    IdUbicacionDestino = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientoStock", x => x.IdMovimientoStock);
                    table.CheckConstraint("CK_MovimientoStock_Cantidad", "Cantidad >= 0");
                    table.ForeignKey(
                        name: "FK_MovimientoStock_Almacenes_IdAlmacen",
                        column: x => x.IdAlmacen,
                        principalTable: "Almacenes",
                        principalColumn: "IdAlmacen",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimientoStock_Guias_IdGuia",
                        column: x => x.IdGuia,
                        principalTable: "Guias",
                        principalColumn: "IdGuia",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimientoStock_Productos_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimientoStock_TipoOperacionGuias_IdTipoOperacion",
                        column: x => x.IdTipoOperacion,
                        principalTable: "TipoOperacionGuias",
                        principalColumn: "IdTipoOperacionGuia",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimientoStock_Ubicaciones_IdUbicacionDestino",
                        column: x => x.IdUbicacionDestino,
                        principalTable: "Ubicaciones",
                        principalColumn: "IdUbicacion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimientoStock_Ubicaciones_IdUbicacionOrigen",
                        column: x => x.IdUbicacionOrigen,
                        principalTable: "Ubicaciones",
                        principalColumn: "IdUbicacion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimientoStock_Usuarios_IdUsuarioOpe",
                        column: x => x.IdUsuarioOpe,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockAlmacen",
                columns: table => new
                {
                    IdStockAlmacen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAlmacen = table.Column<int>(type: "int", nullable: false),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    CantidadActual = table.Column<int>(type: "int", nullable: false),
                    FechaUltimoRegitro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAlmacen", x => x.IdStockAlmacen);
                    table.CheckConstraint("CK_StockAlmacen_CantidadActual", "CantidadActual >= 0");
                    table.ForeignKey(
                        name: "FK_StockAlmacen_Almacenes_IdAlmacen",
                        column: x => x.IdAlmacen,
                        principalTable: "Almacenes",
                        principalColumn: "IdAlmacen",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockAlmacen_Productos_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockUbicacion",
                columns: table => new
                {
                    IdStockUbicacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    IdUbicacion = table.Column<int>(type: "int", nullable: false),
                    CantidadActual = table.Column<int>(type: "int", nullable: false),
                    FechaUltimoRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockUbicacion", x => x.IdStockUbicacion);
                    table.CheckConstraint("CK_StockUbicacion_Cantidad", "CantidadActual >= 0");
                    table.ForeignKey(
                        name: "FK_StockUbicacion_Productos_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockUbicacion_Ubicaciones_IdUbicacion",
                        column: x => x.IdUbicacion,
                        principalTable: "Ubicaciones",
                        principalColumn: "IdUbicacion",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStock_IdAlmacen",
                table: "MovimientoStock",
                column: "IdAlmacen");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStock_IdGuia_IdProducto",
                table: "MovimientoStock",
                columns: new[] { "IdGuia", "IdProducto" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStock_IdProducto",
                table: "MovimientoStock",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStock_IdTipoOperacion",
                table: "MovimientoStock",
                column: "IdTipoOperacion");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStock_IdUbicacionDestino",
                table: "MovimientoStock",
                column: "IdUbicacionDestino");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStock_IdUbicacionOrigen",
                table: "MovimientoStock",
                column: "IdUbicacionOrigen");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoStock_IdUsuarioOpe",
                table: "MovimientoStock",
                column: "IdUsuarioOpe");

            migrationBuilder.CreateIndex(
                name: "IX_StockAlmacen_IdAlmacen_IdProducto",
                table: "StockAlmacen",
                columns: new[] { "IdAlmacen", "IdProducto" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockAlmacen_IdProducto",
                table: "StockAlmacen",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_StockUbicacion_IdProducto_IdUbicacion",
                table: "StockUbicacion",
                columns: new[] { "IdProducto", "IdUbicacion" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockUbicacion_IdUbicacion",
                table: "StockUbicacion",
                column: "IdUbicacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovimientoStock");

            migrationBuilder.DropTable(
                name: "StockAlmacen");

            migrationBuilder.DropTable(
                name: "StockUbicacion");
        }
    }
}
