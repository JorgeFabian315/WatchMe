using Microsoft.AspNetCore.Mvc;
using WatchMe.Models.Entities;
using WatchMe.Models.ViewModels.Reseñas;
using WatchMe.Repositories;

namespace WatchMe.Controllers
{
    public class ReseñasController : Controller
    {
        private readonly PeliculasRepository repositoryPelicula;
        private readonly ReseñaRepositorio repositoryResena;

        public ReseñasController(PeliculasRepository repositoryPelicula, ReseñaRepositorio repositoryResena)
        {
            this.repositoryPelicula = repositoryPelicula;
            this.repositoryResena = repositoryResena;
        }
        public IActionResult Agregar(string id)
        {
            id = id.Replace("-", " ");

            var peli = repositoryPelicula.GetByName(id);
            if (peli == null)
                return RedirectToAction("Index");

            var vm = new ReseñaAgregarViewModel()
            {
                Titulo = id,
            };

            if (repositoryResena.GetAll().Any(x => x.IdUsuario == 1 && x.IdPelicula == peli.Id))
            {
                return RedirectToAction(nameof(MenuReseña), new { titulo = peli.Titulo.Replace(" ", "-"), usuario = 1 });
            }


            return View(vm);
        }

        [HttpPost]
        public IActionResult Agregar(ReseñaAgregarViewModel vm)
        {
            ModelState.Clear();
            var peli = repositoryPelicula.GetByName(vm.Titulo);

            if (peli == null)
                return RedirectToAction("Index");


            if (ModelState.IsValid)
            {
                vm.Reseña.IdUsuario = 1;
                vm.Reseña.IdPelicula = peli.Id;

                repositoryResena.Insert(vm.Reseña);

                return RedirectToAction("VerDetalles", "Home", new {id = vm.Titulo.Replace(" ", "-")});
            }
            return View(vm);
        }

        public IActionResult MenuReseña(string titulo, int usuario)
        {
            titulo = titulo.Replace("-", " ");

            var idpeli = repositoryPelicula.GetByName(titulo);

            if (idpeli == null)
                return RedirectToAction("Index");


            var resena = repositoryResena.GetByIdUsuario(idpeli.Id, usuario);

            if (resena == null)
                return RedirectToAction("Index");

            return View(resena);
        }


        public IActionResult Eliminar(int id)
        {
            var r = repositoryResena.Get(id);


            if (r == null)
                return RedirectToAction("Index");

            return View(r);
        }

        [HttpPost]
        public IActionResult Eliminar(Reseña re)
        {
            var r = repositoryResena.Get(re.Id);
            if (r != null)
            {
                repositoryResena.Delete(r);
            }
            return RedirectToAction("VerDetalles", "Home", new {id = r?.IdPeliculaNavigation.Titulo.Replace("-", " ")});
        }


        public IActionResult Editar(int id)
        {
            var r = repositoryResena.Get(id);
            
            if (r == null)
                return RedirectToAction("Index");

            return View(r);
        }

        [HttpPost]
        public IActionResult Editar(Reseña r)
        {
            return View();
        }
    }
}
