﻿// <auto-generated />
using System;
using FileServer.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FileServer.Server.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("AccountTopFolder", b =>
                {
                    b.Property<int>("OwnersId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TopFoldersId")
                        .HasColumnType("INTEGER");

                    b.HasKey("OwnersId", "TopFoldersId");

                    b.HasIndex("TopFoldersId");

                    b.ToTable("AccountTopFolder");
                });

            modelBuilder.Entity("FileServer.Server.Models.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Approved")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("Guid")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("StorageLimit")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("FileServer.Server.Models.Entities.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<int>("FolderId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("Guid")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PhysicalPath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FolderId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("FileServer.Server.Models.Entities.Folder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Guid")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("SubFolderId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SubFolderId");

                    b.ToTable("Folder");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Folder");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("FileServer.Server.Models.Entities.SubFolder", b =>
                {
                    b.HasBaseType("FileServer.Server.Models.Entities.Folder");

                    b.Property<int>("ParentId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TopFolderId")
                        .HasColumnType("INTEGER");

                    b.HasIndex("ParentId");

                    b.HasIndex("TopFolderId");

                    b.HasDiscriminator().HasValue("SubFolder");
                });

            modelBuilder.Entity("FileServer.Server.Models.Entities.TopFolder", b =>
                {
                    b.HasBaseType("FileServer.Server.Models.Entities.Folder");

                    b.HasDiscriminator().HasValue("TopFolder");
                });

            modelBuilder.Entity("AccountTopFolder", b =>
                {
                    b.HasOne("FileServer.Server.Models.Entities.Account", null)
                        .WithMany()
                        .HasForeignKey("OwnersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FileServer.Server.Models.Entities.TopFolder", null)
                        .WithMany()
                        .HasForeignKey("TopFoldersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FileServer.Server.Models.Entities.File", b =>
                {
                    b.HasOne("FileServer.Server.Models.Entities.Folder", "Folder")
                        .WithMany()
                        .HasForeignKey("FolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Folder");
                });

            modelBuilder.Entity("FileServer.Server.Models.Entities.Folder", b =>
                {
                    b.HasOne("FileServer.Server.Models.Entities.SubFolder", null)
                        .WithMany("Children")
                        .HasForeignKey("SubFolderId");
                });

            modelBuilder.Entity("FileServer.Server.Models.Entities.SubFolder", b =>
                {
                    b.HasOne("FileServer.Server.Models.Entities.Folder", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FileServer.Server.Models.Entities.TopFolder", null)
                        .WithMany("SubFolders")
                        .HasForeignKey("TopFolderId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("FileServer.Server.Models.Entities.SubFolder", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("FileServer.Server.Models.Entities.TopFolder", b =>
                {
                    b.Navigation("SubFolders");
                });
#pragma warning restore 612, 618
        }
    }
}
