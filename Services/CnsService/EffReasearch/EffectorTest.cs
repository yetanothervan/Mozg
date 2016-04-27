using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;

namespace CnsService.EffReasearch
{
    public class EffectorTest
    {
        private readonly int _dbId;
        private readonly double _current;
        private readonly double _min;
        private readonly double _max;
        private List<ResearchInterval> _intervals;

        public EffectorTest(int id, double current, double min, double max)
        {
            _dbId = id;
            _current = current;
            _min = min;
            _max = max;
            CreateIntervals();
        }

        private void CreateIntervals()
        {
            _intervals = new List<ResearchInterval>();

            //research strategy
            var unit = (_max - _min) / Constants.UnitStep;
            int i = 0;
            while (true)
            {
                var upInter = new ResearchInterval()
                {
                    Ceiling = _current + unit * Math.Pow(2, i),
                    Floor = (i == 0) ? _current : _current + unit * Math.Pow(2, i - 1)
                };
                var downInter = new ResearchInterval()
                {
                    Ceiling = (i == 0) ? _current : _current - unit * Math.Pow(2, i - 1),
                    Floor = _current - unit * Math.Pow(2, i)
                };
                if (upInter.Ceiling > _max && downInter.Floor < _min) break;
                if (upInter.Ceiling <= _max) _intervals.Add(upInter);
                if (downInter.Floor >= _min) _intervals.Add(downInter);
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

        public int Id { get { return _dbId; } }

        public bool ResearchedWell()
        {
            return _intervals.All(i => i.Researched);
        }
    }
}