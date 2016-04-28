using System.Collections.Generic;
using System.Linq;

namespace CnsService.EffReasearch
{
    public class EffectorsResearcher
    {
        private readonly DbCns _dbCns;
        private readonly List<EffectorTest> _effTests;
        private EffectorTest _currentTest;
        private Researching _currentResearchValue;

        public EffectorsResearcher(DbCns dbCns)
        {
            _dbCns = dbCns;
            _currentTest = null;
            _effTests = new List<EffectorTest>();
            _currentResearchValue = null;
        }

        public bool AreThereIssues()
        {
            if (_dbCns.GetEffectors().Count > _effTests.Count)
                ArrangeResearches();

            if (_currentTest != null && !_currentTest.ResearchedWell()) return true;

            return _effTests.Any(e => !e.ResearchedWell());
        }

        public void DoResearch()
        {
            if (_effTests == null || _effTests.Count == 0) return;

            if (_currentTest == null || _currentTest.ResearchedWell())
                _currentTest = _effTests.FirstOrDefault(e => !e.ResearchedWell());

            if (_currentTest == null) return;
            
            _currentResearchValue = new Researching()
            {
                Id = _currentTest.Id,
                Value = _currentTest.PickForResearch()
            };
            _dbCns.SetEffector(_currentResearchValue.Id, _currentResearchValue.Value);
        }
        
        private void ArrangeResearches()
        {
            var exist = _effTests.Select(s => s.Id).ToList();
            var toAddEffs = _dbCns.GetEffectors().Where(e => !exist.Contains(e.DbId));
            foreach (var eff in toAddEffs)
                _effTests.Add(new EffectorTest(eff.DbId, eff.GetValue(), eff.MinValue, eff.MaxValue));
        }

        public void PredictedWell()
        {
            if (_currentResearchValue != null)
                _effTests.First(t => t.Id == _currentResearchValue.Id).SetResearched(_currentResearchValue.Value);
        }
    }

    class Researching
    {
        public int Id { get; set; }
        public double Value { get; set; }
    }
}