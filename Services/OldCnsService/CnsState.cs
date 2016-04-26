using System.Collections.Generic;
using Interfaces;

namespace CnsService
{
    public class CnsState : ICnsState
    {
        private readonly Cns _myCns;
        private ICellMemory _myMemory;

        public CnsState(Cns myCns, ICellMemory myMemory,  IDictionary<int, double> effectorsNextValues)
        {
            _myCns = myCns;
            _myMemory = myMemory;
            _effectorsNextValues = new Dictionary<int, double>(effectorsNextValues);
        }

        public int TimeMoment
        {
            get { return _myCns.TimeMoment; }
        }

        public int LastSavedMoment
        {
            get { return _myMemory.LastSavedMoment; }
        }

        public Dictionary<int, double> _effectorsNextValues;
        public IReadOnlyDictionary<int, double> EffectorsNextValues
        {
            get { return _effectorsNextValues; }
        }
    }
}
