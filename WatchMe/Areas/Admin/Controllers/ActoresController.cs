using Microsoft.AspNetCore.Mvc;
using WatchMe.Areas.Admin.Models.ViewModels.Actores;
using WatchMe.Models.Entities;
using WatchMe.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                    Id = x.Id,
                    Nombre = x.Nombre,
                    TotalPeliculas = x.Participacion.Count()
                });

            return View(data);
        }


        public IActionResult Agregar()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Agregar(AdminActorAgregarViewModel vm)
        {
            ModelState.Clear();

            if (!ValidarActor(out List<string> Errores, vm))
            {
                foreach (var e in Errores)
                {
                    ModelState.AddModelError("", e);
                }
            }


            if (ModelState.IsValid)
            {

                repositoryActores.Insert(vm.Actor);


                if (vm.Archivo == null) // No elijio archivo
                {
                    System.IO.File.Copy("wwwroot/Imagenes/Actores/0.jpg", $"wwwroot/Imagenes/Actores/{vm.Actor.Id}.jpg");
                }
                else
                {
                    System.IO.FileStream fs = System.IO.File.Create($"wwwroot/Imagenes/Actores/{vm.Actor.Id}.jpg");
                    vm.Archivo.CopyTo(fs);
                    fs.Close();
                }
                return RedirectToAction(nameof(Index));
            }

            return View(vm);
        }




        public IActionResult Eliminar(int id)
        {
            var actor = repositoryActores.Get(id);

            if (actor == null)
                return RedirectToAction(nameof(Index));


            return View(actor);
        }

        [HttpPost]
        public IActionResult Eliminar(Actor a)
        {
            var actor = repositoryActores.Get(a.Id);

            if (actor != null)
            {
                var ruta = $"wwwroot/Imagenes/Actores/{actor.Id}.jpg";

                repositoryActores.Delete(actor);

                if (System.IO.File.Exists(ruta))
                    System.IO.File.Delete(ruta);
            }

            return RedirectToAction(nameof(Index));
        }



        public IActionResult Editar(int id)
        {
            var actor = repositoryActores.Get(id);

            if (actor == null)
                return RedirectToAction(nameof(Index));

            var vm = new AdminActorAgregarViewModel()
            {
                Actor = actor
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Editar(AdminActorAgregarViewModel vm)
        {
            var actor = repositoryActores.Get(vm.Actor.Id);

            if (actor == null)
                return RedirectToAction(nameof(Index));


            return View(actor);
        }




        public bool ValidarActor(out List<string> errores, AdminActorAgregarViewModel vm)
        {
            errores = new();

            if (string.IsNullOrWhiteSpace(vm.Actor.Nombre))
                errores.Add("El nombre del actor no puede estar vació.");
            else if (vm.Actor.Nombre.Length > 50)
                errores.Add("El nombre del actor ha superado el tamaño establecido.");

            if (string.IsNullOrWhiteSpace(vm.Actor.LugarNacimiento))
                errores.Add("El lugar de nacimiento del actor no puede estar vació.");
            else if (vm.Actor.Nombre.Length > 90)
                errores.Add("El lugar de nacimiento ha superado el tamaño establecido.");

            if (string.IsNullOrWhiteSpace(vm.Actor.Biografia))
                errores.Add("La biografía del actor no puede estar vaciá.");

            if (vm.Actor.FechaNacimiento.Date >= DateTime.Now.Date)
                errores.Add("La fecha de nacimiento es incorrecta.");

            if (vm.Archivo != null) // Selecciono un archivo
            {
                if (vm.Archivo.ContentType != "image/jpeg")
                    errores.Add("Solo se permiten imagenes jpg");

                if (vm.Archivo.Length > 500 * 1024)
                    errores.Add("Solo se permiten archivos no mayores a 500kb");
            }


            return errores.Count() == 0;

        }


    }
}
