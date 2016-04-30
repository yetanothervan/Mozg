using Entities;

namespace CnsService.Predictors
{
    internal interface IPredictor
    {
        double Predict(int tm);
        void AddEffectorToWatch(int eff);
        void Refine();
    }
}