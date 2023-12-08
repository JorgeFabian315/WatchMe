using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WatchMe.Models.Entities;

public partial class WatchMeContext : DbContext
{
    public WatchMeContext()
    {
    }

    public WatchMeContext(DbContextOptions<WatchMeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Actor> Actor { get; set; }

    public virtual DbSet<Clasificacion> Clasificacion { get; set; }

    public virtual DbSet<Genero> Genero { get; set; }

    public virtual DbSet<Participacion> Participacion { get; set; }

    public virtual DbSet<Pelicula> Pelicula { get; set; }

    public virtual DbSet<Plataforma> Plataforma { get; set; }

    public virtual DbSet<Reseña> Reseña { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Actor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("actor");

            entity.Property(e => e.Biografia).HasColumnType("text");
            entity.Property(e => e.FechaAgregado)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaNacimiento)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Nacimiento");
            entity.Property(e => e.LugarNacimiento)
                .HasMaxLength(100)
                .HasColumnName("Lugar_Nacimiento");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Clasificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("clasificacion");

            entity.Property(e => e.Nombre).HasMaxLength(10);
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("genero");

            entity.Property(e => e.Nombre).HasMaxLength(30);
        });

        modelBuilder.Entity<Participacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("participacion");

            entity.HasIndex(e => e.IdActor, "fk_Actor_Pelicula_idx");

            entity.HasIndex(e => e.IdPelicula, "fk_Pelicula_Actor_idx");

            entity.Property(e => e.Personaje)
                .HasMaxLength(45)
                .HasDefaultValueSql("'Desconocido'");

            entity.HasOne(d => d.IdActorNavigation).WithMany(p => p.Participacion)
                .HasForeignKey(d => d.IdActor)
                .HasConstraintName("fk_Actor_Pelicula");

            entity.HasOne(d => d.IdPeliculaNavigation).WithMany(p => p.Participacion)
                .HasForeignKey(d => d.IdPelicula)
                .HasConstraintName("fk_Pelicula_Actor");
        });

        modelBuilder.Entity<Pelicula>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pelicula");

            entity.HasIndex(e => e.ClasificacionId, "fk_Pelicula_Clasificcion_idx");

            entity.HasIndex(e => e.IdGenero, "fk_Pelicula_Genero_idx");

            entity.HasIndex(e => e.PlataformaId, "fk_Pelicula_Plataforma1_idx");

            entity.Property(e => e.CalificacionPromedio)
                .HasDefaultValueSql("'0'")
                .HasColumnName("Calificacion_Promedio");
            entity.Property(e => e.ClasificacionId).HasColumnName("Clasificacion_Id");
            entity.Property(e => e.Director).HasMaxLength(50);
            entity.Property(e => e.FechaAgregada)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaLanzamiento)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Lanzamiento");
            entity.Property(e => e.PlataformaId).HasColumnName("Plataforma_Id");
            entity.Property(e => e.Sinopsis).HasColumnType("text");
            entity.Property(e => e.Titulo).HasMaxLength(40);
            entity.Property(e => e.UrlTrailer)
                .HasMaxLength(150)
                .HasColumnName("URL_Trailer");

            entity.HasOne(d => d.Clasificacion).WithMany(p => p.Pelicula)
                .HasForeignKey(d => d.ClasificacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Pelicula_Clasificacion");

            entity.HasOne(d => d.IdGeneroNavigation).WithMany(p => p.Pelicula)
                .HasForeignKey(d => d.IdGenero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Pelicula_Genero");

            entity.HasOne(d => d.Plataforma).WithMany(p => p.Pelicula)
                .HasForeignKey(d => d.PlataformaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Pelicula_Plataforma1");
        });

        modelBuilder.Entity<Plataforma>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plataforma");

            entity.Property(e => e.Nombre).HasMaxLength(45);
        });

        modelBuilder.Entity<Reseña>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("reseña");

            entity.HasIndex(e => e.IdPelicula, "fk_Reseña_Pelicula1_idx");

            entity.HasIndex(e => e.IdUsuario, "fk_Reseña_Usuario_idx");

            entity.Property(e => e.Comentario).HasColumnType("text");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdPeliculaNavigation).WithMany(p => p.Reseña)
                .HasForeignKey(d => d.IdPelicula)
                .HasConstraintName("fk_Reseña_Pelicula1");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Reseña)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("fk_Reseña_Usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.CorreoElectronico, "CorreoElectronico_UNIQUE").IsUnique();

            entity.Property(e => e.Contrasena)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.CorreoElectronico).HasMaxLength(80);
            entity.Property(e => e.Nombre).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
