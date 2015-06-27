using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;

namespace CnsService
{
    public class Predictor
    {
        private readonly ISensor _sensorPhysical;
        private List<SensorValueInterval> _intervals;

        public Predictor(ISensor sensorPhysical)
        {
            _sensorPhysical = sensorPhysical;
            _intervals = new List<SensorValueInterval>();
            var init = new SensorValueInterval() {Ceiling = double.PositiveInfinity, Floor = double.NegativeInfinity};
            init.Foreteller = new ConstantForeteller(_sensorPhysical);
            _intervals.Add(init);
        }

        private double? _predictedValue;
        public void Predict()
        {
            var interval = _intervals.First(i => i.Ceiling >= _sensorPhysical.Value && i.Floor < _sensorPhysical.Value);
            _predictedValue = interval.Foreteller.Foretell();
        }

        public bool IsPredictedWell()
        {
            if (_predictedValue == null) return false;
            return Math.Abs(_predictedValue.Value - _sensorPhysical.Value) < _sensorPhysical.Tolerance;
        }
    }

    public class SensorValueInterval : ValueInterval
    {
        public IForeteller Foreteller { get; set; }
    }

    public interface IForeteller
    {
        double Foretell();
    }

    public class ConstantForeteller : IForeteller
    {
        private readonly ISensor _sensorPhysical;

        public ConstantForeteller(ISensor sensorPhysical)
        {
            _sensorPhysical = sensorPhysical;
        }

        public double Foretell()
        {
            return _sensorPhysical.Value;
        }
    }
}