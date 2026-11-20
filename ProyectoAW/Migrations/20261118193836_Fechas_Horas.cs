using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoAW.Migrations
{
    /// <inheritdoc />
    public partial class Fechas_Horas : Migration
    {
        private static readonly (string Table, string Column, bool HasDefault)[] DateColumns =
        [
            ("Guias", "FechaActualizada", false),
            ("Guias", "FechaProgramada", false),
            ("Guias", "FechaRegistro", true),
            ("Almacenes", "FchModificacion", false),
            ("Almacenes", "FchRegistro", true),
            ("Clientes", "FchModificacion", false),
            ("Clientes", "FchRegistro", true),
            ("Proveedores", "FchModificacion", false),
            ("Proveedores", "FchRegistro", true),
            ("Ubicaciones", "FchModificacion", false),
            ("Ubicaciones", "FchRegistro", true),
            ("Productos", "FchModificacion", false),
            ("Productos", "FchRegistro", true),
            ("Usuarios", "FechaActualizada", false),
            ("Usuarios", "FechaRegistro", true),
            ("LecturaGuia", "FechaRegistroUtc", false),
            ("MovimientoStock", "FchMovimiento", false),
            ("StockAlmacen", "FechaUltimoRegitro", true),
            ("StockUbicacion", "FechaUltimoRegistro", true)
        ];

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            foreach (var dateColumn in DateColumns)
                SetSeparator(migrationBuilder, dateColumn, toSpace: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            foreach (var dateColumn in DateColumns.Reverse())
                SetSeparator(migrationBuilder, dateColumn, toSpace: false);
        }

        private static void SetSeparator(
            MigrationBuilder migrationBuilder,
            (string Table, string Column, bool HasDefault) item,
            bool toSpace)
        {
            var (table, column, hasDefault) = item;
            var from = toSpace ? "T" : " ";
            var to = toSpace ? " " : "T";

            migrationBuilder.Sql($@"
UPDATE [dbo].[{table}]
SET [{column}] = REPLACE([{column}], N'{from}', N'{to}')
WHERE [{column}] LIKE N'%{from}%';");

            if (!hasDefault)
                return;

            var constraint = toSpace
                ? $"DF_{table}_{column}_Space"
                : $"DF_{table}_{column}_Text";
            var style = toSpace ? 120 : 126;

            migrationBuilder.Sql($@"
DECLARE @constraint sysname;
SELECT @constraint = dc.name
FROM sys.default_constraints dc
JOIN sys.columns c ON c.object_id = dc.parent_object_id AND c.column_id = dc.parent_column_id
WHERE dc.parent_object_id = OBJECT_ID(N'[dbo].[{table}]') AND c.name = N'{column}';
IF @constraint IS NOT NULL
    EXEC(N'ALTER TABLE [dbo].[{table}] DROP CONSTRAINT [' + @constraint + N']');
ALTER TABLE [dbo].[{table}] ADD CONSTRAINT [{constraint}]
    DEFAULT (LEFT(CONVERT(varchar(19), GETDATE(), {style}), 16)) FOR [{column}];");
        }
    }
}
