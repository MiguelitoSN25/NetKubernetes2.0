using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetKubernetes2._0.Data.Inmuebles;
using NetKubernetes2._0.DTOs.InmuebleDtos;
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
            IMapper mapper
            )
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InmuebleResponseDto>>> GetInmuebles()
        {
            var inmuebles = _repository.GetAllInmuebles();
            return Ok(_mapper.Map<IEnumerable<InmuebleResponseDto>>(inmuebles));
        }

        [HttpGet("{id}",Name = "GetInmueblesById")]
        public async Task <ActionResult<InmuebleResponseDto>> GetInmueblesById(int id)
        {
            var inmuebles = await _repository.GetInmuebleById(id);
            if (inmuebles == null)
            {
                throw new MiddlewareException(
                    HttpStatusCode.NotFound,
                    new {mensaje = $"No se encontro el inmueble por este id {id}"}
                    );
            }
            return Ok(_mapper.Map<IEnumerable<InmuebleResponseDto>>(inmuebles));
        }

        [HttpPost]
        public async Task <ActionResult<InmuebleResponseDto>> CreateInmueble([FromBody] InmuebleRequestDto inmueble)
        {
            var inmueblemodel = _mapper.Map<Inmueble>(inmueble);
            _repository.CreateInmueble(inmueblemodel);
            _repository.SaveChanges();

            var inmuebleResponse =  _mapper.Map<InmuebleResponseDto>(inmueblemodel);

            return CreatedAtRoute(nameof(GetInmueblesById), new { inmuebleResponse.Id}, inmuebleResponse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInmueble(int id)
        {
           await _repository.DeleteInmueble(id);
           await _repository.SaveChanges();
            return Ok();

        }


    }
}
