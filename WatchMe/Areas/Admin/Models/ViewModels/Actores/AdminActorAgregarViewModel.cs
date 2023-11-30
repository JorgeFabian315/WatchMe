using WatchMe.Models.Entities;

namespace WatchMe.Areas.Admin.Models.ViewModels.Actores
{
    public class AdminActorAgregarViewModel
    {
        public Actor Actor { get; set; } = new Actor();

        public IFormFile? Archivo { get; set; }

    }
}
