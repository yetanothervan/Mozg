using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class SensorEntry
    {
        public int Id { get; set; }
        public int TimeMoment { get; set; }
        public double Value { get; set; }
        public virtual DbSensor DbSensor { get; set; }
    }
}
