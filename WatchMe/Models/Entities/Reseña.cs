using System;
using System.Collections.Generic;

namespace WatchMe.Models.Entities;

public partial class Reseña
{
    public int Id { get; set; }

    public string Comentario { get; set; } = null!;

    public sbyte Calificacion { get; set; }

    public DateTime Fecha { get; set; }

    public int IdPelicula { get; set; }

    public int IdUsuario { get; set; }

    public virtual Pelicula IdPeliculaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
