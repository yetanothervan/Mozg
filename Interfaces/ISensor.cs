namespace Interfaces
{
    public interface ISensor
    {
        string Name { get; }
        double Value { get; }
        double MaxValue { get; }
        double MinValue { get; }
        double Tolerance { get; }
    }
}