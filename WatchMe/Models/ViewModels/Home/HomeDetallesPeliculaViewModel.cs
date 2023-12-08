using WatchMe.Models.Entities;

namespace WatchMe.Models.ViewModels.Home
{
    public class HomeDetallesPeliculaViewModel
    {
        public Pelicula Pelicula { get; set; } = null!;

        public bool TieneReseña {  get; set; } 
    }
}
