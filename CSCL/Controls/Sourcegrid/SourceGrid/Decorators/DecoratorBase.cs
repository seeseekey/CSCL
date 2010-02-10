using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Controls.SourceGrid.Decorators
{
    public abstract class DecoratorBase
    {
        public abstract bool IntersectWith(Range range);

        public abstract void Draw(RangePaintEventArgs e);
    }
}
