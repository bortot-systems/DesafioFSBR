﻿// <auto-generated />
using System;
using DesafioFSBR.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DesafioFSBR.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241204232126_RemoveCodigoMunicipio")]
    partial class RemoveCodigoMunicipio
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("DesafioFSBR.Models.Processo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataVisualizacao")
                        .HasColumnType("TEXT");

                    b.Property<string>("Municipio")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NPU")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NomeProcesso")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UF")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Processos");
                });
#pragma warning restore 612, 618
        }
    }
}
