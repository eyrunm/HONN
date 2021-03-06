﻿// <auto-generated />
using LibraryAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace api.Migrations
{
    [DbContext(typeof(AppDataContext))]
    [Migration("20171030152509_initialCreate")]
    partial class initialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("LibraryAPI.Models.EntityModels.Book", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DatePublished");

                    b.Property<string>("FirstName");

                    b.Property<string>("ISBN");

                    b.Property<string>("LastName");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("LibraryAPI.Models.EntityModels.Friend", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("ID");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("LibraryAPI.Models.EntityModels.Loan", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateBorrowed");

                    b.Property<DateTime?>("DateReturned");

                    b.Property<int>("bookID");

                    b.Property<int>("friendID");

                    b.Property<bool>("hasReturned");

                    b.HasKey("ID");

                    b.ToTable("Loans");
                });

            modelBuilder.Entity("LibraryAPI.Models.EntityModels.Review", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Rating");

                    b.Property<int>("bookID");

                    b.Property<int>("friendID");

                    b.HasKey("ID");

                    b.ToTable("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
