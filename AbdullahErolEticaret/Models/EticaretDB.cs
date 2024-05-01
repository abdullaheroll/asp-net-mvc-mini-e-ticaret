using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace AbdullahErolEticaret.Models
{
    public class EticaretDB : DbContext
    {
        public EticaretDB()
        {
            Database.SetInitializer<EticaretDB>(null);
        }

        public DbSet<KullanicilarT> kullanicilar { get; set; }
        public DbSet<UrunlerT> UrunlerT { get; set; }
        public DbSet<KategorilerT> kategorilerT { get; set; }
        public DbSet<IletisimT> IletisimT { get; set; }
        public DbSet<HakkimizdaT> HakkimizdaT { get; set; }
        public DbSet<SiparislerT> SiparislerT { get; set; }
        public DbSet<SiparisDetayT> SiparisDetayT { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

    }
}