using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WatchMe.Areas.Admin.Models.ViewModels.Peliculas;
using WatchMe.Models.Entities;
using WatchMe.Repositories;

namespace WatchMe.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrador")]
    public class PeliculasController : Controller
    {
        private readonly Repository<Genero> repositoryGeneros;
        private readonly PeliculasRepository repositoryPeliculas;
        private readonly Repository<Plataforma> repositoryPlataformas;
        private readonly ActoresRepository repositoryActor;
        private readonly Repository<Clasificacion> repositoryClasificacion;
        private readonly Repository<Participacion> repositoryParticipacion;

        public PeliculasController(Repository<Genero> repositoryGeneros, PeliculasRepository repositoryPeliculas
            , Repository<Plataforma> repositoryPlataformas, ActoresRepository repositoryActor, Repository<Clasificacion> repositoryClasificacion, Repository<Participacion> repositoryPsrticipacion)
        {
            this.repositoryGeneros = repositoryGeneros;
            this.repositoryPeliculas = repositoryPeliculas;
            this.repositoryPlataformas = repositoryPlataformas;
            this.repositoryActor = repositoryActor;
            this.repositoryClasificacion = repositoryClasificacion;
            this.repositoryParticipacion = repositoryPsrticipacion;
        }

        [HttpGet]
        [HttpPost]
        public IActionResult Index(AdminPeliculasViewModel vm)
        {
            if (vm.GeneroSeleccionado == 0)
            {
                vm.Peliculas = repositoryPeliculas.GetAll().Select(x => new PeliculasModel
                {
                    Id = x.Id,
                    Titulo = x.Titulo,
                    Director = x.Director,
                    Plataforma = x.Plataforma.Nombre,
                    Genero = x.IdGeneroNavigation.Nombre
                });
            }
            else
            {
                vm.Peliculas = repositoryPeliculas.GetAll()
                    .Where(p => p.IdGenero == vm.GeneroSeleccionado)
                    .Select(x => new PeliculasModel
                    {
                        Id = x.Id,
                        Titulo = x.Titulo,
                        Director = x.Director,
                        Plataforma = x.Plataforma.Nombre,
                        Genero = x.IdGeneroNavigation.Nombre

                    });
            }

            vm.Generos = repositoryGeneros.GetAll().OrderBy(x => x.Nombre).Select(g => new GeneroModel
            {
                Id = g.Id,
                Nombre = g.Nombre
            });

            return View(vm);
        }

        public IActionResult Agregar()
        {
            var vm = new AdminPeliculasAgregarViewModel();

            vm.Plataformas = repositoryPlataformas.GetAll().Select(x => new Model
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            vm.Clasificaciones = repositoryClasificacion.GetAll().Select(x => new Model
            {
                Id = x.Id,
                Nombre = x.Nombre
            });

            vm.Actores = repositoryActor.GetAll().Select(x => new Model
            {
                Id = x.Id,
                Nombre = x.Nombre
            });

            vm.Generos = repositoryGeneros.GetAll().Select(x => new GeneroModel
            {
                Id = x.Id,
                Nombre = x.Nombre
            });

            return View(vm);
        }

        [HttpPost]
        public IActionResult Agregar(AdminPeliculasAgregarViewModel vm)
        {
            ModelState.Clear();

            if (!ValidarPelicula(out List<string> Erroes, vm))
            {
                foreach (var error in Erroes)
                {
                    ModelState.AddModelError("", error);
                }
            }

            if (ModelState.IsValid)
            {
                repositoryPeliculas.Insert(vm.Pelicula);

                if (vm.Archivo == null) // No elijio archivo
                {
                    System.IO.File.Copy("wwwroot/Imagenes/Peliculas/0.jpg", $"wwwroot/Imagenes/Peliculas/{vm.Pelicula.Id}.jpg");
                }
                else
                {
                    System.IO.FileStream fs = System.IO.File.Create($"wwwroot/Imagenes/Peliculas/{vm.Pelicula.Id}.jpg");
                    vm.Archivo.CopyTo(fs);
                    fs.Close();
                }

                foreach (var actorId in vm.ActoresId)
                {
                    var participacion = new Participacion();
                    participacion.IdPelicula = vm.Pelicula.Id;
                    participacion.IdActor = actorId;
                    repositoryParticipacion.Insert(participacion);
                }
                return RedirectToAction("Index");
            }


            vm.Plataformas = repositoryPlataformas.GetAll().Select(x => new Model
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            vm.Clasificaciones = repositoryClasificacion.GetAll().Select(x => new Model
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            vm.Actores = repositoryActor.GetAll().Select(x => new Model
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            vm.Generos = repositoryGeneros.GetAll().Select(x => new GeneroModel
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            return View(vm);
        }

        public IActionResult Eliminar(int id)
        {
            var pelicula = repositoryPeliculas.Get(id);

            if (pelicula == null)
                return RedirectToAction("Index");

            return View(pelicula);
        }

        [HttpPost]
        public IActionResult Eliminar(Pelicula p)
        {
            var pelicula = repositoryPeliculas.Get(p.Id);

            if (pelicula != null)
            {
                var ruta = $"wwwroot/Imagenes/Peliculas/{pelicula.Id}.jpg";
                repositoryPeliculas.Delete(pelicula);

                if (System.IO.File.Exists(ruta))
                {
                    System.IO.File.Delete(ruta);
                }
            }

            return RedirectToAction("Index");
        }
        public IActionResult Editar(int id)
        {
            var vm = new AdminPeliculasAgregarViewModel();

            var p = repositoryPeliculas.Get(id);

            if (p == null)
                return RedirectToAction("Index");

            vm.Pelicula = p;

            var actores = p.Participacion.Select(x => x.IdActor);
            foreach (var item in actores)
            {
                vm.ActoresId.Add(item);
            }

            vm.Plataformas = repositoryPlataformas.GetAll().Select(x => new Model
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            vm.Clasificaciones = repositoryClasificacion.GetAll().Select(x => new Model
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            vm.Actores = repositoryActor.GetAll().Select(x => new Model
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            vm.Generos = repositoryGeneros.GetAll().Select(x => new GeneroModel
            {
                Id = x.Id,
                Nombre = x.Nombre
            });

            return View(vm);
        }


        [HttpPost]
        public IActionResult Editar(AdminPeliculasAgregarViewModel vm)
        {
            ModelState.Clear();

            if (!ValidarPelicula(out List<string> errores, vm))
            {
                foreach (var error in errores)
                {
                    ModelState.AddModelError("", error);
                }
            }

            if (ModelState.IsValid)
            {
                var peliculaExiste = repositoryPeliculas.Get(vm.Pelicula.Id);

                if (peliculaExiste != null)
                {

                    peliculaExiste.Titulo = vm.Pelicula.Titulo;
                    peliculaExiste.Director = vm.Pelicula.Director;
                    peliculaExiste.Duracion = vm.Pelicula.Duracion;
                    peliculaExiste.FechaLanzamiento = vm.Pelicula.FechaLanzamiento;
                    peliculaExiste.UrlTrailer = vm.Pelicula.UrlTrailer;
                    peliculaExiste.ClasificacionId = vm.Pelicula.ClasificacionId;
                    peliculaExiste.PlataformaId = vm.Pelicula.PlataformaId;
                    peliculaExiste.Sinopsis = vm.Pelicula.Sinopsis;
                    peliculaExiste.IdGenero = vm.Pelicula.IdGenero;


                    peliculaExiste.Participacion.Clear();

                    foreach (var actorid in vm.ActoresId)
                    {
                        var par = new Participacion();
                        par.IdActor = actorid;
                        par.IdPelicula = peliculaExiste.Id;
                        peliculaExiste.Participacion.Add(par);
                    }

                    
                    repositoryPeliculas.Update(peliculaExiste);
                    

                    if(vm.Archivo != null)
                    {
                        System.IO.FileStream fs = System.IO.File.Create($"wwwroot/Imagenes/Peliculas/{vm.Pelicula.Id}.jpg");
                        vm.Archivo.CopyTo(fs);
                        fs.Close();
                    }

                }

                return RedirectToAction(nameof(Index));
            }

            vm.Plataformas = repositoryPlataformas.GetAll().Select(x => new Model
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            vm.Clasificaciones = repositoryClasificacion.GetAll().Select(x => new Model
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            vm.Actores = repositoryActor.GetAll().Select(x => new Model
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            vm.Generos = repositoryGeneros.GetAll().Select(x => new GeneroModel
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
            return View(vm);
        }


        public IActionResult Reparto(int id)
        {

            var p = repositoryPeliculas.Get(id);

            if (p == null)
                return RedirectToAction("Index");


            var vm = new AdminPeliculasRepartoViewModel()
            {
                IdPelicula= id,
                TituloPelicula = p.Titulo,
                Actores = p.Participacion.Select(x => new ParticipacionPeliculaModel
                {
                    ActorId = x.IdActor,
                    NombreActor = x.IdActorNavigation.Nombre,
                    Personaje = x.Personaje ?? "Desconocido"
                }).ToList()
            };


            return View(vm);
        }


        [HttpPost]
        public IActionResult Reparto(AdminPeliculasRepartoViewModel vm)
        {
            ModelState.Clear();

            var p = repositoryPeliculas.Get(vm.IdPelicula);
            
            if (p == null)
                return RedirectToAction("Index");


            if (ModelState.IsValid)
            {
                foreach (var item in vm.Actores)
                {
                    var par = repositoryParticipacion.GetAll()
                               .Where(x => x.IdPelicula == vm.IdPelicula && x.IdActor == item.ActorId)
                               .FirstOrDefault();

                    if (string.IsNullOrWhiteSpace(item.Personaje))
                        ModelState.AddModelError("", "El personaje del actor no puede estar vació.");

                    if (par != null)
                    {
                        par.Personaje = item.Personaje;
                        repositoryParticipacion.Update(par);
                    }
                }

                return RedirectToAction("Index");
            }

            vm.Actores = p.Participacion.Select(x => new ParticipacionPeliculaModel
            {
                ActorId = x.IdActor,
                NombreActor = x.IdActorNavigation.Nombre,
                Personaje = x.Personaje ?? "Desconocido"
            }).ToList();
           
            return View(vm);
        }


            public bool ValidarPelicula(out List<string> errores, AdminPeliculasAgregarViewModel vm)
        {
            errores = new();

            if (string.IsNullOrWhiteSpace(vm.Pelicula.Titulo))
                errores.Add("El título de la película no puede estar vacío.");
            else if (vm.Pelicula.Titulo.Length > 40)
                errores.Add("El título de la película ha superado el tamaño establecido.");

            if (string.IsNullOrWhiteSpace(vm.Pelicula.Director))
                errores.Add("El director de la película no puede estar vacío.");
            else if (vm.Pelicula.Director.Length > 50)
                errores.Add("El director de la película ha superado el tamaño establecido.");
            else if(vm.Pelicula.Director.Length <= 5)
                errores.Add("El nombre del director de la película no tiene el suficiente tamaño.");

            if (vm.Pelicula.Duracion <= 0 || vm.Pelicula.Duracion >= 500)
                errores.Add("La duración de la película es incorrecta");

            if (vm.Pelicula.FechaLanzamiento > DateTime.Now.Date)
                errores.Add("La fecha de lanzamiento es incorrecta.");

            if (string.IsNullOrWhiteSpace(vm.Pelicula.UrlTrailer))
                errores.Add("El trailer de la película no puede estar vacío.");
            else if (vm.Pelicula.Titulo.Length > 150)
                errores.Add("El trailer de la película ha superado el tamaño establecido.");
            else if(vm.Pelicula.UrlTrailer.Length < 20)
                errores.Add("El trailer de la película no tiene el suficiente tamaño.");

            if (vm.Pelicula.PlataformaId == 0)
                errores.Add("Selecciona una plataforma.");

            if (vm.Pelicula.ClasificacionId == 0)
                errores.Add("Selecciona una clasificación.");

            if (vm.Pelicula.IdGenero == 0)
                errores.Add("Selecciona un genero de pelicula.");

            if (string.IsNullOrWhiteSpace(vm.Pelicula.Sinopsis))
                errores.Add("La sinopsis de la película no puede estar vacío.");
            else if (vm.Pelicula.Sinopsis.Length < 20)
                errores.Add("La sinopsis de la película no tiene el suficiente tamaño.");

            if (vm.ActoresId == null)
                errores.Add("Selecciona actores.");
            else if (vm.ActoresId.Count() == 0)
                errores.Add("Selecciona por lo menos un actor.");

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
