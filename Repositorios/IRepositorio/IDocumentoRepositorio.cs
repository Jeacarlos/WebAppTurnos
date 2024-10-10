using WebAppTurnos.Models;

namespace WebAppTurnos.Repositorios.IRepositorio
{
    public interface IDocumentoRepositorio
    {
        //Para selecionar todos los documento
        ICollection<Documento> GetDocumentos();
        //Para selecionar un tipo de documento por ID
        Documento GetDocumento(int DocumentoId);
        //Para verificar si ya se ha creado un documento por el ID y su tipo
        bool ExisteDocumento(int id);
        bool ExisteDocumentoCedula(string cedula);

        bool ExisteDocumentoPasaporte(string pasaporte);
        //Para crear un documento
        bool CrearDocumento(Documento documento);
        //Para actualizar un documento
        bool ActualizarDocumento(Documento documento );
        //Para borrar un documento
        bool BorrarDocumento(Documento documento);
        //Para Guardar un documento
        bool Guardar();
    }
}
