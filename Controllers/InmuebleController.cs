using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetKubernetes2._0.Data.Inmuebles;
using NetKubernetes2._0.Data.Usuarios;
using NetKubernetes2._0.DTOs.InmuebleDTOs;
using NetKubernetes2._0.Middleware;
using NetKubernetes2._0.Models;
using System.Net;

namespace NetKubernetes2._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InmuebleController : ControllerBase
    {

        private readonly IInmuebleRepository _repository;

        private IMapper _mapper;

        public InmuebleController(
            IInmuebleRepository repository,
            IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<InmuebleResponseDto>> GetInmuebles()
        {
            var inmuebles = _repository.GetAllInmuebles();

            return Ok(_mapper.Map<IEnumerable<InmuebleResponseDto>>(inmuebles));
        }

        [HttpGet("{Id}", Name = "GetInmueblesById")]
        public ActionResult<IEnumerable<InmuebleResponseDto>> GetInmueblesById(int id)
        {
            var inmuebles = _repository.GetInmuebleById(id);

            if (inmuebles is null)
            {
                throw new MiddlewareException(
                    HttpStatusCode.NotFound,
                    new { mensaje = $"No se encontro el inmueble por este id {id}" }
                );
            }

            return Ok(_mapper.Map<IEnumerable<InmuebleResponseDto>>(inmuebles));
        }

        [HttpPost]

        public ActionResult<InmuebleResponseDto> CreateInmueble ([FromBody] InmuebleRequestDto inmueble)
        {
            var inmuebleModel = _mapper.Map<Inmueble>(inmueble);
            _repository.CreateInmueble(inmuebleModel);
            _repository.SaveChanges();

            var inmuebleResponse = _mapper.Map<InmuebleResponseDto>(inmuebleModel);

            return CreatedAtRoute(nameof(GetInmueblesById), new { inmuebleResponse.Id }, inmuebleResponse);
        }


        [HttpDelete("{id}")]

        public ActionResult DeleteInmueble (int id)
        {
            _repository.DeleteInmueble(id);
            _repository.SaveChanges();
            return Ok();
        }


    }
}
