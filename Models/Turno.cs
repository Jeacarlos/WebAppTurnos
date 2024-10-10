using Microsoft.VisualBasic;

namespace WebAppTurnos.Models
{
    public class Turno : BaseEntity
    {
        public string Descripcion { get; set; }
        public DateTime Horadeinicio { get; set; }
        public DateTime Horafin { get; set; }
        public string Estado { get; set; }

    }
}
