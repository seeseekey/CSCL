using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsSample.GridSamples
{
    [Sample("SourceGrid - Advanced features", 51, "Nullable CheckBox and OnValueChanged Controller")]
    public partial class frmSample51 : Form
    {
        public frmSample51()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            grid1.Redim(30, 2);

            grid1[0, 0] = new CSCL.Controls.SourceGrid.Cells.ColumnHeader("Checked");
            grid1[0, 1] = new CSCL.Controls.SourceGrid.Cells.ColumnHeader("CheckStatus");

            CSCL.Controls.SourceGrid.Cells.Editors.ComboBox combo = new CSCL.Controls.SourceGrid.Cells.Editors.ComboBox(typeof(DevAge.Drawing.CheckBoxState));

            DependencyColumn boolToStatus = new DependencyColumn(1);
            boolToStatus.ConvertFunction = delegate(object valBool)
                                            {
                                                if (valBool == null)
                                                    return DevAge.Drawing.CheckBoxState.Undefined;
                                                else if ((bool)valBool == true)
                                                    return DevAge.Drawing.CheckBoxState.Checked;
                                                else
                                                    return DevAge.Drawing.CheckBoxState.Unchecked;
                                            };

            DependencyColumn statusToBool = new DependencyColumn(0);
            statusToBool.ConvertFunction = delegate(object valStatus)
                                            {
                                                DevAge.Drawing.CheckBoxState status = 
                                                    (DevAge.Drawing.CheckBoxState)valStatus;

                                                if (status == DevAge.Drawing.CheckBoxState.Undefined)
                                                    return null;
                                                else if (status == DevAge.Drawing.CheckBoxState.Checked)
                                                    return true;
                                                else
                                                    return false;
                                            };

            for (int r = 1; r < grid1.RowsCount; r++)
            {
                grid1[r, 0] = new CSCL.Controls.SourceGrid.Cells.CheckBox(null, null);
                grid1[r, 0].Editor.AllowNull = true;
                grid1[r, 0].AddController(boolToStatus);

                grid1[r, 1] = new CSCL.Controls.SourceGrid.Cells.Cell(DevAge.Drawing.CheckBoxState.Undefined, combo);
                grid1[r, 1].AddController(statusToBool);
            }

            grid1.AutoSizeCells();
        }

		class DependencyColumn:CSCL.Controls.SourceGrid.Cells.Controllers.ControllerBase
        {
            private int mDependencyColumn;
            public DependencyColumn(int dependencyColumn)
            {
                mDependencyColumn = dependencyColumn;
            }

            public delegate object ConvertDelegate(object source);
            public ConvertDelegate ConvertFunction;

			public override void OnValueChanged(CSCL.Controls.SourceGrid.CellContext sender, EventArgs e)
            {
                base.OnValueChanged(sender, e);

                //I use the OnValueChanged to link the value of 2 cells
                // changing the value of the other cell

                CSCL.Controls.SourceGrid.Position otherCell = new CSCL.Controls.SourceGrid.Position(sender.Position.Row, mDependencyColumn);
                CSCL.Controls.SourceGrid.CellContext otherContext = new CSCL.Controls.SourceGrid.CellContext(sender.Grid, otherCell);

                object newVal = sender.Value;
                if (ConvertFunction != null)
                    newVal = ConvertFunction(newVal);

                if (!object.Equals(otherContext.Value, newVal))
                    otherContext.Value = newVal;
            }
        }
    }
}