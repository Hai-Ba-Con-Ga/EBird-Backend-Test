﻿// <auto-generated />
using System;
using EBird.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EBird.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230203111449_createDB2")]
    partial class createDB2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.9");

            modelBuilder.Entity("EBird.Domain.Entities.AccountEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateDateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar");

                    b.Property<string>("RoleString")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar")
                        .HasColumnName("Role");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.HasKey("Id");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("EBird.Domain.Entities.BirdEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Age")
                        .HasColumnType("int")
                        .HasColumnName("BirdAge");

                    b.Property<Guid>("BirdTypeId")
                        .HasColumnType("TEXT")
                        .HasColumnName("BirdTypeId");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar")
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
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar")
                        .HasColumnName("BirdName");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("TEXT")
                        .HasColumnName("OwnerId");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar")
                        .HasColumnName("BirdStatus");

                    b.Property<float>("Weight")
                        .HasColumnType("float")
                        .HasColumnName("BirdWeight");

                    b.HasKey("Id");

                    b.HasIndex("BirdTypeId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Bird");
                });

            modelBuilder.Entity("EBird.Domain.Entities.BirdTypeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedDatetime")
                        .HasColumnType("datetime")
                        .HasColumnName("BirdTypeCreatedDatetime");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TypeCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar")
                        .HasColumnName("BirdTypeCode");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar")
                        .HasColumnName("BirdTypeName");

                    b.HasKey("Id");

                    b.HasIndex("TypeCode")
                        .IsUnique();

                    b.ToTable("BirdType");
                });

            modelBuilder.Entity("EBird.Domain.Entities.GroupEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateDatetime")
                        .HasColumnType("TEXT")
                        .HasColumnName("GroupCreateDatetime");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxELO")
                        .HasColumnType("INTEGER")
                        .HasColumnName("GroupMaxELO");

                    b.Property<int>("MinELO")
                        .HasColumnType("INTEGER")
                        .HasColumnName("GroupMinELO");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar")
                        .HasColumnName("GroupName");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar")
                        .HasColumnName("GroupStatus");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Group");
                });

            modelBuilder.Entity("EBird.Domain.Entities.RefreshTokenEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("IssuedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("JwtId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("EBird.Domain.Entities.RoomEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar")
                        .HasColumnName("RoomCity");

                    b.Property<DateTime>("CreateDateTime")
                        .HasColumnType("datetime")
                        .HasColumnName("RoomCreateDateTime");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar")
                        .HasColumnName("RoomName");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar")
                        .HasColumnName("RoomStatus");

                    b.HasKey("Id");

                    b.ToTable("Room");
                });

            modelBuilder.Entity("EBird.Domain.Entities.VerifcationStoreEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("VerifcationStore");
                });

            modelBuilder.Entity("EBird.Domain.Entities.BirdEntity", b =>
                {
                    b.HasOne("EBird.Domain.Entities.BirdTypeEntity", "BirdType")
                        .WithMany("Birds")
                        .HasForeignKey("BirdTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EBird.Domain.Entities.AccountEntity", "Owner")
                        .WithMany("Birds")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BirdType");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("EBird.Domain.Entities.GroupEntity", b =>
                {
                    b.HasOne("EBird.Domain.Entities.AccountEntity", "CreatedBy")
                        .WithMany("Groups")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("EBird.Domain.Entities.RefreshTokenEntity", b =>
                {
                    b.HasOne("EBird.Domain.Entities.AccountEntity", "Account")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("EBird.Domain.Entities.AccountEntity", b =>
                {
                    b.Navigation("Birds");

                    b.Navigation("Groups");

                    b.Navigation("RefreshTokens");
                });

            modelBuilder.Entity("EBird.Domain.Entities.BirdTypeEntity", b =>
                {
                    b.Navigation("Birds");
                });
#pragma warning restore 612, 618
        }
    }
}
