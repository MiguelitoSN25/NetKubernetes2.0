using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetKubernetes2._0.Middleware;
using NetKubernetes2._0.Models;
using NetKubernetes2._0.Token;
using System.Net;

namespace NetKubernetes2._0.Data.Inmuebles
{
    public class InmuebleRepository : IInmuebleRepository
    {
        private readonly AppDbContext _contexto;
        private readonly UserManager<Usuario> _userManager;
        private readonly IUsuarioSesion _usuarioSesion;
        public InmuebleRepository(AppDbContext contexto, IUsuarioSesion sesion)
        {
            _contexto = contexto;
            _usuarioSesion = sesion;
            UserManager<Usuario> userManager;

        }
        public async Task CreateInmueble(Inmueble inmueble)
        {
            var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());

            if (usuario is null)
            {
                throw new MiddlewareException(
                    HttpStatusCode.Unauthorized,
                    new { mensaje = "El usuario no es valido para hacer esta insercion" }
                    );
            }

            if (inmueble is null)
            {
                throw new MiddlewareException(
                   HttpStatusCode.Unauthorized,
                   new { mensaje = "Los datos del inmueble son incorrectos" }
                   );
            }


            inmueble.FechaCreacion = DateTime.Now;
            inmueble.IdUsuario = Guid.Parse(usuario!.Id);

            await _contexto.Inmuebles!.AddAsync(inmueble);
        }

        public async Task DeleteInmueble(int id)
        {
            var inmueble = await _contexto.Inmuebles!
                .FirstOrDefaultAsync(x => x.Id == id);

            _contexto.Inmuebles!.Remove(inmueble!);
        }

        public async Task<IEnumerable<Inmueble>> GetAllInmuebles()
        {
            return await _contexto.Inmuebles!.ToListAsync();
        }

        public async Task<Inmueble> GetInmuebleById(int id)
        {

            return await _contexto.Inmuebles!.FirstOrDefaultAsync(x => x.Id == id)!;

        }

        public async Task <bool> SaveChanges()
        {
            return (( await _contexto.SaveChangesAsync()) >= 0);

        }
    }
}
