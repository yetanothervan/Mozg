namespace Interfaces
{
    public interface ILeg
    {
        string Name { get; }
        double MinAngle { get; }
        double MaxAngle { get; }
        double HorAngle { get; }
        double VerAngle { get; }
    }
}