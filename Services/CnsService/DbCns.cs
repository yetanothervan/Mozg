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

namespace CnsService
{
    public class DbCns
    {
        private readonly List<Sensor> _sensors;
        private readonly List<Sensor> _targetSensors;
        private readonly List<Effector> _effectors;
        private readonly ICnsContext _context;
        private int _timeMomentSaved;
        
        public DbCns()
        {
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
            _sensors.Add(new Sensor(s, dbs.Id));
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
            _targetSensors.Add(new Sensor(ts, dbs.Id));
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
            return 1 + Math.Max(_context.DbEffectors.Max(i => i.Id), _context.DbSensors.Max(i => i.Id));
        }
    }
}
