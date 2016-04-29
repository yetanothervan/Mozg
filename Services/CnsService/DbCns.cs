using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CnsService.Cells;
using EF;
using Entities;
using Interfaces;
using Prism.Regions;

namespace CnsService
{
    public class DbCns : IDbCnsOut
    {
        private readonly ICnsState _cnsState;
        private readonly List<Sensor> _sensors;
        private readonly List<Sensor> _targetSensors;
        private readonly List<Effector> _effectors;
        private readonly ICnsContext _context;
        private int _timeMomentSaved;
        
        public DbCns(ICnsState cnsState)
        {
            _cnsState = cnsState;
            _sensors = new List<Sensor>();
            _targetSensors = new List<Sensor>();
            _effectors = new List<Effector>();
            _context = new FakeCnsContext();
            _timeMomentSaved = -1;
        }

        public void MemorizeValues(int timeMoment)
        {
            if (_timeMomentSaved >= timeMoment) return;

            _context.CellEntries.AddRange(
                _sensors.Union(
                _targetSensors).Select(s => new CellEntry()
            {
                CellId = s.DbId,
                TimeMoment = timeMoment,
                Value = s.GetValue()
            }));

            _context.CellEntries.AddRange(
                _effectors.Select(e => new CellEntry()
                {
                    CellId = e.DbId,
                    TimeMoment = timeMoment,
                    Value = e.GetValue()
                }));

            _timeMomentSaved = timeMoment;

            _context.SaveChanges();
        }

        public void AddSensor(ISensor s)
        {
            var dbs = _context.DbSensors.Add(new DbSensor()
            {
                Id = GetNewId(),
                Name = s.Name,
                Tolerance = (s.MaxValue - s.MinValue)/(Constants.UnitStep*100)
            });
            _sensors.Add(new Sensor(s, dbs.Id, this, dbs.Tolerance));
            _context.SaveChanges();
        }

        public void AddTargetSensor(ISensor ts)
        {
            var dbs = _context.DbSensors.Add(new DbSensor()
            {
                Id = GetNewId(),
                Name = ts.Name,
                Tolerance = (ts.MaxValue - ts.MinValue)/(Constants.UnitStep*100)
            });
            _targetSensors.Add(new Sensor(ts, dbs.Id, this, dbs.Tolerance));
            _context.SaveChanges();
        }

        public void AddEffector(IEffector e)
        {
            var dbs = _context.DbEffectors.Add(new DbEffector()
            {
                Id = GetNewId(),
                Name = e.Name,
                Tolerance = (e.MaxValue - e.MinValue) / (Constants.UnitStep * 100)
            });
            _effectors.Add(new Effector(e, dbs.Id));
            _context.SaveChanges();
        }

        private int GetNewId()
        {
            if (!_context.DbEffectors.Any() && !_context.DbSensors.Any())
                return 1;

            if (_context.DbEffectors.Any() && _context.DbSensors.Any())
                return 1 + Math.Max(_context.DbEffectors.Max(i => i.Id), _context.DbSensors.Max(i => i.Id));

            if (!_context.DbSensors.Any())
                return 1 + _context.DbEffectors.Max(i => i.Id);

            return 1 + _context.DbSensors.Max(i => i.Id);
        }
        
        public void RefinePrediction()
        {
            if (_timeMomentSaved == -1) return;

            var badPredictors = GetPoorPredictors();
            foreach (var bp in badPredictors)
                bp.RefinePrediction();
        }
        
        public List<Effector> GetEffectors()
        {
            return _effectors;
        }

        public void SetEffector(int id, double value)
        {
            _effectors.First(e => e.DbId == id).SetNextValue(value);
        }

        public bool IsPredictedWell()
        {
            var poorPredictors = GetPoorPredictors();
            return poorPredictors == null || poorPredictors.Count == 0;
        }

        public void DoPrediction()
        {
            foreach (var s in _sensors) 
                s.DoPrediction();
            foreach (var ts in _targetSensors)
                ts.DoPrediction();
        }

        public int CurrentTimeMoment
        {
            get { return _cnsState.TimeMoment; }
        }

        public List<Cell> GetEffectorsThatChangedLastMoment()
        {
            var effIds = _context.DbEffectors.Select(c => new Cell(){Id = c.Id, Tolerance = c.Tolerance}).ToList();
            return CellsThatChangedLastMoment(effIds);
        }

        public List<Cell> GetSensorThatChangedLastMoment()
        {
            var sensIds = _context.DbSensors.Select(c => new Cell() { Id = c.Id, Tolerance = c.Tolerance }).ToList();
            return CellsThatChangedLastMoment(sensIds);
        }
        

        public List<double> GetValuesForCellLast(int id, int depth)
        {
            return
                _context.CellEntries.Where(c => c.CellId == id && c.TimeMoment > _timeMomentSaved - depth)
                    .OrderByDescending(c => c.TimeMoment)
                    .Select(c => c.Value)
                    .ToList();
        }

        public double GetEffectorNextValue(int id)
        {
            return _effectors.First(e => e.DbId == id).GetNextValue();
        }
        
        private List<Sensor> GetPoorPredictors()
        {
            var result = new List<Sensor>();
            if (_timeMomentSaved == -1)
            {
                result.AddRange(_sensors);
                result.AddRange(_targetSensors);
                return result;
            }

            result.AddRange(_sensors.Where(s => !s.PredictedWell()));
            result.AddRange(_targetSensors.Where(s=>!s.PredictedWell()));
            return result;
        }

        private List<Cell> CellsThatChangedLastMoment(IEnumerable<Cell> cells)
        {
            var last =
                _context.CellEntries.Where(
                    e => cells.Select(id => id.Id).Contains(e.CellId) && e.TimeMoment == _timeMomentSaved);
            var prelast =
                _context.CellEntries.Where(
                    e => cells.Select(id => id.Id).Contains(e.CellId) && e.TimeMoment == _timeMomentSaved - 1);

            if (!last.Any() || !prelast.Any())
                return null;

            var res = last.Join(prelast,
                l => l.CellId, pl => pl.CellId,
                (l, pl) => new { l.CellId, LastValue = l.Value, PrelastValue = pl.Value })
                .Where(e =>
                    Util.DoubleDiffer(e.LastValue, e.PrelastValue, cells.First(eff => eff.Id == e.CellId).Tolerance))
                .ToList();

            return cells.Where(e => res.Select(r => r.CellId).Contains(e.Id)).ToList();
        }
    }
}
