using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppTurnos.Models.Dto
{
    public class EmpleadoDto : BaseEntity
    {
        public string NombreCompleto { get; set; }
        public int documentoId { get; set; }
        public string NumeroDocumento { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
    }
}
