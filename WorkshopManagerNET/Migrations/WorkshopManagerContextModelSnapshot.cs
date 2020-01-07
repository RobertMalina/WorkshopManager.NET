﻿// <auto-generated />
using System;
using WorkshopManagerNET.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WorkshopManagerNET.Migrations
{
    [DbContext(typeof(WorkshopManagerContext))]
    partial class WorkshopManagerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Lab7.Model.Client", b =>
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

            modelBuilder.Entity("Lab7.Model.Order", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ClientId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("DateEnd")
                        .HasColumnType("datetime2(7)");

                    b.Property<DateTime>("DateStart")
                        .HasColumnType("datetime2(7)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<byte>("PartsCount")
                        .HasColumnType("tinyint");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(9,2)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("VehicleModel")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("Lab7.Model.OrderToWorker", b =>
                {
                    b.Property<long>("OrderId")
                        .HasColumnType("bigint");

                    b.Property<long>("WorkerId")
                        .HasColumnType("bigint");

                    b.HasKey("OrderId", "WorkerId");

                    b.HasIndex("WorkerId");

                    b.ToTable("OrderToWorker");
                });

            modelBuilder.Entity("Lab7.Model.Part", b =>
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

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(9,2)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("Part");
                });

            modelBuilder.Entity("Lab7.Model.Trainee", b =>
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

                    b.ToTable("Trainees");
                });

            modelBuilder.Entity("Lab7.Model.Worker", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.ToTable("Worker");

                    b.HasDiscriminator<string>("WorkerType").HasValue("Any");
                });

            modelBuilder.Entity("Lab7.Model.Mechanician", b =>
                {
                    b.HasBaseType("Lab7.Model.Worker");

                    b.Property<int?>("RepairmentsCount")
                        .HasColumnType("int");

                    b.Property<string>("Specialization")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.ToTable("Worker");

                    b.HasDiscriminator().HasValue("Mechanician");
                });

            modelBuilder.Entity("Lab7.Model.Order", b =>
                {
                    b.HasOne("Lab7.Model.Client", "Client")
                        .WithMany("Orders")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Lab7.Model.OrderToWorker", b =>
                {
                    b.HasOne("Lab7.Model.Order", "Order")
                        .WithMany("WorkerOrders")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lab7.Model.Worker", "Worker")
                        .WithMany("WorkerOrders")
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Lab7.Model.Part", b =>
                {
                    b.HasOne("Lab7.Model.Order", "Order")
                        .WithMany("Parts")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Lab7.Model.Trainee", b =>
                {
                    b.HasOne("Lab7.Model.Worker", "Supervisor")
                        .WithOne("Trainee")
                        .HasForeignKey("Lab7.Model.Trainee", "SupervisorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
