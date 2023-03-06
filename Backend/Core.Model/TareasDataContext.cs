using Core.Model.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public partial class TareasDataContext: DbContext
    {
        public TareasDataContext(DbContextOptions<TareasDataContext> options): base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseSqlServer(@"Data Source=190.8.176.206\MSSQLSERVER2019;Initial Catalog=cmsoli_ifgroup;User Id=cmsoli_ifgroupAdmin;Password=ifGroupAdmin*2023;");
        }
        /// <summary>
        /// Crear el modelo cuando se instancia el proyecto
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<Tarea> Tarea { get; set; }

    }
}