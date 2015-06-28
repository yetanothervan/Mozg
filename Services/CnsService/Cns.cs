using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using EF;
using Entities;
using Interfaces;

namespace CnsService
{
    public class Cns : ICns
    {
        private readonly List<Sensor> _sensors;
        private readonly List<Sensor> _targetSensors;
        private readonly List<Effector> _effectors;
        private int _timeMoment;
        private readonly CellMemory _cellMemory;
        readonly CnsState _state;
        
        public Cns()
        {
            _sensors = new List<Sensor>();
            _targetSensors = new List<Sensor>();
            _effectors = new List<Effector>();
            _timeMoment = 0;
            _state = new CnsState(this, new Dictionary<int, double>());
            _cellMemory = new CellMemory(_state);
        }

        public int TimeMoment { get { return _timeMoment; }  }

        public void AddSensor(ISensor s)
        {
            var dbSensor = _cellMemory.AddSensor(s);
            _sensors.Add(new Sensor(s, dbSensor, _cellMemory, _state));
            
        }

        public void AddTargetSensor(ISensor ts)
        {
            var dbSensor = _cellMemory.AddSensor(ts);
            _targetSensors.Add(new Sensor(ts, dbSensor, _cellMemory, _state));
        }

        public void AddEffector(IEffector e)
        {
            var dbEffector = _cellMemory.AddEffector(e);
            _effectors.Add(new Effector(e, dbEffector));
        }

        public void SetEffectors()
        {
            WriteValuesToDb();
            var poorPredictors = GetPoorPredictors();
            
            if (poorPredictors != null && poorPredictors.Count > 0)
                RefinePrediction(poorPredictors);
            else
            {
                var poorlyResearchedEffector = _effectors.FirstOrDefault(e => !e.IsResearchedWell());
                if (poorlyResearchedEffector != null)
                    DoResearchEffector(poorlyResearchedEffector);
                else
                    DoBestStrategy();
            }
            UpdateState();
        }

        private void UpdateState()
        {
            foreach (var eff in _effectors)
                _state._effectorsNextValues[eff.DbEffectorId] = eff.PhysicalEffector.NextValue;
        }

        private static void RefinePrediction(List<Sensor> poorPredictors)
        {
            foreach (var predictor in poorPredictors)
                predictor.RefinePrediction();
        }

        private void WriteValuesToDb()
        {
            foreach (var sensor in _sensors) _cellMemory.AddSensorEntry(sensor);
            foreach (var sensor in _targetSensors) _cellMemory.AddSensorEntry(sensor);
            foreach (var effector in _effectors) _cellMemory.AddEffectorEntry(effector);
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
            ++_timeMoment;
        }
    }
}
