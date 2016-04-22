using System.Collections.Generic;

namespace Interfaces
{
    public interface ICreature
    {
        List<Action> GetActions();
        void AdvantageMoment();
        ICnsDiagnostics CnsDiagnostics { get; }
    }
}
