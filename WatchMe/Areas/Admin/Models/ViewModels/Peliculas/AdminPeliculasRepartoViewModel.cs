using WatchMe.Models.Entities;

namespace WatchMe.Areas.Admin.Models.ViewModels.Peliculas
{
    public class AdminPeliculasRepartoViewModel
    {
        public int IdPelicula { get; set; }

        public string TituloPelicula { get; set; } = null!;

        public List<ParticipacionPeliculaModel> Actores { get; set; } = new();

    }
    public class ParticipacionPeliculaModel
    {
        public int ActorId { get; set; }
        public string NombreActor { get; set; } = null!;
        public string Personaje { get; set; } = null!;

    }

}
