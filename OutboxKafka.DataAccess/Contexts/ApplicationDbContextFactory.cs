using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OutboxKafka.DataAccess.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutboxKafka.DataAccess.Contexts
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            // Настройка подключения к PostgreSQL
            var connStr = "Host=localhost;Port=5432;Username=postgres;Password=orcl;Database=Outbox_Pattern_Demo";
            optionsBuilder.UseNpgsql(connStr);
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}


