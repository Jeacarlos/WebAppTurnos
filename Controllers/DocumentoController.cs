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
    public class DocumentoController : ControllerBase
    {
        //Llamada al repositorio y los mapper con el medoto constructor
        private readonly IDocumentoRepositorio _dRepositorio;
        private readonly IMapper _mapper;
        public DocumentoController(IDocumentoRepositorio dRepositorio, IMapper mapper)
        {
            _dRepositorio = dRepositorio;
            _mapper = mapper;
        }
        //Seleccionando todos los documento
        [HttpGet]
        //Codigo de mensajes
        [ProducesResponseType(StatusCodes.Status403Forbidden)]//error por parte del usuario cuando ingresa algun datos mal.
        [ProducesResponseType(StatusCodes.Status200OK)] //La solicitud ha tenido éxito.
        public IActionResult GetDocumentos()
        {
            var listaDocumentos = _dRepositorio.GetDocumentos();
            var listaDocumentosDto = new List<DocumentoDto>();
            foreach (var lista in listaDocumentos)
            {
                listaDocumentosDto.Add(_mapper.Map<DocumentoDto>(lista));
            }
            return Ok(listaDocumentosDto);
        }
        //Selecionando un documento por su numero de ID
        [HttpGet("{documentoID:int}", Name = "GetDocumento")]
        //Codigo de mensaje de errores y registro guardado con exito.
        [ProducesResponseType(StatusCodes.Status403Forbidden)]//error por parte del usuario cuando ingresa algun datos mal.
        [ProducesResponseType(StatusCodes.Status200OK)]//La solicitud ha tenido éxito.
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//el servidor no pudo interpretar la solicitud dada una sintaxis inválida.
        [ProducesResponseType(StatusCodes.Status404NotFound)] //El servidor no pudo encontrar el contenido solicitado. 
        public IActionResult GetDocumento(int documentoID)
        {
            var itemDocumento = _dRepositorio.GetDocumento(documentoID);
            if (itemDocumento == null)
            {
                return NotFound();
            }
            var itemDocumentoDto = _mapper.Map<DocumentoDto>(itemDocumento);
            return Ok(itemDocumentoDto);
        }

        //Creando un nuevo documento
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)] //La solicitud ha tenido éxito y se ha creado un nuevo recurso como resultado de ello
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//el servidor no pudo interpretar la solicitud dada una sintaxis inválida.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] //El servidor ha encontrado una situación
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//Es necesario autenticar para obtener la respuesta solicitada.
        public IActionResult CrearDocumento([FromBody] CreaDocumentoDto crearDocumentoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //validando si lo que esta guardando esta vacio o sin datos
            if (crearDocumentoDto == null)
            {
                return BadRequest(ModelState);
            }
            //validando si existe el turno que se esta creando en la BBD
            if (_dRepositorio.ExisteDocumentoCedula(crearDocumentoDto.Cedula))
            {
                ModelState.AddModelError("", "El documento ya existe");
                return StatusCode(404, ModelState);
            }
            if (_dRepositorio.ExisteDocumentoPasaporte(crearDocumentoDto.Pasaporte))
            {
                ModelState.AddModelError("", "El documento ya existe");
                return StatusCode(404, ModelState);
            }
            //condicion para almacenar
            var documento = _mapper.Map<Documento>(crearDocumentoDto);
            if (!_dRepositorio.CrearDocumento(documento))
            {
                ModelState.AddModelError("", $"Algo Salio mal guardando el registro{documento.Cedula}");
               // ModelState.AddModelError("", $"Algo Salio mal guardando el registro{documento.Pasaporte}");
                return StatusCode(404, ModelState);
            }
            return CreatedAtRoute("GetDocumento", new { documentoID = documento.Id }, documento);

        }

        //Actualizar un documento metodo patch
        [HttpPatch("{documentoID:int}", Name = "ActualizarPatchDocumento")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]//La petición se ha completado con éxito pero su respuesta no tiene ningún contenido
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//el servidor no pudo interpretar la solicitud dada una sintaxis inválida.
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//Es necesario autenticar para obtener la respuesta solicitada.
        public IActionResult ActualizarPatchDocumento(int documentoID, [FromBody] DocumentoDto DocumentoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //validando si existe esa categoria o si es nula
            if (DocumentoDto == null || documentoID != DocumentoDto.Id)
            {
                return BadRequest(ModelState);
            }
            var documento = _mapper.Map<Documento>(DocumentoDto);

            if (!_dRepositorio.ActualizarDocumento(documento))
            {
                ModelState.AddModelError("", $"Algo Salio mal actualizando el registro{documento.Cedula}");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }

        //Eliminar un documento
        [HttpDelete("{documentoID:int}", Name = "BorrarDocumento")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]//La petición se ha completado con éxito pero su respuesta no tiene ningún contenido
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//el servidor no pudo interpretar la solicitud dada una sintaxis inválida.
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]//Es necesario autenticar para obtener la respuesta solicitada.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//El servidor ha encontrado una situación
        public IActionResult BorrarDocumento(int documentoID)
        {
            if (!_dRepositorio.ExisteDocumento(documentoID))
            {
                return NotFound();
            }
            var documento = _dRepositorio.GetDocumento(documentoID);

            if (!_dRepositorio.BorrarDocumento(documento))
            {
                ModelState.AddModelError("", $"Algo Salio mal borrando el registro{documento.Cedula}");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
    }
}
