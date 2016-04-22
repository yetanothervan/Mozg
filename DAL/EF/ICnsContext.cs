using System.Data.Entity;
using Entities;

namespace EF
{
    public interface ICnsContext
    {
        DbSet<DbSensor> DbSensors { get; set; }
        DbSet<DbEffector> DbEffectors { get; set; }
        DbSet<SensorEntry> SensorEntries { get; set; }
        DbSet<EffectorEntry> EffectorEntries { get; set; }
        int SaveChanges();

    }
}