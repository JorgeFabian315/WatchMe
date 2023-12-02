using Microsoft.AspNetCore.Mvc;
using WatchMe.Models.Entities;
using WatchMe.Models.ViewModels.Home;
using WatchMe.Repositories;

namespace WatchMe.Controllers
{
    public class HomeController : Controller
    {
        private readonly PeliculasRepository repositoryPeliculas;

        public HomeController(PeliculasRepository repositoryPeliculas)
        {
            this.repositoryPeliculas = repositoryPeliculas;
        }
        public IActionResult Index()
        {
            var pelidestacada = repositoryPeliculas.GetAll()
                .OrderBy(x => x.CalificacionPromedio)
                .Select(x => new PeliculaIndexModel
                {
                    Id = x.Id,
                    CalificacionPromedio = x.CalificacionPromedio ?? 0,
                    Genero = x.IdGeneroNavigation.Nombre,
                    Titulo = x.Titulo

                })
                .FirstOrDefault();

            if (pelidestacada == null)
                pelidestacada = new PeliculaIndexModel();

            var vm = new HomeIndexViewModel()
            {
                PeliculaDestacada = pelidestacada,
                PeliculasMejorValoradas = repositoryPeliculas.GetAll()
                .OrderByDescending(x => x.CalificacionPromedio)
                .Where(x => x.Id != pelidestacada.Id)
                .Take(5)
                .Select(x => new PeliculaIndexModel
                {
                    Id = x.Id,
                    CalificacionPromedio = x.CalificacionPromedio ?? 0,
                    Genero = x.IdGeneroNavigation.Nombre,
                    Titulo = x.Titulo

                }),

                Tendencias = repositoryPeliculas.GetAll()
                .OrderBy(x => x.FechaAgregada)
                .Take(5)
                .Select(x => new PeliculaIndexModel
                {
                    Id = x.Id,
                    CalificacionPromedio = x.CalificacionPromedio ?? 0,
                    Genero = x.IdGeneroNavigation.Nombre,
                    Titulo = x.Titulo

                }),

                Terror = repositoryPeliculas.GetAll()
                .Where(x => x.IdGeneroNavigation.Nombre == "Terror")
                .OrderByDescending(x => x.CalificacionPromedio)
                .Take(5)
                .Select(x => new PeliculaIndexModel
                {
                    Id = x.Id,
                    CalificacionPromedio = x.CalificacionPromedio ?? 0,
                    Genero = x.IdGeneroNavigation.Nombre,
                    Titulo = x.Titulo

                })

            };




            return View(vm);
        }
    }
}
