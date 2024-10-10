using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppTurnos.Models
{
    public class Empleado : BaseEntity
    {
        public string NombreCompleto { get; set; }
        //Relacion con entidad Documentos
        public int documentoId { get; set; }
        [ForeignKey("domentoId")]
        public Documento Documento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }

    }
}
