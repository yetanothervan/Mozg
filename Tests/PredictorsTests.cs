using System;
using CnsService;
using CnsService.Predictors;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class PredictorsTests
    {
        private const double Tol = 0.0000001;

        [TestMethod]
        public void FirstGradePredictorTestOneEff1()
        {
            //4S + 0E + 2ES = SR
            double[,] x =
            {
                {1.0, 1.0, 6.0},
                {2.0, 1.0, 12.0},
                {1.0, 2.0, 8.0},
            };
            var m = Matrix<double>.Build.DenseOfArray(x);
            
            var coeff = FirstGradePredictor.CalculateCoeffs(m, 1);

            bool resOk =
                !Util.DoubleDiffer(coeff[0], 4.0, Tol) && !Util.DoubleDiffer(coeff[1], 0.0, Tol)
                && !Util.DoubleDiffer(coeff[2], 2.0, Tol);

            Assert.IsTrue(resOk);
        }

        [TestMethod]
        public void FirstGradePredictorTestOneEff2()
        {
            //2S + 2E + 0.1ES = SR
            double[,] x =
            {
                {1.0, 0.0, 2*1.0 + 2*0.0 + 0.1*0.0*1.0},
                {2.0, 0.5, 2*2.0 + 2*0.5 + 0.1*0.5*2.0},
                {5.0, 1.0, 2*5.0 + 2*1.0 + 0.1*1.0*5.0},
            };
            var m = Matrix<double>.Build.DenseOfArray(x);

            var coeff = FirstGradePredictor.CalculateCoeffs(m, 1);

            bool resOk =
                !Util.DoubleDiffer(coeff[0], 2.0, Tol) && !Util.DoubleDiffer(coeff[1], 2.0, Tol)
                && !Util.DoubleDiffer(coeff[2], 0.1, Tol);

            Assert.IsTrue(resOk);
        }

        [TestMethod]
        public void FirstGradePredictorTestTwoEff1()
        {
            //S + Ea + EaS + Eb + EbS + EaEb + SEaEb = SR
            double[,] x =
            {
                {1.0, 0.0, 2*1.0 + 2*0.0 + 0.1*0.0*1.0},
                {2.0, 0.5, 2*2.0 + 2*0.5 + 0.1*0.5*2.0},
                {5.0, 1.0, 2*5.0 + 2*1.0 + 0.1*1.0*5.0},
            };
            var m = Matrix<double>.Build.DenseOfArray(x);

            var coeff = FirstGradePredictor.CalculateCoeffs(m, 1);

            bool resOk =
                !Util.DoubleDiffer(coeff[0], 2.0, Tol) && !Util.DoubleDiffer(coeff[1], 2.0, Tol)
                && !Util.DoubleDiffer(coeff[2], 0.1, Tol);

            Assert.IsTrue(resOk);
        }

        [TestMethod]
        public void FirstGradePredictorTest3Eff()
        {

        }
    }
}
