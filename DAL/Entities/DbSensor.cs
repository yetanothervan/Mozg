using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class DbSensor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Tolerance { get; set; }
    }
}
