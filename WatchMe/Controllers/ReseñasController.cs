using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WatchMe.Models.Entities;
using WatchMe.Models.ViewModels.Reseñas;
using WatchMe.Repositories;

namespace WatchMe.Controllers
{
    [Authorize(Roles = "Critico")]
    public class ReseñasController : Controller
    {
        private readonly PeliculasRepository repositoryPelicula;
        private readonly ReseñaRepositorio repositoryResena;
        private readonly Repository<Usuario> repositoryUsuario;

        public ReseñasController(PeliculasRepository repositoryPelicula, ReseñaRepositorio repositoryResena
            , Repository<Usuario> repositoryUsuario)
        {
            this.repositoryPelicula = repositoryPelicula;
            this.repositoryResena = repositoryResena;
            this.repositoryUsuario = repositoryUsuario;
        }
        public IActionResult Agregar(string id)
        {
            id = id.Replace("-", " ");

            int idusuario = 0;

            var usuario = User.Claims.FirstOrDefault(c => c.Type == "Id");

            if (usuario != null)
            {
                idusuario = int.Parse(usuario.Value);
            }

            var peli = repositoryPelicula.GetByName(id);
            if (peli == null)
                return RedirectToAction("Index");

            if (repositoryResena.GetAll().Any(x => x.IdUsuario == idusuario && x.IdPelicula == peli.Id))
            {
                return RedirectToAction(nameof(MenuReseña), new { titulo = peli.Titulo.Replace(" ", "-"), usuario = idusuario });
            }

            var vm = new ReseñaAgregarViewModel();
            vm.Titulo = peli.Titulo;
            vm.Reseña.IdUsuario = idusuario;

            return View(vm);
        }

        [HttpPost]
        public IActionResult Agregar(ReseñaAgregarViewModel vm)
        {
            ModelState.Clear();

            var peli = repositoryPelicula.GetByName(vm.Titulo);

            if (peli == null)
                return RedirectToAction("Index");


            if(!ValidarReseña(out List<string> errores, vm))
            {
                foreach (var error in errores)
                {
                    ModelState.AddModelError("", error);
                }
            }


            if (ModelState.IsValid)
            {
                vm.Reseña.IdPelicula = peli.Id;

                repositoryResena.Insert(vm.Reseña);

                return RedirectToAction("VerDetalles", "Home", new { id = vm.Titulo.Replace(" ", "-") });
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
                return RedirectToAction("Index", "Home");

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
            return RedirectToAction("VerDetalles", "Home", new { id = r?.IdPeliculaNavigation.Titulo.Replace("-", " ") });
        }


        public IActionResult Editar(int id)
        {
            var r = repositoryResena.Get(id);

            if (r == null)
                return RedirectToAction("Index", "Home");


            var vm = new ReseñaAgregarViewModel();
            
            vm.Reseña = r;
            vm.Titulo = r.IdPeliculaNavigation.Titulo;
            return View(vm);
        }

        [HttpPost]
        public IActionResult Editar(ReseñaAgregarViewModel vm)
        {
            ModelState.Clear();

            if (!ValidarReseña(out List<string> errores, vm))
            {
                foreach (var error in errores)
                {
                    ModelState.AddModelError("", error);
                }
            }

            if (ModelState.IsValid)
            {

            var re = repositoryResena.Get(vm.Reseña.Id);

                if (re != null)
                {
                    re.Comentario = vm.Reseña.Comentario;
                    re.Calificacion = vm.Reseña.Calificacion;
                    repositoryResena.Update(re);
                }
                return RedirectToAction("VerDetalles", "Home", new { id = vm.Titulo.Replace(" ", "-") });
            }

            return View(vm);
        }

        public bool ValidarReseña(out List<string> errores, ReseñaAgregarViewModel vm)
        {
            errores = new();

            if (vm.Reseña.Calificacion <= 0)
                errores.Add("Por favor califique la película.");
            else if(vm.Reseña.Calificacion > 10)
                errores.Add("Error en la calificación.");

            if(string.IsNullOrWhiteSpace(vm.Reseña.Comentario))
                errores.Add("El comentario de la película no puede estar vacío.");

            return errores.Count() == 0;
        }

    }
}
