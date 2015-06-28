using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class EffectorEntry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int DbEffectorId { get; set; }
        public int TimeMoment { get; set; }
        public double Value { get; set; }
        public virtual DbEffector DbEffector { get; set; }
    }
}
