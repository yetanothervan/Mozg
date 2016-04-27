using System;

namespace CnsService.Predictors
{
    public class ConstantPredictor : IPredictor
    {
        private readonly double _value;
        private PredictedValue _predictedValue;

        public ConstantPredictor(double value)
        {
            _value = value;
            _predictedValue = null;
        }

        public double Predict(int tm)
        {
            if (_predictedValue == null || _predictedValue.TimeMoment != tm)
            {
                _predictedValue = new PredictedValue()
                {
                    TimeMoment = tm,
                    Value = _value
                };
            }
            return _predictedValue.Value;
        }
    }
}