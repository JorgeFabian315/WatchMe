namespace WatchMe.Models.ViewModels.Home
{
    public class HomeBusquedaPeliculaViewModel
    {
        public string Busqueda { get; set; } = null!;

        public IEnumerable<PeliculaBusquedaModel> PeliculasBuscadas { get; set; } = null!;
    }

    public class PeliculaBusquedaModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public int Calificacion { get; set; }

    }
}
