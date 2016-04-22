namespace Interfaces
{
    public interface ICns
    {
        void AddSensor(ISensor s);
        void AddTargetSensor(ISensor ts);
        void AddEffector(IEffector e);
        void SetEffectors();
        void DoPrediction();
        void AdvantageMoment();
        ICnsDiagnostics CnsDiagnostics { get; }
    }
}
