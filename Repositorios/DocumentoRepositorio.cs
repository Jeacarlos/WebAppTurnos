using WebAppTurnos.Context;
using WebAppTurnos.Models;
using WebAppTurnos.Repositorios.IRepositorio;

namespace WebAppTurnos.Repositorios
{
    public class DocumentoRepositorio : IDocumentoRepositorio
    {
        private readonly AppDbContext_context _db;
        public DocumentoRepositorio(AppDbContext_context db)
        {
            _db = db;
        }

        public bool ActualizarDocumento(Documento documento)
        {
            var documentoExistente = _db.Documentos.Find(documento.Id);
            if (documentoExistente != null)
            {
                _db.Entry(documentoExistente).CurrentValues.SetValues(documento);
            }
            else
            {
                _db.Documentos.Update(documento);
            }
            return Guardar();
        }

        public bool BorrarDocumento(Documento documento)
        {
            _db.Documentos.Remove(documento);
            return Guardar();
        }

        public bool CrearDocumento(Documento documento)
        {
            _db.Documentos.Add(documento);
            return Guardar();
        }

        public bool ExisteDocumento(int id)
        {
            return _db.Documentos.Any(d => d.Id == id);
        }

        public bool ExisteDocumentoCedula(string cedula)
        {
            bool valor = _db.Documentos.Any(d => d.Cedula.ToLower().Trim() == cedula.ToLower().Trim());
            return valor;
        }

        public bool ExisteDocumentoPasaporte(string pasaporte)
        {
            bool valor = _db.Documentos.Any(d => d.Pasaporte.ToLower().Trim() == pasaporte.ToLower().Trim());
            return valor;
        }

        public Documento GetDocumento(int DocumentoId)
        {
            return _db.Documentos.FirstOrDefault(d => d.Id == DocumentoId);
        }

        public ICollection<Documento> GetDocumentos()
        {
            return _db.Documentos.OrderBy(d => d.Id).ToList();
        }

        public bool Guardar()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
