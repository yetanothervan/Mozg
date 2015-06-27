using System.Runtime.Remoting.Messaging;
using Interfaces;

namespace CnsService
{
    public  class Sensor
    {
        private readonly ISensor _sensorPhysical;

        public Sensor(ISensor sensorPhysical)
        {
            _sensorPhysical = sensorPhysical;
        }

        public bool IsPredictedWell()
        {
            if (_myPredictor == null) return false;
            return _myPredictor.IsPredictedWell();
        }

        private Predictor _myPredictor;

        public void RefinePrediction()
        {
            if (_myPredictor == null)
                _myPredictor = new Predictor(_sensorPhysical);
            //TODO focus

        }

        public void AdvantageMoment()
        {
            
        }

        public void DoPredict()
        {
            _myPredictor.Predict();
        }
    }
}