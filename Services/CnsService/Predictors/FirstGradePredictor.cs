using System;
using System.Collections.Generic;
using CnsService.Cells;
using Entities;
using MathNet.Numerics.LinearAlgebra;

namespace CnsService.Predictors
{
    public class FirstGradePredictor : IPredictor
    {
        private readonly Sensor _sensor;
        private readonly IDbCnsOut _db;
        private readonly List<int> _effectors;
        private List<double> _coeffs;

        public FirstGradePredictor(Sensor sensor, IDbCnsOut db)
        {
            _sensor = sensor;
            _db = db;
            _effectors = new List<int>();
            _coeffs = new List<double>();
        }

        public double Predict(int tm)
        {
            if (_coeffs == null || _coeffs.Count == 0)
                return -1;

            double res = 0;

            for (int i = 0; i < _coeffs.Count; ++i)
            {
                double comp = 1;
                var vars = GetVariant(i);
                foreach (var var in vars)
                    comp *= 
                        var == 0 
                        ? _sensor.GetValue() 
                        : _db.GetEffectorNextValue(_effectors[var - 1]);
                
                res += _coeffs[i]*comp;
            }
            return res;
        }

        public void AddEffectorToWatch(int id)
        {
            if (!_effectors.Contains(id))
                _effectors.Add(id);
        }

        public void Refine()
        {
            RegainCoeffs();
        }

        private void RegainCoeffs() //regain coeffs by cramer 
        {
            var ec = _effectors.Count; //effectors count
            var variants = (int) Math.Pow(2, _effectors.Count + 1) - 1; //variants count
            
            var mvs = Matrix<double>.Build.Dense(variants, ec + 2);

            //fill main values
            
            //sens component
            var svals = _db.GetValuesForCellLast(_sensor.DbId, variants + 1);
            if (svals == null || svals.Count < variants + 1) return; //TODO just wait?
            
            for (var i = 0; i < variants; ++i)
                mvs[i, 0] = svals[i + 1];

            //result sens component
            for (var i = 0; i < variants; ++i)
                mvs[i, ec + 1] = svals[i];

            //effs component
            for (var i = 0; i < ec; ++i)
            {
                var evals = _db.GetValuesForCellLast(_effectors[i], variants);
                for (var j = 0; j < variants; ++j)
                    mvs[j, 1 + i] = evals[j];
            }

            _coeffs = CalculateCoeffs(mvs, ec);
        }

        public static List<double> CalculateCoeffs(Matrix<double> mvs, int ec)
        {
            var variants = mvs.RowCount;
            //fill extended values
            var evs = Matrix<double>.Build.Dense(variants, variants + 1);
            //fill res sens
            for (int i = 0; i < variants; ++i)
                evs[i, variants] = mvs[i, ec + 1];
            //others
            for (int i = 0; i < variants; ++i) //evs rows
            {
                for (int j = 0; j < variants; ++j) //evs cols
                {
                    double res = 1.0;
                    var vars = GetVariant(j);
                    foreach (var @var in vars)
                        res *= mvs[i, @var];
                    evs[i, j] = res;
                }
            }

            MathNet.Numerics.Control.UseManaged();

            //get coeffs
            var md = evs.RemoveColumn(variants);
            var dmd = md.Determinant();
            var srcol = evs.Column(variants);
            var coeffs = new List<double>();
            for (int i = 0; i < variants; ++i)
            {
                var m = Matrix<double>.Build.Dense(variants, variants);
                md.CopyTo(m);
                m.SetColumn(i, srcol);
                var dm = m.Determinant();
                coeffs.Add(dm / dmd);
            }
            return coeffs;
        }

        public static List<int> GetVariant(int var)
        {
            var res = new List<int>();
            for(int pos = 0; pos < 32; ++pos)
                if (((var + 1) & (1 << pos)) != 0)
                    res.Add(pos);
            return res;
        }
    }
}