﻿// <auto-generated />
using System;
using EBird.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EBird.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("EBird.Domain.Entities.BirdEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Age")
                        .HasColumnType("int")
                        .HasColumnName("BirdAge");

                    b.Property<Guid>("BirdTypeId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("BirdTypeId");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("BirdColor");

                    b.Property<DateTime>("CreatedDatetime")
                        .HasColumnType("datetime")
                        .HasColumnName("BirdCreatedDatetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("BirdDescription");

                    b.Property<int>("Elo")
                        .HasColumnType("int")
                        .HasColumnName("BirdElo");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("BirdName");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("BirdStatus");

                    b.Property<double>("Weight")
                        .HasColumnType("float")
                        .HasColumnName("BirdWeight");

                    b.HasKey("Id");

                    b.HasIndex("BirdTypeId");

                    b.ToTable("Bird");
                });

            modelBuilder.Entity("EBird.Domain.Entities.BirdTypeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDatetime")
                        .HasColumnType("datetime")
                        .HasColumnName("BirdTypeCreatedDatetime");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("TypeCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("BirdTypeCode");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("BirdTypeName");

                    b.HasKey("Id");

                    b.HasIndex("TypeCode")
                        .IsUnique();

                    b.ToTable("BirdType");
                });

            modelBuilder.Entity("EBird.Domain.Entities.BirdEntity", b =>
                {
                    b.HasOne("EBird.Domain.Entities.BirdTypeEntity", "BirdType")
                        .WithMany("Birds")
                        .HasForeignKey("BirdTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BirdType");
                });

            modelBuilder.Entity("EBird.Domain.Entities.BirdTypeEntity", b =>
                {
                    b.Navigation("Birds");
                });
#pragma warning restore 612, 618
        }
    }
}
