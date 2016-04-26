using System.Collections.Generic;
using System.Linq;

namespace CnsService.EffReasearch
{
    public class EffectorsResearcher
    {
        private readonly DbCns _dbCns;
        private readonly List<EffectorTest> _effTests;

        public EffectorsResearcher(DbCns dbCns)
        {
            _dbCns = dbCns;
            _effTests = new List<EffectorTest>();
        }

        public bool AreThereIssues()
        {
            if (_dbCns.GetEffectors().Count > _effTests.Count)
                ArrangeResearches();

            return _effTests.Any(e => !e.ResearchedWell());
        }

        public void DoResearch()
        {
            if (_effTests == null || _effTests.Count == 0) return;
            
            var toResearch = _effTests.FirstOrDefault(e => !e.ResearchedWell());
            if (toResearch == null) return;
            
            //toResearch.
        }

        private void ArrangeResearches()
        {
            var exist = _effTests.Select(s => s.Id).ToList();
            var toAddEffs = _dbCns.GetEffectors().Where(e => !exist.Contains(e.DbId));
            foreach (var eff in toAddEffs)
                _effTests.Add(new EffectorTest(eff.DbId));
        }
    }
}