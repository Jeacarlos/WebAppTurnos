using Dapper;
using Microsoft.Build.Logging;
using Microsoft.Data.SqlClient;
using System.Data;
using WebAppTurnos.Context;
using WebAppTurnos.Models;
using WebAppTurnos.Repositorios.IRepositorio;

namespace WebAppTurnos.Repositorios
{
    public class TurnoRepositorio : ITurnoRepositorio

    {
       
        private readonly AppDbContext_context _db;  
        public TurnoRepositorio(AppDbContext_context db)
        {
            _db = db;
        }
        public bool ActualizarTurno(Turno turno)
        {
            var turnoExistente = _db.Turnos.Find(turno.Id);
            if (turnoExistente != null)
            {
                _db.Entry(turnoExistente).CurrentValues.SetValues(turno);
            }
            else
            {
                _db.Turnos.Update(turno);
            }
            return Guardar();

        }

        public bool BorrarTurno(Turno turno)
        {
           _db.Turnos.Remove(turno);    
            return Guardar();
        }

        public bool CrearTurno(Turno turno)
        {
           turno.Horadeinicio = DateTime.Now;
           turno.Horafin = DateTime.Now;
           _db.Turnos.Add(turno);
            return Guardar();

        }

        public bool ExisteTurno(int id)
        {
            return _db.Turnos.Any(t => t.Id == id);
        }

        public bool ExisteTurno(string descripcion)
        {
            bool valor = _db.Turnos.Any(t => t.Descripcion.ToLower().Trim() == descripcion.ToLower().Trim());
            return valor;
        }

        public Turno GetTurno(int TurnoId)
        {
            return _db.Turnos.FirstOrDefault(t => t.Id == TurnoId);
        }
       
        public ICollection<Turno> GetTurnos()
        {
            return _db.Turnos.OrderBy(t => t.Id).ToList();
        }

        public bool Guardar()
        {
           return _db.SaveChanges() >=0 ? true : false;
        }
    }
}
