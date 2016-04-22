namespace Interfaces
{
    public interface ICnsDiagnostics
    {
        ICellMemory CellMemory { get; }
        ICnsState CnsState { get; }
        double? GetPredictedValueForSensor(int sensorId);
    }
}
