using WatchMe.Models.Entities;

namespace WatchMe.Areas.Admin.Models.ViewModels.Peliculas
{
    public class AdminPeliculasRepartoViewModel
    {
        public int IdPelicula { get; set; }

        public string TituloPelicula { get; set; } = null!;

        public IEnumerable<ParticipacionPeliculaModel> Actores { get; set; } = null!;

    }
    public class ParticipacionPeliculaModel
    {
        public string NombreActor { get; set; } = null!;
        public int ActorId { get; set; }
        public string Personaje { get; set; } = null!;

    }

}
