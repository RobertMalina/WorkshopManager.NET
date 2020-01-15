﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorkshopManagerNET.Model;

namespace WorkshopManagerNET.Migrations
{
    [DbContext(typeof(WorkshopManagerContext))]
    [Migration("20200115193142_SetPartCodeAsUniqueKey")]
    partial class SetPartCodeAsUniqueKey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WorkshopManagerNET.Model.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("AppRole");
                });

            modelBuilder.Entity("WorkshopManagerNET.Model.AppUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("AppUser");
                });

            modelBuilder.Entity("WorkshopManagerNET.Model.AppUserToAppRole", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AppUserToAppRole");
                });

            modelBuilder.Entity("WorkshopManagerNET.Model.Client", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("WorkshopManagerNET.Model.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("WorkshopManagerNET.Model.Order", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Archived")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<long>("ClientId")
                        .HasColumnType("bigint");

                    b.Property<string>("ComplexityClass")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128)
                        .HasDefaultValue("InEstimation");

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(9,2)");

                    b.Property<DateTime?>("DateEnd")
                        .HasColumnType("datetime2(7)");

                    b.Property<DateTime>("DateRegister")
                        .HasColumnType("datetime2(7)");

                    b.Property<DateTime?>("DateStart")
                        .HasColumnType("datetime2(7)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<decimal>("EstimatedTimeInHours")
                        .HasColumnType("decimal(3,1)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<long?>("SupervisorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(null);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("VehicleDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("SupervisorId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("WorkshopManagerNET.Model.OrderToWorker", b =>
                {
                    b.Property<long>("OrderId")
                        .HasColumnType("bigint");

                    b.Property<long>("WorkerId")
                        .HasColumnType("bigint");

                    b.HasKey("OrderId", "WorkerId");

                    b.HasIndex("WorkerId");

                    b.ToTable("OrderToWorker");
                });

            modelBuilder.Entity("WorkshopManagerNET.Model.Part", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<long>("OrderId")
                        .HasColumnType("bigint");

                    b.Property<long?>("ParentPartSetId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(9,2)");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("OrderId");

                    b.HasIndex("ParentPartSetId");

                    b.ToTable("Part");
                });

            modelBuilder.Entity("WorkshopManagerNET.Model.TimeLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Hours")
                        .HasColumnType("decimal(3,1)");

                    b.Property<DateTime>("LogTime")
                        .HasColumnType("datetime2(7)");

                    b.Property<long>("OrderId")
                        .HasColumnType("bigint");

                    b.Property<long>("WorkerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("WorkerId");

                    b.ToTable("TimeLog");
                });

            modelBuilder.Entity("WorkshopManagerNET.Model.Trainee", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<long>("SupervisorId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("SupervisorId")
                        .IsUnique();

                    b.ToTable("Trainee");
                });

            modelBuilder.Entity("WorkshopManagerNET.Model.Worker", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AppUserId")
                        .HasColumnType("bigint");

                    b.Property<short?>("Bid")
                        .HasColumnType("smallint");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("char(10)")
                        .HasMaxLength(10);

                    b.Property<string>("WorkerType")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("AppUserId")
                        .IsUnique();

                    b.ToTable("Worker");

                    b.HasDiscriminator<string>("WorkerType").HasValue("Any");
                });

            modelBuilder.Entity("WorkshopManagerNET.Model.Mechanician", b =>
                {
                    b.HasBaseType("WorkshopManagerNET.Model.Worker");

                    b.Property<int?>("RepairmentsCount")
                        .HasColumnType("int");

                    b.Property<string>("Specialization")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.ToTable("Worker");

                    b.HasDiscriminator().HasValue("Mechanician");
                });

            modelBuilder.Entity("WorkshopManagerNET.Model.AppUserToAppRole", b =>
                {
                    b.HasOne("WorkshopManagerNET.Model.AppRole", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkshopManagerNET.Model.AppUser", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WorkshopManagerNET.Model.Order", b =>
                {
                    b.HasOne("WorkshopManagerNET.Model.Client", "Client")
                        .WithMany("Orders")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkshopManagerNET.Model.Worker", "Supervisor")
                        .WithMany("SupervisedOrders")
                        .HasForeignKey("SupervisorId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("WorkshopManagerNET.Model.OrderToWorker", b =>
                {
                    b.HasOne("WorkshopManagerNET.Model.Order", "Order")
                        .WithMany("WorkerOrders")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkshopManagerNET.Model.Worker", "Worker")
                        .WithMany("WorkerOrders")
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WorkshopManagerNET.Model.Part", b =>
                {
                    b.HasOne("WorkshopManagerNET.Model.Order", "Order")
                        .WithMany("Parts")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkshopManagerNET.Model.Part", "ParentalPartSet")
                        .WithMany("SubParts")
                        .HasForeignKey("ParentPartSetId")
                        .OnDelete(DeleteBehavior.ClientCascade);
                });

            modelBuilder.Entity("WorkshopManagerNET.Model.TimeLog", b =>
                {
                    b.HasOne("WorkshopManagerNET.Model.Order", "Order")
                        .WithMany("TimeLogs")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkshopManagerNET.Model.Worker", "Worker")
                        .WithMany("TimeLogs")
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WorkshopManagerNET.Model.Trainee", b =>
                {
                    b.HasOne("WorkshopManagerNET.Model.Worker", "Supervisor")
                        .WithOne("Trainee")
                        .HasForeignKey("WorkshopManagerNET.Model.Trainee", "SupervisorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WorkshopManagerNET.Model.Worker", b =>
                {
                    b.HasOne("WorkshopManagerNET.Model.AppUser", "AppUser")
                        .WithOne("Worker")
                        .HasForeignKey("WorkshopManagerNET.Model.Worker", "AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
