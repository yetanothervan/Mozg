using System;
using Entities;
using Interfaces;

namespace CnsService
{
    public class Predictor
    {
        private readonly ICellMemory _cellMemory;
        private readonly ICnsState _cnsState;
        private readonly DbSensor _dbSensor;
        private IForeteller _foreteller;

        public Predictor(DbSensor dbSensor, ICellMemory cellMemory, ICnsState cnsState)
        {
            _cellMemory = cellMemory;
            _cnsState = cnsState;
            _dbSensor = dbSensor;
            _foreteller = new ConstantForeteller(_dbSensor, _cellMemory);
        }

        private double? _predictedValue;
        public void Predict()
        {
            _predictedValue = _foreteller.Foretell();
        }

        public bool IsPredictedWell()
        {
            double curVal = _cellMemory.LastValue(_dbSensor);
            
            if (_predictedValue == null) return false;
            return Math.Abs(_predictedValue.Value - curVal) < _dbSensor.Tolerance;
        }

        public void RefinePrediction()
        {
            if (_predictedValue == null) return;

            //remove constant foreteller (it's a gag)
            if (_foreteller is ConstantForeteller)
                _foreteller = null;

            if (_foreteller is SimilarForeteller)
            {
                var res = _foreteller.Improve();
                if (res) return;
            }

            //magic foreteller
            var effs = _cellMemory.GetEffectorsWithDifferentValuesLastTwoMoment();

            if (effs.Count != 1) //эффекторов с различными значениями больше чем один, такое пока не прогнозируем
                throw new NotImplementedException();

            _foreteller = new SimilarForeteller(_dbSensor, effs, _cellMemory, _cnsState);
        }

        public double? GetPredictedValue()
        {
            return _predictedValue;
        }
    }

    public class SensorValueInterval : ValueInterval
    {
        public IForeteller Foreteller { get; set; }
    }

    public class ConstantForeteller : IForeteller
    {
        private readonly DbSensor _dbSensor;
        private readonly ICellMemory _cellMemory;

        public ConstantForeteller(DbSensor dbSensor, ICellMemory cellMemory)
        {
            _dbSensor = dbSensor;
            _cellMemory = cellMemory;
        }

        public double Foretell()
        {
            return _cellMemory.LastValue(_dbSensor);
        }

        public bool Improve()
        {
            return false;
        }
    }
}