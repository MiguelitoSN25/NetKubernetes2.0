namespace NetKubernetes2._0.Profiles
{
    using AutoMapper;
    using NetKubernetes2._0.DTOs.InmuebleDTOs;
    using NetKubernetes2._0.Models;

    public class InmueblesProfile : Profile
    {
        public InmueblesProfile()
        {
            CreateMap<Inmueble, InmuebleResponseDto>();
            CreateMap<InmuebleResponseDto, Inmueble>();

        }
    }

}
