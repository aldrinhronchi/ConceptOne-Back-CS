﻿using TMODELOBASET_WebAPI_CS.Models.Entities;
using TMODELOBASET_WebAPI_CS.Models.Usuario;
using Microsoft.EntityFrameworkCore;

namespace TMODELOBASET_WebAPI_CS.Connections.Database
{
    public class TMODELOBASETContext : DbContext
    {
        public TMODELOBASETContext(DbContextOptions options) : base(options)
        {
        }

        #region DBSet

        #region Usuario

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Modulo> Modulo { get; set; }
        public DbSet<Cargo> Cargo { get; set; }
        public DbSet<Pagina> Pagina { get; set; }
        public DbSet<Permissoes> Permissoes { get; set; }

        #endregion Usuario

        public DbSet<Erro> ErrosLog { get; set; }
        public DbSet<Ocorrencia> OcorrenciaLog { get; set; }

        #endregion DBSet

        public TMODELOBASETContext() : base()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
                modelBuilder.ApplyConfiguration(new Configuration());

                modelBuilder.ApplyGlobalConfigurations();

             */

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                                                                         .SetBasePath(Environment.CurrentDirectory)
                                                                         .AddJsonFile("appsettings.json")
                                                                         .Build();
            optionsBuilder.UseMySql(configuration.GetConnectionString("TMODELOBASETConnection"), new MySqlServerVersion(new Version(8, 0, 23)));
        }
    }
}