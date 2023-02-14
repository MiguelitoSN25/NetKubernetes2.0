using AutoMapper;
using NetKubernetes2._0.DTOs.InmuebleDtos;
using NetKubernetes2._0.Models;

namespace NetKubernetes2._0.Profiles
{
    public class InmuebleProfile : Profile
    {
        public InmuebleProfile()
        {
            CreateMap<Inmueble,InmuebleResponseDto>();
            CreateMap<InmuebleResponseDto, Inmueble>();
        }
    }
}
