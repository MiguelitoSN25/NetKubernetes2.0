using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetKubernetes2._0.Models;
using NetKubernetes2._0.Token;

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

            inmueble.FechaCreacion = DateTime.Now;
            inmueble.IdUsuario = Guid.Parse(usuario!.Id);

            _contexto.Inmuebles!.AddAsync(inmueble);
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
            return ((await _contexto.SaveChangesAsync() >= 0));

        }
    }
}
