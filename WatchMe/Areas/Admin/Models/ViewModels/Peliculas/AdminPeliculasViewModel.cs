namespace WatchMe.Areas.Admin.Models.ViewModels.Peliculas
{
    public class AdminPeliculasViewModel
    {
        public int GeneroSeleccionado { get; set; }
        public IEnumerable<GeneroModel> Generos { get; set; } = null!;

        public IEnumerable<PeliculasModel> Peliculas { get; set; } = null!;

    }

    public class GeneroModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
    }
    public class PeliculasModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Genero { get; set; } = null!;
        public string Director { get; set; } = null!;
        public string Plataforma { get; set; } = null!;
    }

}
