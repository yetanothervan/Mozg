namespace Interfaces
{
    public interface IEffector
    {
        string Name { get; }
        double Value { get; }
        double NextValue { get; set; }
        double MinValue { get; }
        double MaxValue { get; }
    }
}