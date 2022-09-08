using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetKubernetes2._0.Data.Usuarios;
using NetKubernetes2._0.DTOs.UsuarioDTOs;

namespace NetKubernetes2._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        public readonly IUsuarioRepository _repository;

        public UsuarioController(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioResponseDto>> Login(
            [FromBody] UsuarioLoginRequestDto request
        ){
            return await _repository.Login(request);    
        }

        [AllowAnonymous]
        [HttpPost("Registrar")]
        public async Task<ActionResult<UsuarioResponseDto>> Registrar(
           [FromBody] UsuarioRegistroRequestDto request
        )
        {
            return await _repository.RegistroUsuarios(request);
        }

        [HttpGet]
        public async Task<ActionResult<UsuarioResponseDto>> DevolverUsuario(
         [FromBody] UsuarioRegistroRequestDto request
        ){
            return await _repository.GetUsuario();
        }
    }
}
