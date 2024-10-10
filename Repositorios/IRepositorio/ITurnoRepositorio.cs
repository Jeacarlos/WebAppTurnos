using WebAppTurnos.Models;

namespace WebAppTurnos.Repositorios.IRepositorio
{
    public interface ITurnoRepositorio
    {
        //Para selecionar todos los turnos
        ICollection<Turno> GetTurnos();
        //Para selecionar un turno por ID
        Turno GetTurno(int TurnoId);
        //Para verificar si ya se ha creado un turno por el ID Y descripcion
        bool ExisteTurno(int id);
        bool ExisteTurno(string descripcion);
        //Para crear un turno
        bool CrearTurno(Turno turno);
        //Para actualizar un turno
        bool ActualizarTurno(Turno turno);
        //Para borrar un turno
        bool BorrarTurno(Turno turno);
        //Para Guardar un turno
        bool Guardar();
    }
}
