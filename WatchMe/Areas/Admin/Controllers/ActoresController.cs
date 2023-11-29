using Microsoft.AspNetCore.Mvc;
using WatchMe.Areas.Admin.Models.ViewModels.Actores;
using WatchMe.Repositories;

namespace WatchMe.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActoresController : Controller
    {
        private readonly ActoresRepository repositoryActores;

        public ActoresController(ActoresRepository repositoryActores)
        {
            this.repositoryActores = repositoryActores;
        }
        public IActionResult Index()
        {
            var data = repositoryActores.GetAll()
                .Select(x => new AdminActoresViewModel
                {
                    Nombre = x.Nombre,
                    TotalPeliculas = x.Participacion.Count()
                });

            return View(data);
        }


        public IActionResult Agregar()
        {
            return View();
        }
    }
}
