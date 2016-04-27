using System.Collections.Generic;
using CnsService.Cells;
using CnsService.EffReasearch;
using Interfaces;

namespace CnsService
{
    public class Cns : ICns, ICnsState
    {
        private int _timeMoment;
        private readonly DbCns _dbCns;
        private readonly EffectorsResearcher _researcher;

        public Cns()
        {
            _timeMoment = 0;
            _dbCns = new DbCns(this);
            _researcher = new EffectorsResearcher(_dbCns);
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

        public void Act()
        {
            //попытаться улучшить плохие предикторы
            if (!_dbCns.IsPredictedWell())
                _dbCns.RefinePrediction();

            //если иха нет - исследовать неисследованные эффекторы
            else
            {
                _researcher.PredictedWell();
                if (_researcher.AreThereIssues())
                    _researcher.DoResearch();
                else
                //если и иха нет - делать лучший ход
                    DoBestStrategy();
            }

            _dbCns.DoPrediction();
        }

        public void AdvantageMoment()
        {
            //сохранить текущие значения сенсоров и эффекторов
            _dbCns.MemorizeValues(_timeMoment);
            ++_timeMoment;
        }

        public int TimeMoment { get { return _timeMoment; } }
        
        private void DoBestStrategy()
        {
            throw new System.NotImplementedException();
        }
    }
}