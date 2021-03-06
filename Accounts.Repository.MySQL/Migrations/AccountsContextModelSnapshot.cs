﻿// <auto-generated />
using System;
using Accounts.Repository.MySQL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Accounts.Repository.MySQL.Migrations
{
    [DbContext(typeof(AccountsContext))]
    partial class AccountsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Accounts.Entities.Grant", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Action")
                        .HasColumnName("action");

                    b.Property<bool>("Active")
                        .HasColumnName("active");

                    b.Property<string>("ClientID")
                        .IsRequired()
                        .HasColumnName("clientId");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnName("code");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("createdAt")
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Description")
                        .HasColumnName("desc");

                    b.Property<DateTime?>("RemovedAt")
                        .HasColumnName("removedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("updatedAt")
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("now()");

                    b.HasKey("ID");

                    b.HasIndex("ClientID");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("grants");
                });

            modelBuilder.Entity("Accounts.Entities.Profile", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active")
                        .HasColumnName("active");

                    b.Property<string>("ClientID")
                        .IsRequired()
                        .HasColumnName("clientId");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("createdAt")
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Description")
                        .HasColumnName("desc");

                    b.Property<DateTime?>("RemovedAt")
                        .HasColumnName("removedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("updatedAt")
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("now()");

                    b.HasKey("ID");

                    b.HasIndex("ClientID");

                    b.ToTable("profiles");
                });

            modelBuilder.Entity("Accounts.Entities.ProfileGrant", b =>
                {
                    b.Property<int>("ProfileID");

                    b.Property<int>("GrantID");

                    b.HasKey("ProfileID", "GrantID");

                    b.HasIndex("GrantID");

                    b.ToTable("profile_grants");
                });

            modelBuilder.Entity("Accounts.Entities.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active")
                        .HasColumnName("active");

                    b.Property<string>("ClientID")
                        .IsRequired()
                        .HasColumnName("clientId");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("createdAt")
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email");

                    b.Property<DateTimeOffset?>("LastLogin")
                        .HasColumnName("lastLogin")
                        .HasColumnType("datetime");

                    b.Property<string>("LocationID")
                        .HasColumnName("locationId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("password");

                    b.Property<DateTime?>("RemovedAt")
                        .HasColumnName("removedAt")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("updatedAt")
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnName("username");

                    b.HasKey("ID");

                    b.HasIndex("ClientID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username");

                    b.HasIndex("Username", "Password");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Accounts.Entities.UserGrant", b =>
                {
                    b.Property<int>("UserID");

                    b.Property<int>("GrantID");

                    b.HasKey("UserID", "GrantID");

                    b.HasIndex("GrantID");

                    b.ToTable("user_grants");
                });

            modelBuilder.Entity("Accounts.Entities.UserProfile", b =>
                {
                    b.Property<int>("UserID");

                    b.Property<int>("ProfileID");

                    b.HasKey("UserID", "ProfileID");

                    b.HasIndex("ProfileID");

                    b.ToTable("user_profiles");
                });

            modelBuilder.Entity("Accounts.Entities.ProfileGrant", b =>
                {
                    b.HasOne("Accounts.Entities.Grant", "Grant")
                        .WithMany("Profiles")
                        .HasForeignKey("GrantID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Accounts.Entities.Profile", "Profile")
                        .WithMany("Grants")
                        .HasForeignKey("ProfileID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Accounts.Entities.UserGrant", b =>
                {
                    b.HasOne("Accounts.Entities.Grant", "Grant")
                        .WithMany("Users")
                        .HasForeignKey("GrantID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Accounts.Entities.User", "User")
                        .WithMany("Grants")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Accounts.Entities.UserProfile", b =>
                {
                    b.HasOne("Accounts.Entities.Profile", "Profile")
                        .WithMany("Users")
                        .HasForeignKey("ProfileID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Accounts.Entities.User", "User")
                        .WithMany("Profiles")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
