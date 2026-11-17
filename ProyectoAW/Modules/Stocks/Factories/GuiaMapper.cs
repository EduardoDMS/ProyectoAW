using ProyectoAW.Modules.Guias.DTOs.Responses;
using ProyectoAW.Modules.Guias.Entities;
using ProyectoAW.Modules.Maestros.Almacenes.Entities;
using ProyectoAW.Modules.Maestros.Operaciones.Entities;
using ProyectoAW.Modules.Seguridad.Usuarios.Entitites;
using ProyectoAW.Modules.Stocks.Dtos.Response;
using ProyectoAW.Modules.Stocks.Entities;
using ProyectoAW.Modules.Stocks.Services;

namespace ProyectoAW.Modules.Stocks.Factories
{
    public class GuiaMapper
    {
      public static LecturaGuiaDto ToLecturaDto(LecturaGuia lectura) => new(
      IdLecturaGuia: lectura.IdLecturaGuia,
      IdGuia: lectura.IdGuia,
      IdProducto: lectura.IdProducto,
      CodigoProducto: lectura.Producto?.Codigo ?? lectura.CodigoLeido,
      DescripcionProducto: lectura.Producto?.Nombre ?? string.Empty,
      CodigoLeido: lectura.CodigoLeido,
      Cantidad: lectura.Cantidad,
      Lote: lectura.Lote,
      Serie: lectura.Serie,
      Observacion: lectura.Observacion,
      IdUbicacionOrigen: lectura.IdUbicacionOrigen,
      UbicacionOrigen: lectura.UbicacionOrigen?.Codigo,
      IdUbicacionDestino: lectura.IdUbicacionDestino,
      UbicacionDestino: lectura.UbicacionDestino?.Codigo,
      FechaRegistroUtc: lectura.FechaRegistroUtc);


        public static GuiaOperadorDto ToGuiaOperadorDto(Guia guia) => new(
            IdGuia: guia.IdGuia,
            NumeroGuia: guia.NumeroGuia,
            IdTipoOperacionGuia: guia.IdTipoOperacionGuia,
            IdMotivoGuia: guia.IdMotivoGuia,
            IdTipoComportamiento: guia.Motivo?.IdTipoComportamiento,
            NombreMotivo: guia.Motivo?.NombreMotivo,
            RequiereProveedor: RequiereProveedor(guia),
            RequiereCliente: RequiereCliente(guia),
            RequiereAlmacenOrigen: RequiereAlmacenOrigen(guia),
            RequiereAlmacenDestino: RequiereAlmacenDestino(guia),
            TipoTercero: TipoTercero(guia),
            NombreTercero: NombreTercero(guia),
            NombreAlmacenOrigen: NombreAlmacen(guia.AlmacenOrigen),
            NombreAlmacenDestino: NombreAlmacen(guia.AlmacenDestino),
            IdGuiaRelacionada: guia.IdGuiaRelacionada,
            NumeroGuiaRelacionada: guia.GuiaRelacionada?.NumeroGuia,
            IdEstadoGuia: guia.IdEstadoGuia,
            IdProveedor: guia.IdProveedor,
            IdCliente: guia.IdCliente,
            IdAlmacenOrigen: guia.IdAlmacenOrigen,
            IdAlmacenDestino: guia.IdAlmacenDestino,
            FechaProgramada: guia.FechaProgramada,
            FechaRegistro: guia.FechaRegistro,
            FechaActualizada: guia.FechaActualizada,
            TipoDocumentoReferencia: guia.TipoDocumentoReferencia,
            NumeroDocumentoReferencia: guia.NumeroDocumentoReferencia,
            Observacion: guia.Observacion,
                Detalles: guia.Detalles
                .Select(detalle => new DetalleGuiaResponse(
                        detalle.IdDetalleGuia, detalle.IdProducto, detalle.Cantidad))
                .ToList());

        public static MovimientoStockDto ToMovimientoDto(MovimientoStock movimiento) => new(
        IdMovimientoStock: movimiento.IdMovimientoStock,
        IdGuia: movimiento.IdGuia,
        NumeroGuia: movimiento.Guia?.NumeroGuia ?? string.Empty,
        MotivoGuia: movimiento.Guia?.Motivo?.NombreMotivo ?? "—",
        IdTipoOperacion: movimiento.IdTipoOperacion,
        Operacion: movimiento.OperacionGuias?.Nombre ?? string.Empty,
        IdProducto: movimiento.IdProducto,
        CodigoProducto: movimiento.Producto?.Codigo ?? string.Empty,
        Producto: movimiento.Producto?.Nombre ?? string.Empty,
        IdAlmacen: movimiento.IdAlmacen,
        Origen: NombreOrigen(movimiento),
        Destino: NombreDestino(movimiento),
        CantidadEsperada: movimiento.Guia?.Detalles
            .FirstOrDefault(detalle => detalle.IdProducto == movimiento.IdProducto)?.Cantidad ?? 0,
        Cantidad: movimiento.Cantidad,
        CantidadDiferenciada: movimiento.CantidadDiferenciada,
        Observacion: movimiento.Observacion,
        IdUsuarioOperador: movimiento.IdUsuarioOperador,
        Operador: NombreOperador(movimiento.Usuario),
        IdUbicacionOrigen: movimiento.IdUbicacionOrigen,
        UbicacionOrigen: movimiento.UbicacionOrigen?.Codigo ?? string.Empty,
        IdUbicacionDestino: movimiento.IdUbicacionDestino,
        UbicacionDestino: movimiento.UbicacionDestino?.Codigo ?? string.Empty,
        FchMovimiento: movimiento.FchMovimiento);



        private static bool RequiereProveedor(Guia g) => g.Motivo?.RequiereProveedor ?? g.IdTipoOperacionGuia == TipoOperacionGuiaIds.Entrada;
        private static bool RequiereCliente(Guia g) => g.Motivo?.RequiereCliente ?? g.IdTipoOperacionGuia == TipoOperacionGuiaIds.Salida;
        private static bool RequiereAlmacenOrigen(Guia g) => g.Motivo?.RequiereAlmacenOrigen ?? g.IdTipoOperacionGuia is TipoOperacionGuiaIds.Salida;
        private static bool RequiereAlmacenDestino(Guia g) => g.Motivo?.RequiereAlmacenDestino ?? g.IdTipoOperacionGuia is TipoOperacionGuiaIds.Entrada;
        private static string? TipoTercero(Guia g) => RequiereProveedor(g) ? "Proveedor" : RequiereCliente(g) ? "Cliente" : null;
        private static string? NombreTercero(Guia g) => RequiereProveedor(g) ? g.Proveedor?.RazonSocial : RequiereCliente(g) ? g.Cliente?.RazonSocial : null;
        private static string? NombreAlmacen(Almacen? almacen) => almacen is null ? null : string.Join(" · ", new[] { almacen.Codigo, almacen.Nombre }
                          .Where(x => !string.IsNullOrWhiteSpace(x)));

        private static string NombreOperador(Usuario? usuario) =>
                usuario is null ? string.Empty : $"{usuario.Nombre} {usuario.Apellido}".Trim();

        private static string NombreOrigen(MovimientoStock movimiento) =>
            movimiento.IdTipoOperacion == TipoOperacionGuiaIds.Entrada
                ? movimiento.Guia?.Proveedor?.RazonSocial ?? "Cliente"
                : movimiento.Guia?.AlmacenDestino?.Nombre ?? "Almacen";

        private static string NombreDestino(MovimientoStock movimiento) =>
            movimiento.IdTipoOperacion == TipoOperacionGuiaIds.Salida
                ? movimiento.Guia?.Proveedor?.RazonSocial ?? "Cliente"
                : movimiento.Guia?.AlmacenDestino?.Nombre ?? "Almacen";




    }


}
