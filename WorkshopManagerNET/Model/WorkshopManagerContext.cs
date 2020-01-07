using WorkshopManagerNET.Model.Auth;
using Microsoft.EntityFrameworkCore;
using System;

namespace WorkshopManagerNET.Model
{
  class WorkshopManagerContext : DbContext
  {
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer("Server=127.0.0.1;Database=WorkshopManagerDbLab7;User Id=sa; Password=Domdom18#;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      //relacja n:m  pomiędzy  Worker : Order
      modelBuilder.Entity<OrderToWorker>()
          .HasKey(ow => new { ow.OrderId, ow.WorkerId });
      modelBuilder.Entity<OrderToWorker>()
          .HasOne(ow => ow.Worker)
          .WithMany(w => w.WorkerOrders)
          .HasForeignKey(ow => ow.WorkerId);
      modelBuilder.Entity<OrderToWorker>()
          .HasOne(ow => ow.Order)
          .WithMany(c => c.WorkerOrders)
          .HasForeignKey(ow => ow.OrderId);

      modelBuilder.Entity<AppUserToAppRole>()
          .HasKey(auar => new { auar.UserId, auar.RoleId });
      modelBuilder.Entity<AppUserToAppRole>()
          .HasOne(auar => auar.Role)
          .WithMany(u => u.Users)
          .HasForeignKey(ow => ow.RoleId);
      modelBuilder.Entity<AppUserToAppRole>()
          .HasOne(auar => auar.User)
          .WithMany(r => r.Roles)
          .HasForeignKey(auar => auar.UserId);

      //relacja 1:n  pomiędzy  Client : Order
      modelBuilder.Entity<Order>()
          .HasOne(o => o.Client)
          .WithMany(c => c.Orders)
          .IsRequired();

      //relacja 1:n  pomiędzy  Order : Part
      modelBuilder.Entity<Order>()
          .HasMany(o => o.Parts)
          .WithOne(p => p.Order);

      //relacja 1:n  pomiędzy  Order : TimeLogs...
      modelBuilder.Entity<Order>()
        .HasMany(o => o.TimeLogs)
        .WithOne(p => p.Order);

      //relacja 1:n  pomiędzy  Worker : TimeLogs...
      modelBuilder.Entity<Worker>()
        .HasMany(o => o.TimeLogs)
        .WithOne(p => p.Worker);

      //definicja tabeli o strukturze hierarchicznej (self referencing table)
      modelBuilder.Entity<Part>()
        .HasMany(p => p.SubParts)
        .WithOne(p => p.ParentalPartSet)
        .HasForeignKey(p => p.ParentPartSetId);

      //relacja 1:1  pomiędzy  Worker : Trainee
      modelBuilder.Entity<Worker>()
          .HasOne(w => w.Trainee)
          .WithOne(t => t.Supervisor)
          .HasForeignKey<Trainee>(t => t.SupervisorId);

      //definicja konwersji odczytu i zapisu typu wyliczeniowego Mechanician.Specialization
      modelBuilder.Entity<Mechanician>()
        .Property(m => m.Specialization)
        .HasConversion(
          s => s.ToString(),
          s => (SpecializationEnum)Enum.Parse(typeof(SpecializationEnum), s)
        ).HasMaxLength(128);

      //zdefiniowanie pola typu "shadow" dla dyskryminatora od mechanizmu dziedziczenia Worker:Mechanician
      modelBuilder.Entity<Worker>()
        .HasDiscriminator<string>("WorkerType")
        .HasValue<Worker>("Any")
        .HasValue<Mechanician>("Mechanician");

      modelBuilder.Entity<Worker>().Property("WorkerType").HasMaxLength(128);

      modelBuilder.Entity<Order>().Property(o => o.Archived).HasDefaultValue(false);
      modelBuilder.Entity<Order>()
        .Property(m => m.Status)
        .HasConversion(
          s => byte.Parse(s.ToString()),
          s => (OrderStatusEnum)Enum.Parse(typeof(OrderStatusEnum), s.ToString())
        ).HasMaxLength(128);
    }

    public DbSet<Worker> Workers { get; set; }
    public DbSet<Mechanician> Mechanicians { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Part> Parts { get; set; }
    public DbSet<Trainee> Trainees { get; set; }
    public DbSet<TimeLog> TimeLogs { get; set; }
    public DbSet<Department> Departments { get; set; }
  }
}
