using Interfaces;

namespace WorldService.Bug
{
    public class BugSensor : ISensor
    {
        public BugSensor(string name, double minValue, double maxValue)
        {
            Name = name;
            MaxValue = maxValue;
            MinValue = minValue;
            Value = 0.0;
        }

        public string Name { get; private set; }

        private double _value;
        public double Value
        {
            get { return _value; }
            set
            {
                if (value > MaxValue)
                    _value = MaxValue;
                else if (value < MinValue)
                    _value = MinValue;
                else _value = value;
            }
        }

        public double MaxValue { get; private set; }

        public double MinValue { get; private set; }
    }
}