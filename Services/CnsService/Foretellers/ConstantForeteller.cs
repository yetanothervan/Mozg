namespace CnsService.Foretellers
{
    public class ConstantForeteller : IForeteller
    {
        private readonly double _value;

        public ConstantForeteller(double value)
        {
            _value = value;
        }

        public double Foretell()
        {
            return _value;
        }

        public void Improve()
        {
        }
    }
}