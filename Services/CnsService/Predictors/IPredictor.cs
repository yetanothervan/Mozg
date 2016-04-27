namespace CnsService.Predictors
{
    internal interface IPredictor
    {
        double Predict(int tm);
    }
}