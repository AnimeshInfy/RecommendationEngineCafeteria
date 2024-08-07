﻿using Data.Models;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DataAccess
{
    public class CafeteriaDbContext : DbContext
    {
        public CafeteriaDbContext() { } 
        public CafeteriaDbContext(DbContextOptions<CafeteriaDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<MenuItems> MenuItems { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AllSentiments> Sentiments { get; set; }
        public DbSet<RolledOutItems> RolledOutItems { get; set; }   
        public DbSet<VotedItems> VotedItems { get; set; }
        public DbSet<Profile> Profile { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server=ITT-ANIMESH-SH\\SQLEXPRESS;Database=CafeteriaDB;Trusted_Connection=True;";
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.User)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.MenuItems)
                .WithMany()
                .HasForeignKey(f => f.MenuItemId);
        }
    }
}