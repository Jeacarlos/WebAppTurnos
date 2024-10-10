using WebAppTurnos.Models;
using WebAppTurnos.Models.Dto;
using AutoMapper;


namespace WebAppTurnos.Mapper
{
    public class TurnoMapper : Profile
    {
        public TurnoMapper()
        {
            CreateMap<Turno, TurnoDto>().ReverseMap();
            CreateMap<Turno, CrearTurnoDto>().ReverseMap();

            CreateMap<Documento, DocumentoDto>().ReverseMap();
            CreateMap<Documento, CreaDocumentoDto>().ReverseMap();

            CreateMap<Empleado, EmpleadoDto>().ReverseMap();
            CreateMap<Empleado, CrearEmpleadoDto>().ReverseMap();
        }
       
    }
}
