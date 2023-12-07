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
                .OrderByDescending(x => x.CalificacionPromedio)
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

        [Route("~/BuscarPelicula/{id}")]
        [Route("~/BuscarPelicula")]
        public IActionResult BuscarPelicula(string id)
        {

            var vm = new HomeBusquedaPeliculaViewModel()
            {
                Busqueda = id,
                PeliculasBuscadas = repositoryPeliculas.GetAll()
                .OrderBy(x => x.Titulo)
                .Where(x => x.Titulo.ToLower().Contains(id.ToLower()))
                .Select(x => new PeliculaBusquedaModel
                {
                    Id = x.Id,
                    Titulo = x.Titulo,
                    Calificacion = x.CalificacionPromedio ?? 0
                })
            };

            return View(vm);
        }

        [Route("~/VerDetalles/{id}")]
        public IActionResult VerDetalles(string id)
        {
            id = id.Replace("-", " ");

            var peli = repositoryPeliculas.GetByName(id);

            if (peli == null)
                return RedirectToAction("Index");

            return View(peli);
        }

        [Route("~/Genero/{genero}")]
        public IActionResult PeliculasPorGenero(string genero)
        {
            genero = genero.Replace("-", " ");


            var vm = new HomeBusquedaPeliculaViewModel()
            {
                Busqueda = genero,
                PeliculasBuscadas = repositoryPeliculas.GetAll()
                      .OrderBy(x => x.Titulo)
                      .Where(x => x.IdGeneroNavigation.Nombre.ToLower().Contains(genero.ToLower()))
                      .Select(x => new PeliculaBusquedaModel
                      {
                          Id = x.Id,
                          Titulo = x.Titulo,
                          Calificacion = x.CalificacionPromedio ?? 0
                      })
            };

            return View(vm);
        }


        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login()
        {
            return View();
        }

    }
}
