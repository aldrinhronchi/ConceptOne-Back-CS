using Microsoft.EntityFrameworkCore;
using TMODELOBASET_WebAPI_CS.Models.Entities;
using TMODELOBASET_WebAPI_CS.Models.Usuario;

namespace TMODELOBASET_WebAPI_CS.Connections
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> option) : base(option)
        {
        }

        public DatabaseContext() : base()
        {
        }

        #region Dbset

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cargo> Cargos { get; set; }

        public DbSet<Erro> ErrosLog { get; set; }
        public DbSet<Ocorrencia> OcorrenciaLog { get; set; }
        public DbSet<Permissoes> Permissoes { get; internal set; }
        public DbSet<Modulo> Modulos { get; internal set; }
        public DbSet<Pagina> Paginas { get; internal set; }

        #endregion Dbset

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
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DatabaseDB"));
        }
    }
}