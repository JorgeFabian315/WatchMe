using FruitStore.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WatchMe.Models.Entities;
using WatchMe.Models.ViewModels.Home;
using WatchMe.Repositories;

namespace WatchMe.Controllers
{
    public class HomeController : Controller
    {
        private readonly PeliculasRepository repositoryPeliculas;
        private readonly Repository<Usuario> repositoryUsuarios;
        private readonly ActoresRepository repositoryActor;
        private readonly Repository<Reseña> repositoryResena;

        public HomeController(PeliculasRepository repositoryPeliculas, Repository<Usuario> repositoryUsuarios
            ,ActoresRepository repositoryActor, Repository<Reseña> repositoryResena)
        {
            this.repositoryPeliculas = repositoryPeliculas;
            this.repositoryUsuarios = repositoryUsuarios;
            this.repositoryActor = repositoryActor;
            this.repositoryResena = repositoryResena;
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

            int idusuario = 0;
            var usuario = User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (usuario != null)
            {
                idusuario = int.Parse(usuario.Value);
            }

            var tieneReseña = repositoryResena.GetAll().Any(x => x.IdPelicula == peli.Id && x.IdUsuario == idusuario);

            var vm = new HomeDetallesPeliculaViewModel()
            {
                Pelicula = peli,
                TieneReseña = tieneReseña
            };

            return View(vm);
        }


        [Route("~/VerActor/{id}")]
        public IActionResult VerActor(string id) 
        {
            id = id.Replace("-", " ");
            var actor = repositoryActor.GetByNombre(id);

            if (actor == null)
            {
                return RedirectToAction("Index");

            }
            return View(actor);
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


        [Route("~/Plataforma/{plataforma}")]
        public IActionResult PeliculasPorPlataforma(string plataforma)
        {
            plataforma = plataforma.Replace("-", " ");


            var vm = new HomeBusquedaPeliculaViewModel()
            {
                Busqueda = plataforma,
                PeliculasBuscadas = repositoryPeliculas.GetAll()
                      .OrderBy(x => x.Titulo)
                      .Where(x => x.Plataforma.Nombre.ToLower().Contains(plataforma.ToLower()))
                      .Select(x => new PeliculaBusquedaModel
                      {
                          Id = x.Id,
                          Titulo = x.Titulo,
                          Calificacion = x.CalificacionPromedio ?? 0
                      })
            };

            return View(vm);
        }

        [Route("~/Login")]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(HomeLoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = repositoryUsuarios.GetAll().FirstOrDefault(d =>
                d.CorreoElectronico == vm.CorreoElectronico && d.Contrasena == Encriptacion.StringToSha512(vm.Contraseña));

                if (user == null)
                {
                    ModelState.AddModelError("", "La contraseña o el correo electrónico son incorrectos");
                }
                else
                {
                    List<Claim> claims = new List<Claim>();

                    claims.Add(new Claim("Id", user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.Nombre));
                    claims.Add(new Claim(ClaimTypes.Role, user.Rol == 1 ? "Administrador" : "Critico"));

                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    HttpContext.SignInAsync(new ClaimsPrincipal(identity), new AuthenticationProperties
                    {
                        IsPersistent = true
                    });

                    if (user.Rol == 1)
                    {
                        return RedirectToAction("Index", "Home", new {area = "Admin"});
                    }

                    return RedirectToAction("Index", "Home");

                }
            }
            return View(vm);
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Denied() => View();

    }
}
