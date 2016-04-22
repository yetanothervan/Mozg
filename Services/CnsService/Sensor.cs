using Entities;
using Interfaces;

namespace CnsService
{
    public  class Sensor
    {
        public readonly ISensor SensorPhysical;
        private readonly DbSensor _dbSensor;
        private readonly ICellMemory _cellMemory;
        private readonly ICnsState _cnsState;

        public Sensor(ISensor sensorPhysical, DbSensor dbSensor, ICellMemory cellMemory, ICnsState cnsState)
        {
            SensorPhysical = sensorPhysical;
            _dbSensor = dbSensor;
            _cellMemory = cellMemory;
            _cnsState = cnsState;
        }

        public int DbSensorId { get { return _dbSensor.Id; } }

        public bool IsPredictedWell()
        {
            if (_myPredictor == null) return false;
            return _myPredictor.IsPredictedWell();
        }

        private Predictor _myPredictor;

        public void RefinePrediction()
        {
            if (_myPredictor == null)
                _myPredictor = new Predictor(_dbSensor, _cellMemory, _cnsState);
            _myPredictor.RefinePrediction();
            //TODO focus
        }
        
        public void AdvantageMoment()
        {
            
        }

        public void DoPredict()
        {
            _myPredictor.Predict();
        }

        public double? GetPredictedValue()
        {
            if (_myPredictor == null) return null;
            return _myPredictor.GetPredictedValue();
        }
    }
}