using WatchMe.Models.Entities;

namespace WatchMe.Areas.Admin.Models.ViewModels.Peliculas
{
    public class AdminPeliculasAgregarViewModel
    {
        public Pelicula Pelicula { get; set; } = new Pelicula()
        {
            FechaLanzamiento = DateTime.Now
        };

        public int Duracion { get; set; }
        public IEnumerable<Model>? Plataformas { get; set; }
        public IEnumerable<Model>? Clasificaciones { get; set; }
        public IEnumerable<Model>? Actores { get; set; }
        public List<int> ActoresId { get; set; } = new();
        public IEnumerable<GeneroModel>? Generos { get; set; }
        public IFormFile? Archivo { get; set; }

    }

    public class Model
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
    }
}
