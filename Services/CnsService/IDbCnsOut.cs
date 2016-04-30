using System.Collections.Generic;
using CnsService.Cells;
using Entities;

namespace CnsService
{
    public interface IDbCnsOut
    {
        int CurrentTimeMoment { get; }
        List<Cell> GetEffectorsThatChangedLastMoment();
        List<double> GetValuesForCellLast(int id, int depth);
        double GetEffectorNextValue(int id);
        List<Cell> GetSensorThatChangedLastMoment();
    }
}