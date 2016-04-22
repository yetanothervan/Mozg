﻿using System.Collections.Generic;
using Interfaces;

namespace CnsService
{
    public class CnsState : ICnsState
    {
        private readonly Cns _myCns;

        public CnsState(Cns myCns, IDictionary<int, double> effectorsNextValues)
        {
            _myCns = myCns;
            _effectorsNextValues = new Dictionary<int, double>(effectorsNextValues);
        }

        public int TimeMoment
        {
            get { return _myCns.TimeMoment; }
        }

        public Dictionary<int, double> _effectorsNextValues;
        public IReadOnlyDictionary<int, double> EffectorsNextValues
        {
            get { return _effectorsNextValues; }
        }
    }
}
