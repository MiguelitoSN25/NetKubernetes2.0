using NetKubernetes2._0.DTOs.UsuarioDTOs;

namespace NetKubernetes2._0.Data.Usuarios
{
    public interface IUsuarioRepository
    {
        Task<UsuarioResponseDto> GetUsuario();

        Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto request);

        Task<UsuarioResponseDto> RegistroUsuarios(UsuarioRegistroRequestDto request);
    }
}
