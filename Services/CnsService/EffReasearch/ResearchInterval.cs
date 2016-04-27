namespace CnsService.EffReasearch
{
    public class ResearchInterval
    {
        public ResearchInterval()
        {
            Researched = false;
        }
        public double Ceiling { get; set; }
        public double Floor { get; set; }
        public bool Researched { get; set; }
    }
}