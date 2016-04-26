using System.Data.Entity;
using Entities;

namespace EF
{
    public interface ICnsContext
    {
        DbSet<DbSensor> DbSensors { get; set; }
        DbSet<DbEffector> DbEffectors { get; set; }
        DbSet<CellEntry> CellEntries { get; set; }
        int SaveChanges();

    }
}