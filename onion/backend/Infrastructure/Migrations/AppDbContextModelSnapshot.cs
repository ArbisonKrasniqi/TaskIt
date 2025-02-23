﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Board", b =>
                {
                    b.Property<int>("BoardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BoardId"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WorkspaceId")
                        .HasColumnType("int");

                    b.HasKey("BoardId");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("Boards");
                });

            modelBuilder.Entity("Domain.Entities.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<int>("TaskId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CommentId");

                    b.HasIndex("TaskId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Domain.Entities.Invite", b =>
                {
                    b.Property<int>("InviteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InviteId"));

                    b.Property<DateTime>("DateSent")
                        .HasColumnType("datetime2");

                    b.Property<string>("InviteStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InviteeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("InviterId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("WorkspaceId")
                        .HasColumnType("int");

                    b.HasKey("InviteId");

                    b.HasIndex("InviteeId");

                    b.HasIndex("InviterId");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("Invites");
                });

            modelBuilder.Entity("Domain.Entities.List", b =>
                {
                    b.Property<int>("ListId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ListId"));

                    b.Property<int>("BoardId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ListId");

                    b.HasIndex("BoardId");

                    b.ToTable("Lists");
                });

            modelBuilder.Entity("Domain.Entities.Members", b =>
                {
                    b.Property<int>("MemberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MemberId"));

                    b.Property<DateTime>("DateJoined")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("WorkspaceId")
                        .HasColumnType("int");

                    b.HasKey("MemberId");

                    b.HasIndex("UserId");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("Domain.Entities.StarredBoard", b =>
                {
                    b.Property<int>("StarredBoardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StarredBoardId"));

                    b.Property<int>("BoardId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("WorkspaceId")
                        .HasColumnType("int");

                    b.HasKey("StarredBoardId");

                    b.HasIndex("BoardId");

                    b.HasIndex("UserId");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("StarredBoards");
                });

            modelBuilder.Entity("Domain.Entities.Tasks", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TaskId"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<int>("ListId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TaskId");

                    b.HasIndex("ListId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Entities.Workspace", b =>
                {
                    b.Property<int>("WorkspaceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WorkspaceId"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("WorkspaceId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Workspaces");
                });

            modelBuilder.Entity("Domain.Entities.WorkspaceActivity", b =>
                {
                    b.Property<int>("WorkspaceActivityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WorkspaceActivityId"));

                    b.Property<DateTime>("ActionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ActionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EntityName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("WorkspaceId")
                        .HasColumnType("int");

                    b.HasKey("WorkspaceActivityId");

                    b.HasIndex("UserId");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("WorkspaceActivities");
                });

            modelBuilder.Entity("Domain.Entities.Board", b =>
                {
                    b.HasOne("Domain.Entities.Workspace", "Workspace")
                        .WithMany("Boards")
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("Domain.Entities.Comment", b =>
                {
                    b.HasOne("Domain.Entities.Tasks", "Task")
                        .WithMany("Comments")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Task");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Invite", b =>
                {
                    b.HasOne("Domain.Entities.User", "Invitee")
                        .WithMany("ReceivedInvites")
                        .HasForeignKey("InviteeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "Inviter")
                        .WithMany("SentInvites")
                        .HasForeignKey("InviterId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Workspace", "Workspace")
                        .WithMany("Invites")
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Invitee");

                    b.Navigation("Inviter");

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("Domain.Entities.List", b =>
                {
                    b.HasOne("Domain.Entities.Board", "Board")
                        .WithMany("Lists")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Board");
                });

            modelBuilder.Entity("Domain.Entities.Members", b =>
                {
                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Workspace", "Workspace")
                        .WithMany("Members")
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("Domain.Entities.StarredBoard", b =>
                {
                    b.HasOne("Domain.Entities.Board", "Board")
                        .WithMany("StarredBoards")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("StarredBoards")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Workspace", "Workspace")
                        .WithMany("StarredBoards")
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");

                    b.Navigation("User");

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("Domain.Entities.Tasks", b =>
                {
                    b.HasOne("Domain.Entities.List", "List")
                        .WithMany("Tasks")
                        .HasForeignKey("ListId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("List");
                });

            modelBuilder.Entity("Domain.Entities.Workspace", b =>
                {
                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("Workspaces")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.WorkspaceActivity", b =>
                {
                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("Activity")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Workspace", "Workspace")
                        .WithMany("Activity")
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("Domain.Entities.Board", b =>
                {
                    b.Navigation("Lists");

                    b.Navigation("StarredBoards");
                });

            modelBuilder.Entity("Domain.Entities.List", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Domain.Entities.Tasks", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Navigation("Activity");

                    b.Navigation("Comments");

                    b.Navigation("ReceivedInvites");

                    b.Navigation("SentInvites");

                    b.Navigation("StarredBoards");

                    b.Navigation("Workspaces");
                });

            modelBuilder.Entity("Domain.Entities.Workspace", b =>
                {
                    b.Navigation("Activity");

                    b.Navigation("Boards");

                    b.Navigation("Invites");

                    b.Navigation("Members");

                    b.Navigation("StarredBoards");
                });
#pragma warning restore 612, 618
        }
    }
}
