using Interfaces;

namespace WorldService.Bug
{
    public class LegEffector : IEffector
    {
        public LegEffector(string name, double effMin, double effMax)
        {
            Name = name;
            Value = 0.0;
            NextValue = 0.0;
            MinValue = effMin;
            MaxValue = effMax;
        }

        public string Name { get; private set; }

        public double Value { get; set; }

        public double NextValue { get; set; }

        public double MinValue { get; private set; }

        public double MaxValue { get; private set; }
    }
}