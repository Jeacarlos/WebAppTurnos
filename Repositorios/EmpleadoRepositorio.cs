using WebAppTurnos.Models;
using Microsoft.EntityFrameworkCore;
using WebAppTurnos.Repositorios.IRepositorio;
using WebAppTurnos.Context;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAppTurnos.Repositorios
{
    public class EmpleadoRepositorio : IEmpleadoRepositorio
    {
        private readonly AppDbContext_context _db;
        public EmpleadoRepositorio(AppDbContext_context db)
        {
            _db = db;
        }
        public bool ActualizarEmpleado(Empleado empleado)
        {
            //Arreglando problema del PATCH
            var empleadoExistente = _db.Empleados.Find(empleado.Id);
            if (empleadoExistente != null)
            {
                _db.Entry(empleadoExistente).CurrentValues.SetValues(empleado);
            }
            else
            {
               _db.Empleados.Update(empleado);
            }

            return Guardar();

        }

        public bool BorrarEmpleado(Empleado empleado)
        {
            _db.Empleados.Remove(empleado);
            return Guardar();
        }

        public IEnumerable<Empleado> BuscarEmpleado(string nombrecompleto)
        {
            IQueryable<Empleado> query = _db.Empleados;
            if (!string.IsNullOrEmpty(nombrecompleto))
            {
                query = query.Where(e => e.NombreCompleto.Contains(nombrecompleto));
            }
            return query.ToList();
        }

        public bool CrearEmpleado(Empleado empleado)
        {
           
            _db.Empleados.Add(empleado);
            return Guardar();
        }

        public bool ExisteEmpleado(int id)
        {
            return _db.Empleados.Any(E => E.Id == id);
        }

        public bool ExisteEmpleado(string nombrecompleto)
        {
          
            bool valor = _db.Empleados.Any(E => E.NombreCompleto.ToLower().Trim() == nombrecompleto.ToLower().Trim());
            return valor;
        }

        public Empleado GetEmpleado(int empleadoId)
        {
            return _db.Empleados.FirstOrDefault(e => e.Id == empleadoId);
        }

        public ICollection<Empleado> GetEmpleados()
        {
            return _db.Empleados.OrderBy(e => e.NombreCompleto).ToList();
        }

        public ICollection<Empleado> GetEmpleadosEnDocumento(int docId)
        {
            return _db.Empleados.Include(doc => doc.Documento).Where(doc => doc.documentoId == docId).ToList();
        }

        public bool Guardar()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
