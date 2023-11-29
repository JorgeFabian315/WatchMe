using System;
using System.Collections.Generic;

namespace WatchMe.Models.Entities;

public partial class Actor
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public string Biografia { get; set; } = null!;

    public string LugarNacimiento { get; set; } = null!;

    public virtual ICollection<Participacion> Participacion { get; set; } = new List<Participacion>();
}
