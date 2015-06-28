using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace CnsService
{
    public interface ICellMemory
    {
        List<DbEffector> GetEffectorsWithDifferentValuesLastTwoMoment();
        List<double> GetEffectorValues(DbEffector eff, int depth);
        List<double> GetSensorValues(DbSensor sensor, int depth);
        double LastValue(DbSensor dbSensor);
    }
}
