namespace WindowsFormsSample.GridSamples
{
    partial class frmSample48
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grid1 = new CSCL.Controls.SourceGrid.Grid();
            this.SuspendLayout();
            // 
            // grid1
            // 
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.Location = new System.Drawing.Point(0, 0);
            this.grid1.Name = "grid1";
            this.grid1.Size = new System.Drawing.Size(250, 245);
            this.grid1.TabIndex = 0;
            // 
            // frmSample48
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 245);
            this.Controls.Add(this.grid1);
            this.Name = "frmSample48";
            this.Text = "Basic grid 2";
            this.ResumeLayout(false);

        }

        #endregion

        private CSCL.Controls.SourceGrid.Grid grid1;
    }
}