using System;
using System.Collections.Generic;
using System.Linq;
using EF;
using Entities;
using Interfaces;

namespace CnsService
{
    public class CellMemory 
    {
        private readonly ICnsState _cnsState;
        private readonly ICnsContext _context;

        public CellMemory(ICnsState cnsState)
        {
            _cnsState = cnsState;
            //var connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new FakeCnsContext();
        }
        

        public List<DbEffector> GetEffectorsWithDifferentValuesLastTwoMoment()
        {
            //var entries = _context.EffectorEntries.Where(
            //    e => e.TimeMoment == _cnsState.TimeMoment || e.TimeMoment == _cnsState.TimeMoment - 1).ToList();

            //var effs = entries.Select(e => e.DbEffectorId).Select(e => e).Distinct().ToList();

            //var result =
            //    effs.Where(eff => Math.Abs(entries.First(e => e.DbEffectorId == eff && e.TimeMoment == _cnsState.TimeMoment).Value 
            //        - entries.First(e => e.DbEffectorId == eff && e.TimeMoment == _cnsState.TimeMoment - 1).Value) 
            //        > _context.DbEffectors.First(ef => ef.Id == eff).Tolerance).ToList();

            //return _context.DbEffectors.Where(e => result.Contains(e.Id)).ToList();
            return null;
        }
    }
}
