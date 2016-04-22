using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class SensorEntry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int DbSensorId { get; set; }
        public int TimeMoment { get; set; }
        public double Value { get; set; }
        //public virtual DbSensor DbSensor { get; set; }
    }
}
