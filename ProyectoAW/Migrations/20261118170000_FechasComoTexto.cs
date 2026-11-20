using Microsoft.EntityFrameworkCore.Migrations;
using ProyectoAW.Infrastructure.Data;

#nullable disable

namespace ProyectoAW.Migrations;

public partial class FechasComoTexto : Migration
{
    private static readonly (string Table, string Column, bool Nullable, bool HasDefault)[] DateColumns =
    [
        ("Guias", "FechaActualizada", true, false),
        ("Guias", "FechaProgramada", false, false),
        ("Guias", "FechaRegistro", false, true),
        ("Almacenes", "FchModificacion", true, false),
        ("Almacenes", "FchRegistro", false, true),
        ("Clientes", "FchModificacion", true, false),
        ("Clientes", "FchRegistro", false, true),
        ("Proveedores", "FchModificacion", true, false),
        ("Proveedores", "FchRegistro", false, true),
        ("Ubicaciones", "FchModificacion", true, false),
        ("Ubicaciones", "FchRegistro", false, true),
        ("Productos", "FchModificacion", true, false),
        ("Productos", "FchRegistro", false, true),
        ("Usuarios", "FechaActualizada", true, false),
        ("Usuarios", "FechaRegistro", false, true),
        ("LecturaGuia", "FechaRegistroUtc", false, false),
        ("MovimientoStock", "FchMovimiento", false, false),
        ("StockAlmacen", "FechaUltimoRegitro", false, true),
        ("StockUbicacion", "FechaUltimoRegistro", false, true)
    ];

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_LecturaGuia_IdGuia_FechaRegistroUtc",
            table: "LecturaGuia");

        foreach (var column in DateColumns)
            ConvertToText(migrationBuilder, column);

        migrationBuilder.CreateIndex(
            name: "IX_LecturaGuia_IdGuia_FechaRegistroUtc",
            table: "LecturaGuia",
            columns: new[] { "IdGuia", "FechaRegistroUtc" });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_LecturaGuia_IdGuia_FechaRegistroUtc",
            table: "LecturaGuia");

        foreach (var column in DateColumns.Reverse())
            ConvertToDateTime(migrationBuilder, column);

        migrationBuilder.CreateIndex(
            name: "IX_LecturaGuia_IdGuia_FechaRegistroUtc",
            table: "LecturaGuia",
            columns: new[] { "IdGuia", "FechaRegistroUtc" });
    }

    private static void ConvertToText(
        MigrationBuilder migrationBuilder,
        (string Table, string Column, bool Nullable, bool HasDefault) item)
    {
        var (table, column, nullable, hasDefault) = item;
        var temporary = $"__{column}_text";
        var defaultConstraint = $"DF_{table}_{column}_Text";

        migrationBuilder.Sql($@"
DECLARE @constraint sysname;
SELECT @constraint = dc.name
FROM sys.default_constraints dc
JOIN sys.columns c ON c.object_id = dc.parent_object_id AND c.column_id = dc.parent_column_id
WHERE dc.parent_object_id = OBJECT_ID(N'[dbo].[{table}]') AND c.name = N'{column}';
IF @constraint IS NOT NULL
    EXEC(N'ALTER TABLE [dbo].[{table}] DROP CONSTRAINT [' + @constraint + N']');

ALTER TABLE [dbo].[{table}] ADD [{temporary}] nvarchar(16) NULL;");

        migrationBuilder.Sql($@"
UPDATE [dbo].[{table}]
SET [{temporary}] = LEFT(CONVERT(nvarchar(33), [{column}], 126), 16);");

        if (!nullable)
            migrationBuilder.Sql($"ALTER TABLE [dbo].[{table}] ALTER COLUMN [{temporary}] nvarchar(16) NOT NULL;");

        migrationBuilder.Sql($@"
ALTER TABLE [dbo].[{table}] DROP COLUMN [{column}];
EXEC sys.sp_rename N'[dbo].[{table}].[{temporary}]', N'{column}', N'COLUMN';");

        if (hasDefault)
            migrationBuilder.Sql($"ALTER TABLE [dbo].[{table}] ADD CONSTRAINT [{defaultConstraint}] DEFAULT (LEFT(CONVERT(varchar(19), GETDATE(), 126), 16)) FOR [{column}];");
    }

    private static void ConvertToDateTime(
        MigrationBuilder migrationBuilder,
        (string Table, string Column, bool Nullable, bool HasDefault) item)
    {
        var (table, column, nullable, hasDefault) = item;
        var temporary = $"__{column}_datetime";
        var defaultConstraint = $"DF_{table}_{column}_DateTime";

        migrationBuilder.Sql($@"
DECLARE @constraint sysname;
SELECT @constraint = dc.name
FROM sys.default_constraints dc
JOIN sys.columns c ON c.object_id = dc.parent_object_id AND c.column_id = dc.parent_column_id
WHERE dc.parent_object_id = OBJECT_ID(N'[dbo].[{table}]') AND c.name = N'{column}';
IF @constraint IS NOT NULL
    EXEC(N'ALTER TABLE [dbo].[{table}] DROP CONSTRAINT [' + @constraint + N']');

ALTER TABLE [dbo].[{table}] ADD [{temporary}] datetime2(7) NULL;");

        migrationBuilder.Sql($@"
UPDATE [dbo].[{table}]
SET [{temporary}] = TRY_CONVERT(datetime2(7), [{column}], 126);");

        if (!nullable)
            migrationBuilder.Sql($"ALTER TABLE [dbo].[{table}] ALTER COLUMN [{temporary}] datetime2(7) NOT NULL;");

        migrationBuilder.Sql($@"
ALTER TABLE [dbo].[{table}] DROP COLUMN [{column}];
EXEC sys.sp_rename N'[dbo].[{table}].[{temporary}]', N'{column}', N'COLUMN';");

        if (hasDefault)
            migrationBuilder.Sql($"ALTER TABLE [dbo].[{table}] ADD CONSTRAINT [{defaultConstraint}] DEFAULT (GETDATE()) FOR [{column}];");
    }
}
