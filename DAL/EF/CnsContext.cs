using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace EF
{
    public class CnsContext : DbContext
    {
        public DbSet<DbSensor> DbSensors { get; set; }
        public DbSet<DbEffector> DbEffectors { get; set; }
        public DbSet<SensorEntry> SensorEntries { get; set; }
        public DbSet<EffectorEntry> EffectorEntries { get; set; }
    }
}
