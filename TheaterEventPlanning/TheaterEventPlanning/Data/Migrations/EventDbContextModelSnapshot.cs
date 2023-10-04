﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TheaterEventPlanning.Data;

#nullable disable

namespace TheaterEventPlanning.Data.Migrations
{
    [DbContext(typeof(EventDbContext))]
    partial class EventDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TheaterEventPlanning.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EventId"));

                    b.Property<DateTime>("endDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("startDate")
                        .HasColumnType("datetime2");

                    b.HasKey("EventId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("TheaterEventPlanning.Models.Event", b =>
                {
                    b.OwnsMany("TheaterEventPlanning.Models.CastMember", "CastMembers", b1 =>
                        {
                            b1.Property<int>("EventId")
                                .HasColumnType("int");

                            b1.Property<int>("CastMemberId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("CastMemberId"));

                            b1.Property<string>("actorName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("role")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("EventId", "CastMemberId");

                            b1.ToTable("CastMember");

                            b1.WithOwner()
                                .HasForeignKey("EventId");
                        });

                    b.Navigation("CastMembers");
                });
#pragma warning restore 612, 618
        }
    }
}
