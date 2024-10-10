using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAppTurnos.Models;
using WebAppTurnos.Models.Dto;
using WebAppTurnos.Repositorios.IRepositorio;

namespace WebAppTurnos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        //Llamada al repositprio y el mapper
        private readonly IEmpleadoRepositorio _empRepo;
        private readonly IMapper _mapper;
        public EmpleadoController(IEmpleadoRepositorio empRepo, IMapper mapper)
        {
            _empRepo = empRepo;
            _mapper = mapper;
        }
        //Obtener todos los empleado
        [HttpGet]
        //Codigo de mensaje de errores y registro guardado con exito.
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetEmpleados()
        {
            var listaEmpleados = _empRepo.GetEmpleados();
            var listaEmpleadosDto = new List<EmpleadoDto>();
            foreach (var lista in listaEmpleados)
            {
                listaEmpleadosDto.Add(_mapper.Map<EmpleadoDto>(lista));
            }
            return Ok(listaEmpleadosDto);
        }
        //Obtener empleado por numero de ID
        [HttpGet("{empleadoID:int}", Name = "GetEmpleado")]
        //Codigo de mensaje de errores y registro guardado con exito.
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEmpleado(int empleadoID)
        {
            var itemempleado = _empRepo.GetEmpleado(empleadoID);
            if (itemempleado == null)
            {
                return NotFound();
            }
            var itemEmpleadoDto = _mapper.Map<EmpleadoDto>(itemempleado);
            return Ok(itemEmpleadoDto);
        }
        //codigo para crear un nuevo registro

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(EmpleadoDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearPelicula([FromBody] CrearEmpleadoDto crearEmpleadoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //validando si lo guardo  vacio o sin dantos
            if (crearEmpleadoDto == null)
            {
                return BadRequest(ModelState);
            }
            //validando si la categoria existe en la BBD
            if (_empRepo.ExisteEmpleado(crearEmpleadoDto.NombreCompleto))
            {
                ModelState.AddModelError("", "El empleado ya existe");
                return StatusCode(404, ModelState);
            }

            var empleado = _mapper.Map<Empleado>(crearEmpleadoDto);

            if (!_empRepo.CrearEmpleado(empleado))
            {
                ModelState.AddModelError("", $"Algo Salio mal guardando el registro{empleado.NombreCompleto}");
                return StatusCode(404, ModelState);
            }
            return CreatedAtRoute("GetEmpleado", new { empleadoID = empleado.Id }, empleado);
        }
        //codigo para actualizar un empleado metodo patch
        [HttpPatch("{empleadoID:int}", Name = "ActualizarPatchEmpleado")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult ActualizarPatchEmpleado(int empleadoID, [FromBody] EmpleadoDto empleadoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //validando si existe esa categoria o si es nula
            if (empleadoDto == null || empleadoID != empleadoDto.Id)
            {
                return BadRequest(ModelState);

            }
            var empleadoExistente = _empRepo.GetEmpleado(empleadoID);
            if (empleadoExistente == null)
            {
                return NotFound($"No se encontro el empleado con ID{empleadoExistente}");
            }
            var empleado = _mapper.Map<Empleado>(empleadoDto);

            if (!_empRepo.ActualizarEmpleado(empleado))
            {
                ModelState.AddModelError("", $"Algo Salio mal actualizando el registro{empleado.NombreCompleto}");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
        //codigo para eliminar un empleado
        [HttpDelete("{empleadoID:int}", Name = "BorrarEmpleado")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BorrarEmpleado(int empleadoID)
        {
            if (!_empRepo.ExisteEmpleado(empleadoID))
            {
                return NotFound();
            }
            var empleado = _empRepo.GetEmpleado(empleadoID);

            if (!_empRepo.BorrarEmpleado(empleado))
            {
                ModelState.AddModelError("", $"Algo Salio mal borrando el registro{empleado.NombreCompleto}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        //obtener empleado por tipo de documento ID
        [HttpGet("GetEmpleadoEnDocumento/{documentoId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetEmpleadoEnDocumento(int documentoId)
        {
            var listaempleado = _empRepo.GetEmpleadosEnDocumento(documentoId);
            if (listaempleado == null)
            {
                return NotFound();
            }
            var itemEmpleado = new List<EmpleadoDto>();
            foreach (var empleado in listaempleado)
            {
                itemEmpleado.Add(_mapper.Map<EmpleadoDto>(empleado));
            }
            return Ok(itemEmpleado);
        }
        //obtener empleado por nombre y demas
        [HttpGet("Buscar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Buscar(string nombre)
        {
            try
            {
                var resultado = _empRepo.BuscarEmpleado(nombre);
                //Any = siginifica que todo lo que se valla buscar coincida con lo que dijite en la api
                if (resultado.Any())
                {
                    return Ok(resultado);
                }
                return NotFound();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos de la aplicacion");
            }

        }
    }


}
