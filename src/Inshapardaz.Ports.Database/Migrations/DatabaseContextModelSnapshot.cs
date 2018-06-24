﻿// <auto-generated />
using Inshapardaz.Domain.Entities;
using Inshapardaz.Domain.Entities.Dictionary;
using Inshapardaz.Ports.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Inshapardaz.Ports.Database.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Inshapardaz")
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.Dictionary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsPublic");

                    b.Property<int>("Language");

                    b.Property<string>("Name")
                        .HasMaxLength(255);

                    b.Property<Guid>("UserId")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Dictionary","Inshapardaz");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.DictionaryDownload", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DictionaryId");

                    b.Property<int>("FileId");

                    b.Property<string>("MimeType");

                    b.HasKey("Id");

                    b.HasIndex("DictionaryId");

                    b.HasIndex("FileId");

                    b.ToTable("DictionaryDownload","Inshapardaz");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.Meaning", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Context");

                    b.Property<string>("Example");

                    b.Property<string>("Value");

                    b.Property<long>("WordId");

                    b.HasKey("Id");

                    b.HasIndex("WordId");

                    b.ToTable("Meaning","Inshapardaz");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.Translation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsTrasnpiling");

                    b.Property<int>("Language");

                    b.Property<string>("Value");

                    b.Property<long>("WordId");

                    b.HasKey("Id");

                    b.HasIndex("WordId");

                    b.ToTable("Translation","Inshapardaz");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.Word", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Attributes");

                    b.Property<string>("Description");

                    b.Property<int>("DictionaryId");

                    b.Property<int>("Language");

                    b.Property<string>("Pronunciation");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("TitleWithMovements");

                    b.HasKey("Id");

                    b.HasIndex("DictionaryId");

                    b.HasIndex("Title");

                    b.ToTable("Word","Inshapardaz");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.WordRelation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("RelatedWordId");

                    b.Property<int>("RelationType");

                    b.Property<long>("SourceWordId");

                    b.HasKey("Id");

                    b.HasIndex("RelatedWordId");

                    b.HasIndex("SourceWordId");

                    b.ToTable("WordRelation","Inshapardaz");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Contents");

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("FileName");

                    b.Property<DateTime>("LiveUntil");

                    b.Property<string>("MimeType");

                    b.HasKey("Id");

                    b.ToTable("File","Inshapardaz");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Author","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AuthorId");

                    b.Property<string>("Description");

                    b.Property<int?>("ImageId");

                    b.Property<bool>("IsPublic");

                    b.Property<int>("Language");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ImageId");

                    b.ToTable("Book","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.BookGenre", b =>
                {
                    b.Property<int>("BookId");

                    b.Property<int>("GenereId");

                    b.HasKey("BookId", "GenereId");

                    b.HasIndex("GenereId");

                    b.ToTable("BookGenre","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.BookImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Image");

                    b.HasKey("Id");

                    b.ToTable("BookImage","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.Chapter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BookId");

                    b.Property<int>("ChapterNumber");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.ToTable("Chapter","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.ChapterText", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChapterId");

                    b.Property<string>("Content");

                    b.HasKey("Id");

                    b.HasIndex("ChapterId")
                        .IsUnique();

                    b.ToTable("ChapterText","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Genre","Library");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.DictionaryDownload", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Dictionary.Dictionary", "Dictionary")
                        .WithMany("Downloads")
                        .HasForeignKey("DictionaryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Inshapardaz.Ports.Database.Entities.File", "File")
                        .WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.Meaning", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Dictionary.Word", "Word")
                        .WithMany("Meaning")
                        .HasForeignKey("WordId")
                        .HasConstraintName("FK_Meaning_Word")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.Translation", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Dictionary.Word", "Word")
                        .WithMany("Translation")
                        .HasForeignKey("WordId")
                        .HasConstraintName("FK_Translation_Word")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.Word", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Dictionary.Dictionary", "Dictionary")
                        .WithMany("Word")
                        .HasForeignKey("DictionaryId")
                        .HasConstraintName("FK_Word_Dictionary")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Dictionary.WordRelation", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Dictionary.Word", "RelatedWord")
                        .WithMany("WordRelationRelatedWord")
                        .HasForeignKey("RelatedWordId")
                        .HasConstraintName("FK_WordRelation_RelatedWord")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Inshapardaz.Ports.Database.Entities.Dictionary.Word", "SourceWord")
                        .WithMany("WordRelationSourceWord")
                        .HasForeignKey("SourceWordId")
                        .HasConstraintName("FK_WordRelation_SourceWord")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.Book", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.BookImage", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.BookGenre", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Book", "Book")
                        .WithMany("Generes")
                        .HasForeignKey("GenereId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Genre", "Genre")
                        .WithMany("BookGeneres")
                        .HasForeignKey("GenereId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.Chapter", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Book", "Book")
                        .WithMany("Chapters")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Inshapardaz.Ports.Database.Entities.Library.ChapterText", b =>
                {
                    b.HasOne("Inshapardaz.Ports.Database.Entities.Library.Chapter", "Chapter")
                        .WithOne("Content")
                        .HasForeignKey("Inshapardaz.Ports.Database.Entities.Library.ChapterText", "ChapterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
