using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CDNVNCMS.Tube.Entities
{
    public class TubeContext : DbContext
    {
        public TubeContext() : base("TubeConnection") { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Film> Films { get; set; }

        public DbSet<FilmPart> FilmParts { get; set; }
        public DbSet<Setting> Settings { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new FilmConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}