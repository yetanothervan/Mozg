using System.Collections;
using System.Collections.Generic;
using Entities;

namespace Interfaces
{
    public interface ICnsDiagnostics
    {
        ICellMemory CellMemory { get; }
        ICnsState CnsState { get; }
        double? GetPredictedValueForSensor(int sensorId);
    }
}
