using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebAppTurnos.Models;
using WebAppTurnos.Models.Dto;
using WebAppTurnos.Repositorios.IRepositorio;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAppTurnos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurnoController : ControllerBase
    {
        //Llamada al repositorio y los mapper con el medoto constructor
        private readonly ITurnoRepositorio _tRepositorio;
        private readonly IMapper _mapper;
       
        public TurnoController(ITurnoRepositorio tRepositorio, IMapper mapper)
        {
            _tRepositorio = tRepositorio;
            _mapper = mapper;
        }

        //Seleccionando todos los turnos
        [HttpGet]
        //Codigo de mensajes
        [ProducesResponseType(StatusCodes.Status403Forbidden)]//error por parte del usuario cuando ingresa algun datos mal.
        [ProducesResponseType(StatusCodes.Status200OK)] //La solicitud ha tenido éxito.
        public IActionResult GetTurnos()
        {
            var listaTurnos = _tRepositorio.GetTurnos();
            var listaTurnosDto = new List<TurnoDto>();
            foreach (var lista in listaTurnos)
            {
                listaTurnosDto.Add(_mapper.Map<TurnoDto>(lista));
            }
            return Ok(listaTurnosDto);
        }

        //Selecionando un turno por su numero de ID
        [HttpGet("{turnoID:int}", Name = "GetTurno")]
        //Codigo de mensaje de errores y registro guardado con exito.
        [ProducesResponseType(StatusCodes.Status403Forbidden)]//error por parte del usuario cuando ingresa algun datos mal.
        [ProducesResponseType(StatusCodes.Status200OK)]//La solicitud ha tenido éxito.
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//el servidor no pudo interpretar la solicitud dada una sintaxis inválida.
        [ProducesResponseType(StatusCodes.Status404NotFound)] //El servidor no pudo encontrar el contenido solicitado. 
        public IActionResult GetTurno(int turnoID)
        {
            var itemTurno = _tRepositorio.GetTurno(turnoID);
            if (itemTurno == null)
            {
                return NotFound();
            }
            var itemTurnoDto = _mapper.Map<TurnoDto>(itemTurno);
            return Ok(itemTurnoDto);
        }

        //Creando un nuevo turno
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)] //La solicitud ha tenido éxito y se ha creado un nuevo recurso como resultado de ello
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//el servidor no pudo interpretar la solicitud dada una sintaxis inválida.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] //El servidor ha encontrado una situación
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//Es necesario autenticar para obtener la respuesta solicitada.
        public IActionResult CrearTurno([FromBody] CrearTurnoDto crearTurnoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //validando si lo que esta guardando esta vacio o sin datos
            if (crearTurnoDto == null)
            {
                return BadRequest(ModelState);
            }
            //validando si existe el turno que se esta creando en la BBD
            if (_tRepositorio.ExisteTurno(crearTurnoDto.Descripcion))
            {
                ModelState.AddModelError("", "El turno ya existe");
                return StatusCode(404, ModelState);
            }
            var turno = _mapper.Map<Turno>(crearTurnoDto);
            if (!_tRepositorio.CrearTurno(turno))
            {
                ModelState.AddModelError("", $"Algo Salio mal guardando el registro{turno.Descripcion}");
                return StatusCode(404, ModelState);
            }
            return CreatedAtRoute("GetTurno", new { turnoID = turno.Id }, turno);

        }

        //Actualizar un turno metodo patch
        [HttpPatch("{turnoID:int}", Name = "ActualizarPatchTurno")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]//La petición se ha completado con éxito pero su respuesta no tiene ningún contenido
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//el servidor no pudo interpretar la solicitud dada una sintaxis inválida.
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//Es necesario autenticar para obtener la respuesta solicitada.
        public IActionResult ActualizarPatchTurno(int turnoID, [FromBody] TurnoDto TurnoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //validando si existe esa categoria o si es nula
            if (TurnoDto == null || turnoID != TurnoDto.Id)
            {
                return BadRequest(ModelState);
            }
            var turno = _mapper.Map<Turno>(TurnoDto);

            if (!_tRepositorio.ActualizarTurno(turno))
            {
                ModelState.AddModelError("", $"Algo Salio mal actualizando el registro{turno.Descripcion}");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }

        //Eliminar un turno
        [HttpDelete("{turnoID:int}", Name = "BorrarTurno")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]//La petición se ha completado con éxito pero su respuesta no tiene ningún contenido
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//el servidor no pudo interpretar la solicitud dada una sintaxis inválida.
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//Es necesario autenticar para obtener la respuesta solicitada.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//El servidor ha encontrado una situación
        public IActionResult BorrarTurno(int turnoID)
        {
            if (!_tRepositorio.ExisteTurno(turnoID))
            {
                return NotFound();
            }
            var turno = _tRepositorio.GetTurno(turnoID);

            if (!_tRepositorio.BorrarTurno(turno))
            {
                ModelState.AddModelError("", $"Algo Salio mal borrando el registro{turno.Descripcion}");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }

}
}
