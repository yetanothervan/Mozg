using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ICns
    {
        void AddSensor(ISensor s);
        void AddTargetSensor(ISensor ts);
        void AddEffector(IEffector e);
        void SetEffectors();
        void DoPrediction();
        void AdvantageMoment();
        ICnsDiagnostics CnsDiagnostics { get; }
    }
}
