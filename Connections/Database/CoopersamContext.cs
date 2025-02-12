﻿using Coopersam_WebAPI_CS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Coopersam_WebAPI_CS.Connections.Database
{
    public class CoopersamContext : DbContext
    {
        public CoopersamContext(DbContextOptions options) : base(options)
        {
        }

        #region DBSet

        public DbSet<Erro> ErrosLog { get; set; }
        public DbSet<Ocorrencia> OcorrenciaLog { get; set; }

        #endregion DBSet

        public CoopersamContext() : base()
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
            optionsBuilder.UseMySql(configuration.GetConnectionString("CoopersamConnection"), new MySqlServerVersion(new Version(8, 0, 23)));
        }
    }
}