using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace CnsService
{
    public class Cns : ICns
    {
        private readonly List<Sensor> _sensors;
        private readonly List<Sensor> _targetSensors;
        private readonly List<Effector> _effectors;

        public Cns()
        {
            _sensors = new List<Sensor>();
            _targetSensors = new List<Sensor>();
            _effectors = new List<Effector>();
        }

        public void AddSensor(ISensor s)
        {
            _sensors.Add(new Sensor(s));
        }

        public void AddTargetSensor(ISensor ts)
        {
            _targetSensors.Add(new Sensor(ts));
        }

        public void AddEffector(IEffector e)
        {
            _effectors.Add(new Effector(e));
        }

        public void SetEffectors()
        {
            var poorPredictors = GetPoorPredictors();
            if (poorPredictors != null && poorPredictors.Count > 0)
            {
                foreach (var predictor in poorPredictors)
                    predictor.RefinePrediction();
            }
            else
            {
                var poorlyResearchedEffector = _effectors.FirstOrDefault(e => !e.IsResearchedWell());
                if (poorlyResearchedEffector != null)
                    DoResearchEffector(poorlyResearchedEffector);
                else
                    DoBestStrategy();
            }
        }

        public void DoPrediction()
        {
            foreach (var sensor in _sensors)
                sensor.DoPredict();

            foreach (var sensor in _targetSensors)
                sensor.DoPredict();
        }

        private void DoBestStrategy()
        {
            throw new NotImplementedException();
        }

        private Effector _toResearchEffector;
        private void DoResearchEffector(Effector effector)
        {
            _toResearchEffector = effector;
            effector.DoResearch();
        }

        private List<Sensor> GetPoorPredictors()
        {
            var result = _sensors.Where(s => !s.IsPredictedWell()).ToList();
            result.AddRange(_targetSensors.Where(s => !s.IsPredictedWell()));
            return result;
        }

        public void AdvantageMoment()
        {
            if (_toResearchEffector != null)
            {
                _toResearchEffector.SetReseached(_toResearchEffector.PhysicalEffector.Value);
            }
        }
    }
}
