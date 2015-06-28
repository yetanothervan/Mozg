using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Entities;
using Interfaces;

namespace CnsService
{
    public class Effector
    {
        private readonly IEffector _effectorPhysical;
        private readonly DbEffector _dbEffector;
        public IEffector PhysicalEffector { get { return _effectorPhysical; } }
        public int DbEffectorId { get { return _dbEffector.Id; } }

        public Effector(IEffector effector, DbEffector dbEffector)
        {
            _effectorPhysical = effector;
            _dbEffector = dbEffector;
        }

        private EffectorReseacher _effectorResacher;

        public bool IsResearchedWell()
        {
            if (_effectorResacher == null) return false;
            return _effectorResacher.IsResearchWell();
        }

        public void DoResearch()
        {
            if (_effectorResacher == null) _effectorResacher = new EffectorReseacher(_effectorPhysical);
            if (!_effectorResacher.IsResearchWell())
                _effectorPhysical.NextValue = _effectorResacher.PickForResearch();
        }

        public void SetReseached(double value)
        {
            if (_effectorResacher != null) _effectorResacher.SetResearched(value);
        }
    }

    public class EffectorResearchInterval : ValueInterval
    {
        public EffectorResearchInterval()
        {
            Researched = false;
        }
        public bool Researched { get; set; }
    }

    public class EffectorReseacher
    {
        private readonly IEffector _effectorPhysical;
        private readonly List<EffectorResearchInterval> _intervals;

        public EffectorReseacher(IEffector effectorPhysical)
        {
            _effectorPhysical = effectorPhysical;
            _intervals = new List<EffectorResearchInterval>();

            //research strategy
            var unit = (_effectorPhysical.MaxValue - _effectorPhysical.MinValue)/Constants.UnitStep;
            int i = 0;
            while (true)
            {
                var upInter = new EffectorResearchInterval()
                {
                    Ceiling = _effectorPhysical.Value + unit*Math.Pow(2, i),
                    Floor = (i == 0) ? _effectorPhysical.Value : _effectorPhysical.Value + unit*Math.Pow(2, i - 1)
                };
                var downInter = new EffectorResearchInterval()
                {
                    Ceiling = (i == 0) ? _effectorPhysical.Value : _effectorPhysical.Value - unit*Math.Pow(2, i - 1),
                    Floor = _effectorPhysical.Value - unit*Math.Pow(2, i)
                };
                if (upInter.Ceiling > _effectorPhysical.MaxValue && downInter.Floor < _effectorPhysical.MinValue) break;
                if (upInter.Ceiling <= _effectorPhysical.MaxValue) _intervals.Add(upInter);
                if (downInter.Floor >= _effectorPhysical.MinValue) _intervals.Add(downInter);
                ++i;
            }
        }

        public double PickForResearch()
        {
            var interval = _intervals.First(i => !i.Researched);
            return (interval.Floor + interval.Ceiling) / 2;
        }

        public void SetResearched(double value)
        {
            var interval = _intervals.FirstOrDefault(i => i.Ceiling >= value && i.Floor < value);
            if (interval != null)
                interval.Researched = true;
        }

        public bool IsResearchWell()
        {
            return _intervals.All(i => i.Researched);
        }
    }
}