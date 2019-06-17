﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApplication1.Models;

namespace WebApplication1.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("WebApplication1.Models.Invoice", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("inv_date");

                    b.HasKey("id");

                    b.ToTable("invoices");
                });

            modelBuilder.Entity("WebApplication1.Models.InvoiceDetail", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Invoiceid");

                    b.Property<double>("price");

                    b.Property<string>("product");

                    b.Property<int>("quantity");

                    b.HasKey("id");

                    b.HasIndex("Invoiceid");

                    b.ToTable("InvoiceDetails");
                });

            modelBuilder.Entity("WebApplication1.Models.InvoiceDetail", b =>
                {
                    b.HasOne("WebApplication1.Models.Invoice")
                        .WithMany("invoiceDetails")
                        .HasForeignKey("Invoiceid")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
