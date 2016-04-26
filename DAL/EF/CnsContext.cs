using System.Data.Common;
using System.Data.Entity;
using Entities;

namespace EF
{
    public class CnsContext : DbContext, ICnsContext
    {
        public CnsContext(DbConnection connection) : base(connection, true)
        {
        }
        public CnsContext()
        {
        }
        public DbSet<DbSensor> DbSensors { get; set; }
        public DbSet<DbEffector> DbEffectors { get; set; }
        public DbSet<CellEntry> CellEntries { get; set; }
    }
}
