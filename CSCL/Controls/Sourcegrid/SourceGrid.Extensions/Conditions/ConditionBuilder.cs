using CSCL.Controls.SourceGrid.Cells.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Controls.SourceGrid.Conditions
{
    public static class ConditionBuilder
    {
        public static ICondition AlternateView(
                                            IView view,
                                            System.Drawing.Color alternateBackcolor,
                                            System.Drawing.Color alternateForecolor)
        {
            CSCL.Controls.SourceGrid.Cells.Views.IView viewAlternate = (CSCL.Controls.SourceGrid.Cells.Views.IView)view.Clone();
            viewAlternate.BackColor = alternateBackcolor;
            viewAlternate.ForeColor = alternateForecolor;

            CSCL.Controls.SourceGrid.Conditions.ConditionView condition =
                        new CSCL.Controls.SourceGrid.Conditions.ConditionView(viewAlternate);

            condition.EvaluateFunction = delegate(CSCL.Controls.SourceGrid.DataGridColumn column, int gridRow, object itemRow)
                                    {
                                        return (gridRow & 1) == 1;
                                    };

            return condition;
        }
    }
}
