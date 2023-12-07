using System;
using System.Collections.Generic;

namespace WatchMe.Models.Entities;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public string Contrsena { get; set; } = null!;

    public int Rol { get; set; }

    public virtual ICollection<Reseña> Reseña { get; set; } = new List<Reseña>();
}
