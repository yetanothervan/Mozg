using System.Collections.Generic;
using Entities;

namespace Interfaces
{
    public interface ICellMemory
    {
        List<DbEffector> GetEffectorsWithDifferentValuesLastTwoMoment();
        List<double> GetEffectorValues(DbEffector eff, int depth);
        List<double> GetSensorValues(DbSensor sensor, int depth);
        double LastValue(DbSensor dbSensor);
        List<DbEffector> GetEffectors();
        IList<DbSensor> GetSensors();
    }
}
