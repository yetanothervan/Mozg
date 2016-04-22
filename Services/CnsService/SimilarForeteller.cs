using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Interfaces;

namespace CnsService
{
    public class SimilarForeteller : IForeteller
    {
        private readonly DbSensor _mySensor;
        private readonly List<DbEffector> _affectors;
        private readonly ICellMemory _cellMemory;
        private readonly ICnsState _cnsState;
        
        private double _a;
        private double _b;

        public SimilarForeteller(DbSensor mySensor, List<DbEffector> affectors, ICellMemory cellMemory, ICnsState cnsState)
        {
            _a = 0;
            _b = 0;
            _mySensor = mySensor;
            _affectors = affectors;
            _cellMemory = cellMemory;
            _cnsState = cnsState;
            Init();
        }

        private void Init()
        {
            if (_affectors.Count != 1)
                throw new NotImplementedException();

            //for one E
            //Snext = aS + bE
            //suppose a == 0
            var a0 = E1SupposeA0();

            if (a0) _a = 0;
            else E1A1GetB(); //suppose a == 1
        }

        private void E1A1GetB()
        {
            var effVals = _cellMemory.GetEffectorValues(_affectors[0], _cnsState.TimeMoment);
            var sensVals = _cellMemory.GetSensorValues(_mySensor, _cnsState.TimeMoment);

            //suppose the b is single
            bool bFound = false;
            for (int i = sensVals.Count - 2; i >= 0; --i)
            {
                var S = sensVals[i + 1];
                var Snext = sensVals[i];
                var e = effVals[i];

                if (Math.Abs(e) < _affectors[0].Tolerance) continue;

                //Snext = aS + bE  =>  b = (Snext - aS) / E
                if (bFound && Math.Abs(_b - (Snext - S) / e) > _affectors[0].Tolerance) throw new NotImplementedException(); //b differs

                _b = (Snext - S) / e;
                bFound = true;
            }
            //b single   _b == b
            _a = 1;
        }
        
        private bool E1SupposeA0()
        {
            var effVals = _cellMemory.GetEffectorValues(_affectors[0], _cnsState.TimeMoment);
            var sensVals = _cellMemory.GetSensorValues(_mySensor, _cnsState.TimeMoment);

            //suppose the b is single
            bool bFound = false;
            for (int i = sensVals.Count - 2; i >= 0; --i)
            {
                var snext = sensVals[i];
                var e = effVals[i];

                if (Math.Abs(e) < _affectors[0].Tolerance) continue;

                if (bFound && Math.Abs(_b - (snext/e)) > _affectors[0].Tolerance) return false; //b differs

                _b = snext/e;
                bFound = true;
            }
            return true; //b single
        }

        public double Foretell()
        {
            //Snext = aS + bE
            return _a*_cellMemory.LastValue(_mySensor) + _b*_cnsState.EffectorsNextValues[_affectors[0].Id];
        }

        public bool Improve()
        {
            Init();
            return true;
        }
    }
}