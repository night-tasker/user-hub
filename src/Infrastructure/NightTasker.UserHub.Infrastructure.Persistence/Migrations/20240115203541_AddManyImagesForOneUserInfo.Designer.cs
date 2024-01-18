﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NightTasker.UserHub.Infrastructure.Persistence;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NightTasker.UserHub.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240115203541_AddManyImagesForOneUserInfo")]
    partial class AddManyImagesForOneUserInfo
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NightTasker.UserHub.Core.Domain.Entities.UserImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("ContentType")
                        .HasColumnType("text")
                        .HasColumnName("content_type");

                    b.Property<DateTimeOffset>("CreatedDateTimeOffset")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date_time_offset");

                    b.Property<string>("Extension")
                        .HasColumnType("text")
                        .HasColumnName("extension");

                    b.Property<string>("FileName")
                        .HasColumnType("text")
                        .HasColumnName("file_name");

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint")
                        .HasColumnName("file_size");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<DateTimeOffset?>("UpdatedDateTimeOffset")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date_time_offset");

                    b.Property<Guid>("UserInfoId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_info_id");

                    b.HasKey("Id")
                        .HasName("pk_user_image");

                    b.HasIndex("UserInfoId")
                        .HasDatabaseName("ix_user_image_user_info_id");

                    b.ToTable("user_image", (string)null);
                });

            modelBuilder.Entity("NightTasker.UserHub.Core.Domain.Entities.UserInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedDateTimeOffset")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date_time_offset");

                    b.Property<string>("Email")
                        .HasMaxLength(254)
                        .HasColumnType("character varying(254)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("last_name");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("middle_name");

                    b.Property<DateTimeOffset?>("UpdatedDateTimeOffset")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date_time_offset");

                    b.Property<string>("UserName")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("user_name");

                    b.HasKey("Id")
                        .HasName("pk_user_infos");

                    b.ToTable("user_infos", (string)null);
                });

            modelBuilder.Entity("NightTasker.UserHub.Core.Domain.Entities.UserImage", b =>
                {
                    b.HasOne("NightTasker.UserHub.Core.Domain.Entities.UserInfo", "UserInfo")
                        .WithMany("UserInfoImages")
                        .HasForeignKey("UserInfoId")
                        .HasConstraintName("fk_user_image_user_info_user_info_id");

                    b.Navigation("UserInfo");
                });

            modelBuilder.Entity("NightTasker.UserHub.Core.Domain.Entities.UserInfo", b =>
                {
                    b.Navigation("UserInfoImages");
                });
#pragma warning restore 612, 618
        }
    }
}
