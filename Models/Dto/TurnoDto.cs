namespace WebAppTurnos.Models.Dto
{
    public class TurnoDto : BaseEntity
    {
        public string Descripcion { get; set; }
        public DateTime Horadeinicio { get; set; }
        public DateTime Horafin { get; set; }
        public string Estado { get; set; }
    }
}
