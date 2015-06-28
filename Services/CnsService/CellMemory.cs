using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF;
using Entities;
using Interfaces;

namespace CnsService
{
    public class CellMemory : ICellMemory
    {
        private readonly ICnsState _cnsState;
        private readonly CnsContext _context;

        public CellMemory(ICnsState cnsState)
        {
            _cnsState = cnsState;
            var connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new CnsContext(connection);
        }

        public void AddSensorEntry(Sensor s)
        {
            _context.SensorEntries.Add(new SensorEntry()
            {
                DbSensorId = s.DbSensorId,
                TimeMoment = _cnsState.TimeMoment,
                Value = s.SensorPhysical.Value
            });
            _context.SaveChanges();
        }

        public void AddEffectorEntry(Effector e)
        {
            _context.EffectorEntries.Add(new EffectorEntry()
            {
                DbEffectorId = e.DbEffectorId,
                TimeMoment = _cnsState.TimeMoment,
                Value = e.PhysicalEffector.Value
            });
            _context.SaveChanges();
        }

        public List<DbEffector> GetEffectorsWithDifferentValuesLastTwoMoment()
        {
            var entries = _context.EffectorEntries.Where(
                e => e.TimeMoment == _cnsState.TimeMoment || e.TimeMoment == _cnsState.TimeMoment - 1).ToList();

            var effs = entries.Select(e => e.DbEffector).Select(e => e).Distinct().ToList();

            var result =
                effs.Where(eff => Math.Abs(entries.First(e => e.DbEffectorId == eff.Id && e.TimeMoment == _cnsState.TimeMoment).Value 
                    - entries.First(e => e.DbEffectorId == eff.Id && e.TimeMoment == _cnsState.TimeMoment - 1).Value) > eff.Tolerance).ToList();

            return result;
        }

        public List<double> GetEffectorValues(DbEffector eff, int depth)
        {
            return
                _context.EffectorEntries.Where(
                    e => e.DbEffectorId == eff.Id && e.TimeMoment > _cnsState.TimeMoment - depth)
                    .OrderByDescending(e => e.TimeMoment)
                    .Select(e => e.Value).ToList();
        }

        public List<double> GetSensorValues(DbSensor sensor, int depth)
        {
            return
                _context.SensorEntries.Where(
                    e => e.DbSensorId == sensor.Id && e.TimeMoment > _cnsState.TimeMoment - depth)
                    .OrderByDescending(e => e.TimeMoment)
                    .Select(e => e.Value).ToList();
        }

        public double LastValue(DbSensor dbSensor)
        {
            return _context.SensorEntries.First(s => s.DbSensor.Id == dbSensor.Id && s.TimeMoment == _cnsState.TimeMoment).Value;
        }

        public DbSensor AddSensor(ISensor s)
        {
            var dbSensor =
                _context.DbSensors.Add(new DbSensor()
                {
                    Name = s.Name,
                    Tolerance = (s.MaxValue - s.MinValue)/(Constants.UnitStep * 100)
                });
            _context.SaveChanges();
            return dbSensor;
        }

        public DbEffector AddEffector(IEffector e)
        {
            var dbEffector =
                _context.DbEffectors.Add(new DbEffector()
                {
                    Name = e.Name,
                    Tolerance = (e.MaxValue - e.MinValue) / (Constants.UnitStep * 100)
                });
            _context.SaveChanges();
            return dbEffector;
        }
    }
}
