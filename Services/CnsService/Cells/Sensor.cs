using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CnsService.Predictors;
using Interfaces;

namespace CnsService.Cells
{
    public class Sensor : Cell
    {
        private readonly string _name;
        private readonly double _maxValue;
        private readonly double _minValue;
        private readonly int _id;
        private readonly ISensor _physical;
        private IPredictor _predictor;
        private PredictedValue _predictedValue;
        private readonly IDbCnsOut _dbCnsOut;
        private readonly double _tolerance;

        public Sensor(ISensor sensor, int id, IDbCnsOut dbCnsOut, double tolerance)
        {
            _name = sensor.Name;
            _maxValue = sensor.MaxValue;
            _minValue = sensor.MinValue;
            _id = id;
            _dbCnsOut = dbCnsOut;
            _tolerance = tolerance;
            _physical = sensor;
        }

        public int DbId { get { return _id; }}

        public double GetValue()
        {
            return _physical.Value;
        }

        public void DoPrediction()
        {
            if (_predictor == null)
                _predictor = new ConstantPredictor(_physical.Value);

            _predictedValue = new PredictedValue()
            {
                TimeMoment = _dbCnsOut.CurrentTimeMoment,
                Value =
                    _predictor.Predict(_dbCnsOut.CurrentTimeMoment)
            };
        }

        public bool PredictedWell()
        {
            if (_predictedValue == null || _predictedValue.TimeMoment + 1 != _dbCnsOut.CurrentTimeMoment)
                throw new Exception("not correct sequence");

            return !Util.DoubleDiffer(_physical.Value, _predictedValue.Value, _tolerance);
        }

        public void RefinePrediction()
        {
            if (_predictor is ConstantPredictor)
            {
                var effs = _dbCnsOut.GetEffectorsThatChangedLastMoment();
                if (effs.Count != 1)
                    throw new NotImplementedException();
                _predictor = new FirstGradePredictor(this, _dbCnsOut);
                _predictor.AddEffectorToWatch(effs.First());
            }

            _predictor.Refine();
        }
    }
}
