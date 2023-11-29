using System;
using System.Collections.Generic;

namespace WatchMe.Models.Entities;

public partial class Genero
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Pelicula> Pelicula { get; set; } = new List<Pelicula>();
}
