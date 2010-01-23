using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Scripting
{
    public interface IScript
    {
        void Run(params object[] ParameterList);
    }
}
