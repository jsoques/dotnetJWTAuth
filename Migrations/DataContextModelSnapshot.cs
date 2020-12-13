﻿// <auto-generated />
using JWTAuth.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JWTAuth.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("JWTAuth.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ActivateKey")
                        .IsRequired()
                        .HasColumnType("text(32)");

                    b.Property<string>("DateCreated")
                        .IsRequired()
                        .HasColumnType("text(25)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text(256)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text(124)");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ActivateKey = "",
                            DateCreated = "12/13/2020 2:19:29 PM",
                            Name = "admin@admin.com",
                            PasswordHash = "AQAAAAEAACcQAAAAEEuZMQSPyBWSA+9sPwLsJvEeL3wMoqj2XFuPs8dfappQ0AXbs9cRzN9/+Cb76U+j4g==",
                            Status = 1
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
