using WebAppTurnos.Models;

namespace WebAppTurnos.Repositorios.IRepositorio
{
    public interface IEmpleadoRepositorio
    {
        //Para selecionar todos los empleados
        ICollection<Empleado> GetEmpleados();
        //Para selecionar los empleados de un tipo de documento
        ICollection<Empleado> GetEmpleadosEnDocumento(int docId);
        //buscar empleado 
        IEnumerable<Empleado> BuscarEmpleado(string nombrecompleto);
        //Para selecionar un empleado por ID
        Empleado GetEmpleado(int empleadoId);
        //Para verificar si ya se ha creado un empleado por el ID Y nombre
        bool ExisteEmpleado(int id);
        bool ExisteEmpleado(string nombrecompleto);
        //Para crear un empleado
        bool CrearEmpleado(Empleado empleado);
        //Para actualizar una Pelicula
        bool ActualizarEmpleado(Empleado empleado);
        //Para borrar una Pelicula
        bool BorrarEmpleado(Empleado empleado);
        //Para Guardar una Pelicula
        bool Guardar();
    }
}
