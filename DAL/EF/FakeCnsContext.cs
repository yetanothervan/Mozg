using System.Data.Entity;
using DataGetServiceTests.Infrastructure;
using Entities;

namespace EF
{
    public class FakeCnsContext : ICnsContext
    {
        public FakeCnsContext()
        {
            DbEffectors = new FakeDbSet<DbEffector>();
            DbSensors = new FakeDbSet<DbSensor>();
            CellEntries = new FakeDbSet<CellEntry>();
        }
        
        public DbSet<DbSensor> DbSensors { get; set; }
        public DbSet<DbEffector> DbEffectors { get; set; }
        public DbSet<CellEntry> CellEntries { get; set; }
        
        public int SaveChanges()
        {
            return 0;
        }
    }
}
