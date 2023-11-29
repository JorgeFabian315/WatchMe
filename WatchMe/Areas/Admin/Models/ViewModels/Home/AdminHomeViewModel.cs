namespace WatchMe.Areas.Admin.Models.ViewModels.Home
{
    public class AdminHomeViewModel
    {
        public int TotalPeliculas { get; set; }
        public int TotalActores { get; set; }
        public int TotalPlataformas { get; set; }

        public IEnumerable<UltimaPeliculaModel> UltimasPeliculasAgregadas { get; set; } = null!;
    }

    public class UltimaPeliculaModel
    {
        public string Titulo { get; set; } = null!;
        public string Plataforma { get; set; } = null!;
        public string Clasificacion { get; set; } = null!;
    }
}
