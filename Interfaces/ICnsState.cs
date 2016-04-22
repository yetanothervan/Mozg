using System.Collections.Generic;

namespace Interfaces
{
    public interface ICnsState
    {
        int TimeMoment { get; }
        IReadOnlyDictionary<int, double> EffectorsNextValues { get; } 
    }
}