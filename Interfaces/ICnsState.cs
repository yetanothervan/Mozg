using System.Collections.Generic;

namespace Interfaces
{
    public interface ICnsState
    {
        int TimeMoment { get; }
        int LastSavedMoment { get; }
        IReadOnlyDictionary<int, double> EffectorsNextValues { get; } 
    }
}