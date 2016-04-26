namespace CnsService.EffReasearch
{
    public class EffectorTest
    {
        private int _dbId;

        public EffectorTest(int id)
        {
            _dbId = id;
        }

        public int Id { get { return _dbId; } }

        public bool ResearchedWell()
        {
            return false;
        }
    }
}