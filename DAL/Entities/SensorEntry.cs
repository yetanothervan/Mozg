using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class SensorEntry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int DbSensorId { get; set; }
        public int TimeMoment { get; set; }
        public double Value { get; set; }
        public virtual DbSensor DbSensor { get; set; }
    }
}
