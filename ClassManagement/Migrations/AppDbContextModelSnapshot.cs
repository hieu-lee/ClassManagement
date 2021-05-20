﻿// <auto-generated />
using System;
using ClassManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ClassManagement.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("ClassManagement.Models.Class", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<string>("DeleteHeight")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Code");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("ClassManagement.Models.ClassNote", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClassroomCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Day")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClassroomCode");

                    b.ToTable("ClassNotes");
                });

            modelBuilder.Entity("ClassManagement.Models.ClassSchedule", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClassroomCode")
                        .HasColumnType("TEXT");

                    b.Property<int>("Day")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan?>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan?>("StartTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClassroomCode");

                    b.ToTable("ClassSchedules");
                });

            modelBuilder.Entity("ClassManagement.Models.Grade", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClassCode")
                        .HasColumnType("TEXT");

                    b.Property<double>("GradeinNum")
                        .HasColumnType("REAL");

                    b.Property<string>("StdName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StudentId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClassCode");

                    b.HasIndex("StudentId");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("ClassManagement.Models.Student", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClassesCodes")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("TEXT");

                    b.Property<string>("Gender")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("ClassStudent", b =>
                {
                    b.Property<string>("ClassesCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("StudentsId")
                        .HasColumnType("TEXT");

                    b.HasKey("ClassesCode", "StudentsId");

                    b.HasIndex("StudentsId");

                    b.ToTable("ClassStudent");
                });

            modelBuilder.Entity("ClassManagement.Models.ClassNote", b =>
                {
                    b.HasOne("ClassManagement.Models.Class", "Classroom")
                        .WithMany("Notes")
                        .HasForeignKey("ClassroomCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Classroom");
                });

            modelBuilder.Entity("ClassManagement.Models.ClassSchedule", b =>
                {
                    b.HasOne("ClassManagement.Models.Class", "Classroom")
                        .WithMany("Schedules")
                        .HasForeignKey("ClassroomCode");

                    b.Navigation("Classroom");
                });

            modelBuilder.Entity("ClassManagement.Models.Grade", b =>
                {
                    b.HasOne("ClassManagement.Models.Class", "Classroom")
                        .WithMany("Grades")
                        .HasForeignKey("ClassCode");

                    b.HasOne("ClassManagement.Models.Student", "Student")
                        .WithMany("Grades")
                        .HasForeignKey("StudentId");

                    b.Navigation("Classroom");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("ClassStudent", b =>
                {
                    b.HasOne("ClassManagement.Models.Class", null)
                        .WithMany()
                        .HasForeignKey("ClassesCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClassManagement.Models.Student", null)
                        .WithMany()
                        .HasForeignKey("StudentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ClassManagement.Models.Class", b =>
                {
                    b.Navigation("Grades");

                    b.Navigation("Notes");

                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("ClassManagement.Models.Student", b =>
                {
                    b.Navigation("Grades");
                });
#pragma warning restore 612, 618
        }
    }
}
