using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using MapraiScheduler.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace MapraiScheduler.Repositories
{
    public class MapRaiContex : DbContext
    {
        public MapRaiContex(DbContextOptions<MapRaiContex> options)
            : base(options)
        { }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseMySQL(Statics.MySQLConnectionString);
        //}

        public DbSet<Notify> Notifiers { get; set; }
        public DbSet<NotifyType> NotifyTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Depo> Depos { get; set; }
    }
}