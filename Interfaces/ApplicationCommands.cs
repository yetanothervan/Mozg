using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;

namespace Interfaces
{
    public static class ApplicationCommands
    {
        public static CompositeCommand DoStepCommand = new CompositeCommand();
    }
}
