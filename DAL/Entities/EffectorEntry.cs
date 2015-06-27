using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class EffectorEntry
    {
        public int Id { get; set; }
        public int TimeMoment { get; set; }
        public double Value { get; set; }
        public virtual DbEffector DbEffector { get; set; }
    }
}
