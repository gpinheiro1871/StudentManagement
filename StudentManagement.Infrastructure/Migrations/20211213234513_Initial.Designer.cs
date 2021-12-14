﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentManagement.Infrastructure;

#nullable disable

namespace StudentManagement.Infrastructure.Migrations
{
    [DbContext(typeof(SchoolContext))]
    [Migration("20211213234513_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("StudentManagement.Domain.Models.Students.Course", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Course", (string)null);
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.Students.Disenrollment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("TEXT");

                    b.Property<long>("StudentId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("Disenrollment", (string)null);
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.Students.Enrollment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Grade")
                        .HasColumnType("INTEGER");

                    b.Property<long>("StudentId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("Enrollment", (string)null);
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.Students.Student", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Student", (string)null);
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.Students.Disenrollment", b =>
                {
                    b.HasOne("StudentManagement.Domain.Models.Students.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentManagement.Domain.Models.Students.Student", "Student")
                        .WithMany("Disenrollments")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.Students.Enrollment", b =>
                {
                    b.HasOne("StudentManagement.Domain.Models.Students.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentManagement.Domain.Models.Students.Student", "Student")
                        .WithMany("Enrollments")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.Students.Student", b =>
                {
                    b.OwnsOne("StudentManagement.Domain.Models.Students.Name", "Name", b1 =>
                        {
                            b1.Property<long>("StudentId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("First")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .HasColumnName("Firstname");

                            b1.Property<string>("Last")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .HasColumnName("Lastname");

                            b1.HasKey("StudentId");

                            b1.ToTable("Student");

                            b1.WithOwner()
                                .HasForeignKey("StudentId");
                        });

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("StudentManagement.Domain.Models.Students.Student", b =>
                {
                    b.Navigation("Disenrollments");

                    b.Navigation("Enrollments");
                });
#pragma warning restore 612, 618
        }
    }
}