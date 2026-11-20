using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
//using ProyectoAW.Modules.Seguridad.Autenticacion;
using ProyectoAW.Modules.Seguridad.Usuarios.DTOs.Requests;
using ProyectoAW.Modules.Seguridad.Usuarios.Services;
using ProyectoAW.Infraestructure.RP;

namespace ProyectoAW.Modules.Seguridad.Usuarios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Policy = SeguridadPolicies.Admin)]
    public class UsuarioApiController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioApiController(IUsuarioService service)
        {
            _service = service;
        }

        [Authorize(Policy = "Usuarios.Ver")]
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {

                var usuarios = await _service.ListarUsuariosAsync();

                return Ok(new Respuesta
                {
                    Exito = true,
                    Mensaje = "Se Obtuvieron todos los usuarios",
                    Datos = usuarios
                });
            }
            catch(NegocioException ex)
            {
                return BadRequest(new Respuesta
                {
                    Exito = false,
                    Mensaje = ex.Message
                });

            }
            catch (Exception)
            {
                return StatusCode(500, new Respuesta
                {
                    Exito = false,
                    Mensaje = "Error en el servidor comuniquese con Dms o vuelva a intentarlo"
                });
            }
        }

        [Authorize(Policy = "Usuarios.Ver")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {

            try
            {
                var usuario = await _service.ObtenerUsuarioPorIdAsync(id);

                if (usuario == null)
                    return NotFound();

                return Ok(new Respuesta
                {
                    Exito = true,
                    Mensaje = "Se obtuvo el usuario por id",
                    Datos = usuario
                });
            }
            catch(NegocioException ex)
            {
                return BadRequest(new Respuesta
                {
                    Exito = false,
                    Mensaje = ex.Message
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new Respuesta
                {
                    Exito = false,
                    Mensaje = "No se pudo encontrar ,solicite apoyo"
                });
            }
            
        }

        [Authorize(Policy = "Usuarios.Crear")]
        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] CrearUsuarioDto request)
        {
            try
            {
                var usuario = await _service.CrearUsuarioAsync(request);
                return CreatedAtAction(nameof(ObtenerPorId),
                                       new { id = usuario!.IdUsuario },
                                       new Respuesta
                                       {    Exito = true,
                                            Mensaje = " Se logro crear el usuario",
                                            Datos = usuario });
            }
            catch(NegocioException ex)
            {
                return BadRequest(new Respuesta
                {
                    Exito = false,
                    Mensaje = ex.Message
                });
            }
            catch(Exception)
            {
                return StatusCode(500, new Respuesta
                {
                    Exito = false,
                    Mensaje = "Error interno contacte con el equipo de DMS"
                });
            }
        }


        [Authorize(Policy = "Usuarios.Actualizar")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id , [FromBody] ActualizarUsuarioDto request)
        {
            try
            {
                var actualizado = await _service.ActualizarUsuarioAsync(id, request);
                if (!actualizado)
                    return NotFound(new Respuesta { Exito = false, Mensaje = "El Usuario no Existe :v"});

                //return NoContent();

                return Ok(new Respuesta
                {
                    Exito = true,
                    Mensaje = "Se logro Actualizar el Usuario",
                    Datos = actualizado
                });
            }  
            catch(NegocioException ex)
            {
                return BadRequest(new Respuesta
                {
                    Exito = false,
                    Mensaje = ex.Message
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new Respuesta
                {
                    Exito = false,
                    Mensaje = "Error en el servidor"
                });
            }
            
            
        }


        //[HttpPatch("{id:int}/estado")]
        //public async Task<IActionResult> CambiarEstado(int id, [FromBody] bool activo)
        //{
        //    var actualizado = await _service.CambiarEstadoAsync(id, activo);

        //    if (!actualizado)
        //        return NotFound();

        //    return NoContent();

       // }





    }
}
