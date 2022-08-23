using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using NetKubernetes2._0.DTOs.UsuarioDTOs;
using NetKubernetes2._0.Models;
using NetKubernetes2._0.Token;

namespace NetKubernetes2._0.Data.Usuarios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IJwtGenerador _JWTGenerador;

        private readonly AppDbContext _contexto;

        private readonly IUsuarioSesion _usuarioSesion;

        public UsuarioRepository(

             UserManager<Usuario> userManager,
            SignInManager<Usuario> SignInManager,
            IJwtGenerador jwtGenerador,
            AppDbContext contexto,
            IUsuarioSesion usuarioSesion
            
        )
        {
            _userManager = userManager;
            _signInManager = SignInManager;
            _JWTGenerador = jwtGenerador;
            _contexto = contexto;

        }
        public async Task<UsuarioResponseDto> GetUsuario()
        {
            var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());
            return TransformerUserToUserDto(usuario!);
        }

        private UsuarioResponseDto TransformerUserToUserDto(Usuario usuario)
        {
            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Name = usuario.Name,
                Lastname = usuario.LastName,
                Phone = usuario.Phone,
                Email = usuario.Email,
                UserName = usuario.UserName,
                Token = _JWTGenerador.CrearToken(usuario)
            };
        }

        public async Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto request)
        {
            var usuario = await _userManager.FindByEmailAsync(request.Email!);
            await _signInManager.CheckPasswordSignInAsync(usuario!, request.Password!,false);

            return TransformerUserToUserDto(usuario!);
        }

        public async Task<UsuarioResponseDto> RegistroUsuarios(UsuarioRegistroRequestDto request)
        {
            var usuario = new Usuario
            {
                Name = request.Name,
                LastName = request.LastName,
                Phone = request.Phone,
                Email = request.Email,
                UserName = request.UserName
            };
            await _userManager.CreateAsync(usuario!, request.Password!);

            return TransformerUserToUserDto(usuario);
        }
    }
}
