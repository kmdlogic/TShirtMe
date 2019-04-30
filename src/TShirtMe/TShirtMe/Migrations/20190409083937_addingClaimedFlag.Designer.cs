﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TShirtMe;

namespace TShirtMe.Migrations
{
    [DbContext(typeof(TShirtMeContext))]
    [Migration("20190409083937_addingClaimedFlag")]
    partial class addingClaimedFlag
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TShirtMe.EntryEntity", b =>
                {
                    b.Property<string>("EntryCode")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Claimed");

                    b.Property<string>("PhoneNumber");

                    b.HasKey("EntryCode");

                    b.ToTable("Entries");
                });
#pragma warning restore 612, 618
        }
    }
}
