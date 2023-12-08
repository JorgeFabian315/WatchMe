using WatchMe.Models.Entities;

namespace WatchMe.Models.ViewModels.Reseñas
{
    public class ReseñaAgregarViewModel
    {
        public string Titulo { get; set; } = null!;
        public Reseña Reseña { get; set; } = new();
    }
}
