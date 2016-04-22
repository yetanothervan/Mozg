using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Interfaces;

namespace CnsService
{
    public partial class Cns 
    {
        public ICnsDiagnostics CnsDiagnostics
        {
            get { return this; }
        }

        public ICellMemory CellMemory
        {
            get { return _cellMemory; }
        }

        public ICnsState CnsState
        {
            get { return _state; }
        }

        public double? GetPredictedValueForSensor(int sensorId)
        {
            return _sensors.First(s => s.DbSensorId == sensorId).GetPredictedValue();
        }
    }
}
