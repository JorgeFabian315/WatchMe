using Microsoft.AspNetCore.Mvc;
using WatchMe.Areas.Admin.Models.ViewModels.Home;
using WatchMe.Models.Entities;
using WatchMe.Repositories;

namespace WatchMe.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly PeliculasRepository repositoryPeliculas;
        private readonly ActoresRepository repositoryActor;
        private readonly Repository<Plataforma> repositoryPlataforma;

        public HomeController(PeliculasRepository repositoryPeliculas, ActoresRepository repositoryActor,
            Repository<Plataforma> repositoryPlataforma)
        {
            this.repositoryPeliculas = repositoryPeliculas;
            this.repositoryActor = repositoryActor;
            this.repositoryPlataforma = repositoryPlataforma;
        }
        public IActionResult Index()
        {
            var vm = new AdminHomeViewModel()
            {

                TotalPeliculas = repositoryPeliculas.GetAll().Count(),
                TotalActores = repositoryActor.GetAll().Count(),
                TotalPlataformas = repositoryPlataforma.GetAll().Count(),
                UltimasPeliculasAgregadas = repositoryPeliculas.GetAll()
                                .OrderBy(x => x.FechaAgregada)
                                .Take(10)
                                .Select(x => new UltimaPeliculaModel
                                {
                                    Titulo = x.Titulo,
                                    Clasificacion = x.Clasificacion.Nombre,
                                    Plataforma = x.Plataforma.Nombre
                                })
            };

            return View(vm);
        }
    }
}
