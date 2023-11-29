using System;
using System.Collections.Generic;

namespace WatchMe.Models.Entities;

public partial class Participacion
{
    public int Id { get; set; }

    public int IdPelicula { get; set; }

    public int IdActor { get; set; }

    public string? Personaje { get; set; }

    public virtual Actor IdActorNavigation { get; set; } = null!;

    public virtual Pelicula IdPeliculaNavigation { get; set; } = null!;
}
