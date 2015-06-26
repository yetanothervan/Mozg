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
            
        }

        public void AdvantageMoment()
        {
           //throw new NotImplementedException();
        }
    }
}
