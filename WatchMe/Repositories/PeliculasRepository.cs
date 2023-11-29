﻿using Microsoft.EntityFrameworkCore;
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
    }
}
