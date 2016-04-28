using System.Collections.Generic;
using CnsService.Cells;
using Entities;

namespace CnsService
{
    public interface IDbCnsOut
    {
        int CurrentTimeMoment { get; }
        List<DbEffector> GetEffectorsThatChangedLastMoment();
        List<double> GetValuesForCellLast(int id, int depth);
        double GetEffectorNextValue(int id);
    }
}