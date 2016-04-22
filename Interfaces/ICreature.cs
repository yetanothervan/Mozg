﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface ICreature
    {
        List<Action> GetActions();
        void AdvantageMoment();
        ICnsDiagnostics CnsDiagnostics { get; }
    }
}
