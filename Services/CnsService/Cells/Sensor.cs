using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace CnsService.Cells
{
    public class Sensor : Cell
    {
        private readonly string _name;
        private readonly double _maxValue;
        private readonly double _minValue;
        private readonly int _id;
        private readonly ISensor _physical;

        public Sensor(ISensor sensor, int id)
        {
            _name = sensor.Name;
            _maxValue = sensor.MaxValue;
            _minValue = sensor.MinValue;
            _id = id;
            _physical = sensor;
        }

        public int DbId { get { return _id; }}

        public double GetValue()
        {
            return _physical.Value;
        }
    }
}
