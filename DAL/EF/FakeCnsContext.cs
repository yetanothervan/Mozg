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
            EffectorEntries = new FakeDbSet<EffectorEntry>();
            SensorEntries = new FakeDbSet<SensorEntry>();
        }
        
        public DbSet<DbSensor> DbSensors { get; set; }
        public DbSet<DbEffector> DbEffectors { get; set; }
        public DbSet<SensorEntry> SensorEntries { get; set; }
        public DbSet<EffectorEntry> EffectorEntries { get; set; }
        public int SaveChanges()
        {
            return 0;
        }
    }
}
