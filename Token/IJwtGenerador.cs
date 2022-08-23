using NetKubernetes2._0.Models;

namespace NetKubernetes2._0.Token
{
    public interface IJwtGenerador
    {
        string CrearToken(Usuario usuario);

    }
}
