using BensonCLI.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BensonCLI.Data
{
    public class BensonContext : DbContext
    {
        public DbSet<Repo> Repos { get; set; }

        public DbSet<WebApp> WebApps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=bensoncli.db");
        }
    }
}
