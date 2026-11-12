using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProyectoAW.Migrations
{
    /// <inheritdoc />
    public partial class TipoGuiaComportamiento_Estado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallesGuia_Guias_IdGuia",
                table: "DetallesGuia");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallesGuia_Productos_IdProducto",
                table: "DetallesGuia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetallesGuia",
                table: "DetallesGuia");

            migrationBuilder.DeleteData(
                table: "TipoOperacionGuias",
                keyColumn: "IdTipoOperacionGuia",
                keyValue: 3);

            migrationBuilder.RenameTable(
                name: "DetallesGuia",
                newName: "DetalleGuia");

            migrationBuilder.RenameIndex(
                name: "IX_DetallesGuia_IdProducto",
                table: "DetalleGuia",
                newName: "IX_DetalleGuia_IdProducto");

            migrationBuilder.RenameIndex(
                name: "IX_DetallesGuia_IdGuia",
                table: "DetalleGuia",
                newName: "IX_DetalleGuia_IdGuia");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetalleGuia",
                table: "DetalleGuia",
                column: "IdDetalleGuia");

            migrationBuilder.CreateTable(
                name: "TiposComportamientoGuia",
                columns: table => new
                {
                    IdTipoComportamiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposComportamientoGuia", x => x.IdTipoComportamiento);
                });

            migrationBuilder.CreateTable(
                name: "MotivosGuia",
                columns: table => new
                {
                    IdMotivoGuia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTipoOperacionGuia = table.Column<int>(type: "int", nullable: false),
                    NombreMotivo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RequiereProveedor = table.Column<bool>(type: "bit", nullable: false),
                    RequiereCliente = table.Column<bool>(type: "bit", nullable: false),
                    RequiereAlmacenOrigen = table.Column<bool>(type: "bit", nullable: false),
                    RequiereAlmacenDestino = table.Column<bool>(type: "bit", nullable: false),
                    IdTipoComportamiento = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotivosGuia", x => x.IdMotivoGuia);
                    table.ForeignKey(
                        name: "FK_MotivosGuia_TipoOperacionGuias_IdTipoOperacionGuia",
                        column: x => x.IdTipoOperacionGuia,
                        principalTable: "TipoOperacionGuias",
                        principalColumn: "IdTipoOperacionGuia",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MotivosGuia_TiposComportamientoGuia_IdTipoComportamiento",
                        column: x => x.IdTipoComportamiento,
                        principalTable: "TiposComportamientoGuia",
                        principalColumn: "IdTipoComportamiento",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "TiposComportamientoGuia",
                columns: new[] { "IdTipoComportamiento", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Movimiento entre proveedor o cliente y un almacén.", "Movimiento normal" },
                    { 2, "Movimiento de existencias entre almacenes.", "Traslado interno" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MotivosGuia_IdTipoComportamiento",
                table: "MotivosGuia",
                column: "IdTipoComportamiento");

            migrationBuilder.CreateIndex(
                name: "IX_MotivosGuia_IdTipoOperacionGuia_NombreMotivo",
                table: "MotivosGuia",
                columns: new[] { "IdTipoOperacionGuia", "NombreMotivo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TiposComportamientoGuia_Nombre",
                table: "TiposComportamientoGuia",
                column: "Nombre",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleGuia_Guias_IdGuia",
                table: "DetalleGuia",
                column: "IdGuia",
                principalTable: "Guias",
                principalColumn: "IdGuia",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleGuia_Productos_IdProducto",
                table: "DetalleGuia",
                column: "IdProducto",
                principalTable: "Productos",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleGuia_Guias_IdGuia",
                table: "DetalleGuia");

            migrationBuilder.DropForeignKey(
                name: "FK_DetalleGuia_Productos_IdProducto",
                table: "DetalleGuia");

            migrationBuilder.DropTable(
                name: "MotivosGuia");

            migrationBuilder.DropTable(
                name: "TiposComportamientoGuia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetalleGuia",
                table: "DetalleGuia");

            migrationBuilder.RenameTable(
                name: "DetalleGuia",
                newName: "DetallesGuia");

            migrationBuilder.RenameIndex(
                name: "IX_DetalleGuia_IdProducto",
                table: "DetallesGuia",
                newName: "IX_DetallesGuia_IdProducto");

            migrationBuilder.RenameIndex(
                name: "IX_DetalleGuia_IdGuia",
                table: "DetallesGuia",
                newName: "IX_DetallesGuia_IdGuia");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetallesGuia",
                table: "DetallesGuia",
                column: "IdDetalleGuia");

            migrationBuilder.InsertData(
                table: "TipoOperacionGuias",
                columns: new[] { "IdTipoOperacionGuia", "Codigo", "Nombre" },
                values: new object[] { 3, "TRA", "Traslado" });

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesGuia_Guias_IdGuia",
                table: "DetallesGuia",
                column: "IdGuia",
                principalTable: "Guias",
                principalColumn: "IdGuia",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesGuia_Productos_IdProducto",
                table: "DetallesGuia",
                column: "IdProducto",
                principalTable: "Productos",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
