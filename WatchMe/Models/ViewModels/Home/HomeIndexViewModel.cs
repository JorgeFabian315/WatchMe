namespace WatchMe.Models.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        public PeliculaIndexModel PeliculaDestacada { get; set; } = new PeliculaIndexModel();
        public IEnumerable<PeliculaIndexModel> PeliculasMejorValoradas { get; set; } = null!;
        public IEnumerable<PeliculaIndexModel> Tendencias { get; set; } = null!;
        public IEnumerable<PeliculaIndexModel> Terror { get; set; } = null!;

    }
    public class PeliculaIndexModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Genero { get; set; } = null!;
        public int CalificacionPromedio { get; set; }
    }
}
