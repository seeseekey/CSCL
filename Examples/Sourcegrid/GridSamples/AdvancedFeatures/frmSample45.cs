using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsSample.GridSamples
{
    [Sample("SourceGrid - Advanced features", 45, "Grid navigation, tab, arrows and other controls")]
    public partial class frmSample45 : Form
    {
        public frmSample45()
        {
            InitializeComponent();
        }

        private void frmSample45_Load(object sender, EventArgs e)
        {
            grid1.BorderStyle = BorderStyle.FixedSingle;

            grid1.ColumnsCount = 3;
            grid1.FixedRows = 1;
            grid1.Rows.Insert(0);
            grid1[0, 0] = new CSCL.Controls.SourceGrid.Cells.ColumnHeader("String");
            grid1[0, 1] = new CSCL.Controls.SourceGrid.Cells.ColumnHeader("DateTime");
            grid1[0, 2] = new CSCL.Controls.SourceGrid.Cells.ColumnHeader("CheckBox");
            for (int r = 1; r < 10; r++)
            {
                grid1.Rows.Insert(r);
                grid1[r, 0] = new CSCL.Controls.SourceGrid.Cells.Cell("Hello " + r.ToString(), typeof(string));
                grid1[r, 1] = new CSCL.Controls.SourceGrid.Cells.Cell(DateTime.Today, typeof(DateTime));
                grid1[r, 2] = new CSCL.Controls.SourceGrid.Cells.CheckBox(null, true);
            }

            grid1.AutoSizeCells();

            grid1.Selection.FocusStyle = grid1.Selection.FocusStyle | CSCL.Controls.SourceGrid.FocusStyle.FocusFirstCellOnEnter;
            grid1.Selection.FocusStyle = grid1.Selection.FocusStyle | CSCL.Controls.SourceGrid.FocusStyle.RemoveFocusCellOnLeave;

            chkTabStop.Checked = grid1.TabStop;
        }

        private void Checkbox_CheckedChanged(object sender, System.EventArgs e)
        {
            CSCL.Controls.SourceGrid.GridSpecialKeys specialKeys = CSCL.Controls.SourceGrid.GridSpecialKeys.None;

            if (chkArrows.Checked)
                specialKeys = specialKeys | CSCL.Controls.SourceGrid.GridSpecialKeys.Arrows;
            if (chkEnter.Checked)
                specialKeys = specialKeys | CSCL.Controls.SourceGrid.GridSpecialKeys.Enter;
            if (chkEscape.Checked)
                specialKeys = specialKeys | CSCL.Controls.SourceGrid.GridSpecialKeys.Escape;
            if (chkTab.Checked)
                specialKeys = specialKeys | CSCL.Controls.SourceGrid.GridSpecialKeys.Tab;

            grid1.SpecialKeys = specialKeys;

            CSCL.Controls.SourceGrid.FocusStyle focusStyle = CSCL.Controls.SourceGrid.FocusStyle.None;

            if (chkAutomaticFocus.Checked)
            {
                focusStyle = focusStyle | CSCL.Controls.SourceGrid.FocusStyle.FocusFirstCellOnEnter;
                focusStyle = focusStyle | CSCL.Controls.SourceGrid.FocusStyle.RemoveFocusCellOnLeave;
            }

            grid1.Selection.FocusStyle = focusStyle;

            grid1.TabStop = chkTabStop.Checked;
        }

    }
}