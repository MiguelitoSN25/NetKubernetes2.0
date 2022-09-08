using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetKubernetes2._0.DTOs.UsuarioDTOs;
using NetKubernetes2._0.Middleware;
using NetKubernetes2._0.Models;
using NetKubernetes2._0.Token;
using System.Net;

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
            if(usuario is null)
            {
                throw new MiddlewareException(
                    HttpStatusCode.Unauthorized , 
                    new {mensaje = "El usuario del token no existe en la base de datos"});
            }
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
            if(usuario is null)
            {
                throw new MiddlewareException(HttpStatusCode.Unauthorized, new { mensaje = "El email del usuario no existe en mi base de datos" }
                );
            }
            var resultado  = await _signInManager.CheckPasswordSignInAsync(usuario!, request.Password!,false);

            if(resultado.Succeeded)
            {
                return TransformerUserToUserDto(usuario);

            }

            throw new MiddlewareException(
                HttpStatusCode.Unauthorized,
                new { mensaje = "Las credenciales son incorrectas" }
            );
        }

        public async Task<UsuarioResponseDto> RegistroUsuarios(UsuarioRegistroRequestDto request)
        {
            var exiseEmail = await _contexto.Users.Where(x => x.Email == request.Email).AnyAsync();

            if (exiseEmail)
            {
                throw new MiddlewareException(
                HttpStatusCode.BadRequest,
                new { mensaje = "El email ya existe en mi base de datos" }
);
            }



            var exiseusername = await _contexto.Users.Where(x => x.UserName == request.UserName).AnyAsync();

            if (exiseusername)
            {
                throw new MiddlewareException(
                HttpStatusCode.BadRequest,
                new { mensaje = "El Usuario ya existe en mi base de datos" }
);
            }


            var usuario = new Usuario
            {
                Name = request.Name,
                LastName = request.LastName,
                Phone = request.Phone,
                Email = request.Email,
                UserName = request.UserName
            };
            var result = await _userManager.CreateAsync(usuario!, request.Password!);

            if (result.Succeeded)
            {
                return TransformerUserToUserDto(usuario);

            }
            else
            {
                throw new Exception("No se pudo registrar el usuario");
            }

        }
    }
}
