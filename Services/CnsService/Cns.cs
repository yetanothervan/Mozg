using System.Collections.Generic;
using CnsService.Cells;
using Interfaces;

namespace CnsService
{
    public class Cns : ICns
    {
        private readonly int _timeMoment;
        private readonly DbCns _dbCns;

        public Cns()
        {
            _timeMoment = 0;
            _dbCns = new DbCns();
        }

        public void AddSensor(ISensor s)
        {
            _dbCns.AddSensor(s);
        }

        public void AddTargetSensor(ISensor ts)
        {
            _dbCns.AddTargetSensor(ts);
        }

        public void AddEffector(IEffector e)
        {
            _dbCns.AddEffector(e);
        }

        public void SetEffectors()
        {
            _dbCns.MemorizeValues(_timeMoment);
        }

        public void AdvantageMoment()
        {
        }
    }
}