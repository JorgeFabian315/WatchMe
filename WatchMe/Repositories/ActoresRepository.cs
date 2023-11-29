using Microsoft.EntityFrameworkCore;
using WatchMe.Models.Entities;

namespace WatchMe.Repositories
{
    public class ActoresRepository : Repository<Actor>
    {
        public ActoresRepository(WatchMeContext context) : base(context)
        {
        }


        public override IEnumerable<Actor> GetAll()
        {
            return Context.Actor
                .Include(x => x.Participacion)
                .OrderBy(x => x.Nombre);
        }
    }
}
