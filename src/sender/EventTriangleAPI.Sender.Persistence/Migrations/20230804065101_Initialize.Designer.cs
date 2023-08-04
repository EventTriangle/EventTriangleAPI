﻿// <auto-generated />
using System;
using EventTriangleAPI.Sender.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EventTriangleAPI.Sender.Persistence.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20230804065101_Initialize")]
    partial class Initialize
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EventTriangleAPI.Sender.Domain.Entities.ContactCreatedEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ContactId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ContactCreatedEvents");
                });

            modelBuilder.Entity("EventTriangleAPI.Sender.Domain.Entities.ContactDeletedEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ContactId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ContactDeletedEvents");
                });

            modelBuilder.Entity("EventTriangleAPI.Sender.Domain.Entities.CreditCardAddedEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CardId")
                        .HasColumnType("uuid");

                    b.Property<string>("CardNumber")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Cvv")
                        .HasColumnType("text");

                    b.Property<string>("Expiration")
                        .HasColumnType("text");

                    b.Property<string>("HolderName")
                        .HasColumnType("text");

                    b.Property<int>("PaymentNetwork")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CreditCardAddedEvents");
                });

            modelBuilder.Entity("EventTriangleAPI.Sender.Domain.Entities.CreditCardChangedEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CardId")
                        .HasColumnType("uuid");

                    b.Property<string>("CardNumber")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Cvv")
                        .HasColumnType("text");

                    b.Property<string>("Expiration")
                        .HasColumnType("text");

                    b.Property<string>("HolderName")
                        .HasColumnType("text");

                    b.Property<int>("PaymentNetwork")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CreditCardChangedEvents");
                });

            modelBuilder.Entity("EventTriangleAPI.Sender.Domain.Entities.CreditCardDeletedEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CardId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CreditCardDeletedEvents");
                });

            modelBuilder.Entity("EventTriangleAPI.Sender.Domain.Entities.SupportTicketOpenedEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TicketReason")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.Property<Guid>("WalletId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("SupportTicketOpenedEvents");
                });

            modelBuilder.Entity("EventTriangleAPI.Sender.Domain.Entities.SupportTicketResolvedEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("TicketId")
                        .HasColumnType("uuid");

                    b.Property<string>("TicketJustification")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SupportTicketResolvedEvents");
                });

            modelBuilder.Entity("EventTriangleAPI.Sender.Domain.Entities.TransactionCreatedEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("From")
                        .HasColumnType("text");

                    b.Property<string>("To")
                        .HasColumnType("text");

                    b.Property<int>("TransactionType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("TransactionCreatedEvents");
                });

            modelBuilder.Entity("EventTriangleAPI.Sender.Domain.Entities.TransactionRollBackedEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("TransactionRollBackedEvents");
                });

            modelBuilder.Entity("EventTriangleAPI.Sender.Domain.Entities.UserCreatedEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<int>("UserRole")
                        .HasColumnType("integer");

                    b.Property<int>("UserStatus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("UserCreatedEvents");
                });

            modelBuilder.Entity("EventTriangleAPI.Sender.Domain.Entities.UserNotSuspendedEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserNotSuspendedEvents");
                });

            modelBuilder.Entity("EventTriangleAPI.Sender.Domain.Entities.UserRoleUpdatedEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<int>("UserRole")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("UserRoleUpdatedEvents");
                });

            modelBuilder.Entity("EventTriangleAPI.Sender.Domain.Entities.UserSuspendedEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserSuspendedEvents");
                });
#pragma warning restore 612, 618
        }
    }
}