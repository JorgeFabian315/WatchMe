using System;
using System.Collections.Generic;

namespace WatchMe.Models.Entities;

public partial class Pelicula
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public DateTime FechaLanzamiento { get; set; }

    public string Director { get; set; } = null!;

    public string Sinopsis { get; set; } = null!;

    public sbyte? CalificacionPromeido { get; set; }

    public string Duracion { get; set; } = null!;

    public string UrlTrailer { get; set; } = null!;

    public DateTime? FechaAgregada { get; set; }

    public int PlataformaId { get; set; }

    public int ClasificacionId { get; set; }

    public int IdGenero { get; set; }

    public virtual Clasificacion Clasificacion { get; set; } = null!;

    public virtual Genero IdGeneroNavigation { get; set; } = null!;

    public virtual ICollection<Participacion> Participacion { get; set; } = new List<Participacion>();

    public virtual Plataforma Plataforma { get; set; } = null!;

    public virtual ICollection<Reseña> Reseña { get; set; } = new List<Reseña>();
}
