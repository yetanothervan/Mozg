using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF;
using Entities;
using EntityFramework.BulkInsert.Extensions;
using Interfaces;

namespace CnsService
{
    public class CellMemory : ICellMemory
    {
        private readonly ICnsState _cnsState;
        private readonly ICnsContext _context;

        public CellMemory(ICnsState cnsState)
        {
            _cnsState = cnsState;
            //var connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new FakeCnsContext();
        }

        public void AddSensorEntry(IEnumerable<Sensor> sensors)
        {
            _context.SensorEntries.AddRange(sensors.Select(s => new SensorEntry()
            {
                DbSensorId = s.DbSensorId,
                TimeMoment = _cnsState.TimeMoment,
                Value = s.SensorPhysical.Value
            }));
            _context.SaveChanges();
        }

        public void AddEffectorEntry(IEnumerable<Effector> effs)
        {
            _context.EffectorEntries.AddRange(effs.Select(eff => new EffectorEntry()
            {
                DbEffectorId = eff.DbEffectorId,
                TimeMoment = _cnsState.TimeMoment,
                Value = eff.PhysicalEffector.Value
            }));
            _context.SaveChanges();
        }

        public List<DbEffector> GetEffectorsWithDifferentValuesLastTwoMoment()
        {
            var entries = _context.EffectorEntries.Where(
                e => e.TimeMoment == _cnsState.TimeMoment || e.TimeMoment == _cnsState.TimeMoment - 1).ToList();

            var effs = entries.Select(e => e.DbEffectorId).Select(e => e).Distinct().ToList();

            var result =
                effs.Where(eff => Math.Abs(entries.First(e => e.DbEffectorId == eff && e.TimeMoment == _cnsState.TimeMoment).Value 
                    - entries.First(e => e.DbEffectorId == eff && e.TimeMoment == _cnsState.TimeMoment - 1).Value) 
                    > _context.DbEffectors.First(ef => ef.Id == eff).Tolerance).ToList();

            return _context.DbEffectors.Where(e => result.Contains(e.Id)).ToList();
        }

        public List<double> GetEffectorValues(DbEffector eff, int depth)
        {
            return
                _context.EffectorEntries.Where(
                    e => e.DbEffectorId == eff.Id && e.TimeMoment >= _cnsState.TimeMoment - depth)
                    .OrderByDescending(e => e.TimeMoment)
                    .Select(e => e.Value).ToList();
        }

        public List<double> GetSensorValues(DbSensor sensor, int depth)
        {
            return
                _context.SensorEntries.Where(
                    e => e.DbSensorId == sensor.Id && e.TimeMoment >= _cnsState.TimeMoment - depth)
                    .OrderByDescending(e => e.TimeMoment)
                    .Select(e => e.Value).ToList();
        }

        public double LastValue(DbSensor dbSensor)
        {
            return _context.SensorEntries.First(s => s.DbSensorId == dbSensor.Id && s.TimeMoment == _cnsState.TimeMoment).Value;
        }

        public List<DbEffector> GetEffectors()
        {
            return _context.DbEffectors.ToList();
        }

        public IList<DbSensor> GetSensors()
        {
            return _context.DbSensors.ToList();
        }

        private int _sensorId;
        public DbSensor AddSensor(ISensor s)
        {
            var dbSensor =
                _context.DbSensors.Add(new DbSensor()
                {
                    Id = ++_sensorId,
                    Name = s.Name,
                    Tolerance = (s.MaxValue - s.MinValue)/(Constants.UnitStep * 100)
                });
            _context.SaveChanges();
            return dbSensor;
        }

        private int _effectorId;
        public DbEffector AddEffector(IEffector e)
        {
            var dbEffector =
                _context.DbEffectors.Add(new DbEffector()
                {
                    Id = ++_effectorId,
                    Name = e.Name,
                    Tolerance = (e.MaxValue - e.MinValue) / (Constants.UnitStep * 100)
                });
            _context.SaveChanges();
            return dbEffector;
        }
    }
}
