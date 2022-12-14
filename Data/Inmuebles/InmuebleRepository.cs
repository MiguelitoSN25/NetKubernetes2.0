using Microsoft.AspNetCore.Identity;
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

            _contexto.Inmuebles!.Add(inmueble);
        }

        public void DeleteInmueble(int id)
        {
            var inmueble = _contexto.Inmuebles!
                .FirstOrDefault(x => x.Id == id);

            _contexto.Inmuebles!.Remove(inmueble!);
        }

        public IEnumerable<Inmueble> GetAllInmuebles()
        {
            return _contexto.Inmuebles!.ToList();
        }

        public Inmueble GetInmuebleById(int id)
        {

            return _contexto.Inmuebles!.FirstOrDefault(x => x.Id == id)!;

        }

        public bool SaveChanges()
        {
            return (_contexto.SaveChanges() >= 0);

        }
    }
}
