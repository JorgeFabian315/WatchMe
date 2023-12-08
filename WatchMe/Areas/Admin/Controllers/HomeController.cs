using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchMe.Areas.Admin.Models.ViewModels.Home;
using WatchMe.Models.Entities;
using WatchMe.Repositories;

namespace WatchMe.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrador")]
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
                                .OrderByDescending(x => x.FechaAgregada)
                                .Take(3)
                                .Select(x => new UltimaPeliculaModel
                                {
                                    Id = x.Id,
                                    Titulo = x.Titulo,
                                    Clasificacion = x.Clasificacion.Nombre,
                                    Plataforma = x.Plataforma.Nombre
                                }),

                UltimasActoresAgregados = repositoryActor.GetAll()
                                .Take(3)
                                .OrderByDescending(x => x.FechaAgregado)
                                 .Select(x => new ActorModel
                                 {
                                     Id = x.Id,
                                     Nombre = x.Nombre
                                 })
        };
            return View(vm);
        }
    }
}
