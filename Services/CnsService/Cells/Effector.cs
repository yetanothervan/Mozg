using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace CnsService.Cells
{
    public class Effector : Cell
    {
        private readonly string _name;
        private readonly double _maxValue;
        private readonly double _minValue;
        private readonly int _id;
        private readonly IEffector _physical;

        public Effector(IEffector effector, int id)
        {
            _name = effector.Name;
            _maxValue = effector.MaxValue;
            _minValue = effector.MinValue;
            _id = id;
            _physical = effector;
        }

        public int DbId { get { return _id; } }

        public double MaxValue
        {
            get { return _maxValue; }
        }

        public double MinValue
        {
            get { return _minValue; }
        }

        public string Name
        {
            get { return _name; }
        }

        public double GetValue()
        {
            return _physical.Value;
        }

        public double GetNextValue()
        {
            return _physical.NextValue;
        }

        public void SetNextValue(double value)
        {
            _physical.NextValue = value;
        }
    }
}
