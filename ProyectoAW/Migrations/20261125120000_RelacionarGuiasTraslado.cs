using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ProyectoAW.Infrastructure.Data;

#nullable disable

namespace ProyectoAW.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20261125120000_RelacionarGuiasTraslado")]
    public partial class RelacionarGuiasTraslado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE Guias SET IdGuiaRelacionada = GuiaRelacionadaIdGuia " +
                "WHERE IdGuiaRelacionada IS NULL AND GuiaRelacionadaIdGuia IS NOT NULL");

            migrationBuilder.DropForeignKey(
                name: "FK_Guias_Guias_GuiaRelacionadaIdGuia",
                table: "Guias");

            migrationBuilder.DropIndex(
                name: "IX_Guias_GuiaRelacionadaIdGuia",
                table: "Guias");

            migrationBuilder.DropColumn(
                name: "GuiaRelacionadaIdGuia",
                table: "Guias");

            migrationBuilder.AddForeignKey(
                name: "FK_Guias_Guias_IdGuiaRelacionada",
                table: "Guias",
                column: "IdGuiaRelacionada",
                principalTable: "Guias",
                principalColumn: "IdGuia",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guias_Guias_IdGuiaRelacionada",
                table: "Guias");

            migrationBuilder.AddColumn<int>(
                name: "GuiaRelacionadaIdGuia",
                table: "Guias",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Guias_GuiaRelacionadaIdGuia",
                table: "Guias",
                column: "GuiaRelacionadaIdGuia");

            migrationBuilder.AddForeignKey(
                name: "FK_Guias_Guias_GuiaRelacionadaIdGuia",
                table: "Guias",
                column: "GuiaRelacionadaIdGuia",
                principalTable: "Guias",
                principalColumn: "IdGuia");
        }
    }
}
