using Microsoft.EntityFrameworkCore;
using WatchMe.Models.Entities;

namespace WatchMe.Repositories
{
    public class PeliculasRepository : Repository<Pelicula>
    {
        public PeliculasRepository(WatchMeContext context) : base(context)
        {
        }

        public override Pelicula? Get(object id)
        {
            return Context.Pelicula
                .Include(p => p.IdGeneroNavigation)
                .Include(x => x.Participacion)
                    .ThenInclude(x => x.IdActorNavigation)
                .FirstOrDefault(p => p.Id == (int)id);
        }
        public override IEnumerable<Pelicula> GetAll()
        {
            return Context.Pelicula
                .Include(x => x.Plataforma)
                .Include(x => x.Clasificacion)
                .Include(x => x.IdGeneroNavigation)
                .OrderBy(x => x.Titulo);
        }


        public  Pelicula? GetByName(string titulo)
        {
            return Context.Pelicula
                .Include(p => p.IdGeneroNavigation)
                .Include(x => x.Participacion)
                    .ThenInclude(x => x.IdActorNavigation)
                .Include(x => x.Reseña)
                    .ThenInclude(x => x.IdUsuarioNavigation)
                .Include(x => x.Clasificacion)
                .Include(x => x.Plataforma)
                .FirstOrDefault(p => p.Titulo.ToLower() == titulo.ToLower());
        }
    }
}
