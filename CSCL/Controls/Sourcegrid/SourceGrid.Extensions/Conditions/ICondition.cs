using CSCL.Controls.SourceGrid.Cells;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Controls.SourceGrid.Conditions
{
    public interface ICondition
    {
        bool Evaluate(DataGridColumn column, int gridRow, object itemRow);

        ICellVirtual ApplyCondition(ICellVirtual cell);
    }
}
