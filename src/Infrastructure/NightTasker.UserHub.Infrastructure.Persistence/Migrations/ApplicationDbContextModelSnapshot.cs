﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NightTasker.UserHub.Infrastructure.Persistence;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NightTasker.UserHub.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NightTasker.UserHub.Core.Domain.Entities.Organization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTimeOffset>("CreatedDateTimeOffset")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date_time_offset");

                    b.Property<string>("Description")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("character varying(254)")
                        .HasColumnName("name");

                    b.Property<DateTimeOffset?>("UpdatedDateTimeOffset")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_date_time_offset");

                    b.HasKey("Id")
                        .HasName("pk_organizations");

                    b.ToTable("organizations", (string)null);
                });

            modelBuilder.Entity("NightTasker.UserHub.Core.Domain.Entities.OrganizationUser", b =>
                {
                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uuid")
                        .HasColumnName("organization_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("Role")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Member")
                        .HasColumnName("role");

                    b.HasKey("OrganizationId", "UserId")
                        .HasName("pk_organization_users");

                    b.HasIndex("Role")
                        .HasDatabaseName("ix_organization_users_role");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_organization_users_user_id");

                    b.ToTable("organization_users", (string)null);
                });

            modelBuilder.Entity("NightTasker.UserHub.Core.Domain.Entities.User", b =>
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
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("NightTasker.UserHub.Core.Domain.Entities.UserImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("ContentType")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)")
                        .HasColumnName("content_type");

                    b.Property<DateTimeOffset>("CreatedDateTimeOffset")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date_time_offset");

                    b.Property<string>("Extension")
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("extension");

                    b.Property<string>("FileName")
                        .HasMaxLength(254)
                        .HasColumnType("character varying(254)")
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
                        .HasName("pk_user_images");

                    b.HasIndex("UserInfoId")
                        .HasDatabaseName("ix_user_images_user_info_id");

                    b.ToTable("user_images", (string)null);
                });

            modelBuilder.Entity("NightTasker.UserHub.Core.Domain.Entities.OrganizationUser", b =>
                {
                    b.HasOne("NightTasker.UserHub.Core.Domain.Entities.Organization", "Organization")
                        .WithMany("OrganizationUsers")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_organization_users_organizations_organization_id");

                    b.HasOne("NightTasker.UserHub.Core.Domain.Entities.User", "User")
                        .WithMany("OrganizationUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_organization_users_user_user_id");

                    b.Navigation("Organization");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NightTasker.UserHub.Core.Domain.Entities.UserImage", b =>
                {
                    b.HasOne("NightTasker.UserHub.Core.Domain.Entities.User", "UserInfo")
                        .WithMany("UserInfoImages")
                        .HasForeignKey("UserInfoId")
                        .HasConstraintName("fk_user_images_user_user_info_id");

                    b.Navigation("UserInfo");
                });

            modelBuilder.Entity("NightTasker.UserHub.Core.Domain.Entities.Organization", b =>
                {
                    b.Navigation("OrganizationUsers");
                });

            modelBuilder.Entity("NightTasker.UserHub.Core.Domain.Entities.User", b =>
                {
                    b.Navigation("OrganizationUsers");

                    b.Navigation("UserInfoImages");
                });
#pragma warning restore 612, 618
        }
    }
}
