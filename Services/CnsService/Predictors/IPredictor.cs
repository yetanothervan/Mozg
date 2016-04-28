using Entities;

namespace CnsService.Predictors
{
    internal interface IPredictor
    {
        double Predict(int tm);
        void AddEffectorToWatch(DbEffector eff);
        void Refine();
    }
}