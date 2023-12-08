using Microsoft.EntityFrameworkCore;
using WatchMe.Models.Entities;

namespace WatchMe.Repositories
{
    public class ReseñaRepositorio : Repository<Reseña>
    {
        public ReseñaRepositorio(WatchMeContext context) : base(context)
        {
        }

        public Reseña? GetByIdUsuario(int idpeli, int idusuario)
        {
            return Context.Reseña
                .Include(x => x.IdPeliculaNavigation)
                .Include(x => x.IdUsuarioNavigation)
                .Where(x => x.IdPelicula == idpeli && x.IdUsuario == idusuario)
                .FirstOrDefault();
        }

        public override Reseña? Get(object id)
        {
            return Context.Reseña.Include(x => x.IdPeliculaNavigation)
                .FirstOrDefault(x => x.Id == (int)id);
        }

    }
}
