﻿// <auto-generated />
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231021132908_TestInit")]
    partial class TestInit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ChapterApp.Models.Chapter", b =>
                {
                    b.Property<int>("ChapterId")
                        .HasColumnType("int");

                    b.HasKey("ChapterId");

                    b.ToTable("Chapters");
                });

            modelBuilder.Entity("ChapterApp.Models.ChapterLink", b =>
                {
                    b.Property<int>("LinkId")
                        .HasColumnType("int");

                    b.HasKey("LinkId");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("ChapterChapterLink", b =>
                {
                    b.Property<int>("LinksLinkId")
                        .HasColumnType("int");

                    b.Property<int>("ParentsChaptersChapterId")
                        .HasColumnType("int");

                    b.HasKey("LinksLinkId", "ParentsChaptersChapterId");

                    b.HasIndex("ParentsChaptersChapterId");

                    b.ToTable("ChapterChapterLink");
                });

            modelBuilder.Entity("ChapterChapterLink", b =>
                {
                    b.HasOne("ChapterApp.Models.ChapterLink", null)
                        .WithMany()
                        .HasForeignKey("LinksLinkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChapterApp.Models.Chapter", null)
                        .WithMany()
                        .HasForeignKey("ParentsChaptersChapterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
