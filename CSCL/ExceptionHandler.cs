//
//  ExceptionHandler.cs
//
//  Copyright (c) 2011, 2012 by seeseekey <seeseekey@googlemail.com>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using CSCL.Network;
using CSCL.Helpers;

namespace CSCL
{
		//Use Example
		//[STAThread]
		//static void Main()
		//{
		//    Application.EnableVisualStyles();
		//    Application.SetCompatibleTextRenderingDefault(false);
		//    InstFormMain = new FormMain();
		//    Application.ThreadException+=new ThreadExceptionEventHandler(Globals.GlobalExceptionHandler);
		//    Application.Run(InstFormMain);
		//}

		//[Conditional("DEBUG")]
		//private static void ShowDebugButton(ExceptionHandler exceptionDialog)
		//{
		//    exceptionDialog.ShowDebugButton=true;
		//}

		///// <summary>
		///// ThreadExceptionEventHandler
		///// </summary>
		///// <param name="sender"></param>
		///// <param name="e"></param>
		//public static void GlobalExceptionHandler(object sender, ThreadExceptionEventArgs e)
		//{
		//    ExceptionInfo exceptionInfo=new ExceptionInfo(e.Exception);

		//    ExceptionHandler exceptionDialog=new ExceptionHandler(exceptionInfo);
		//    exceptionDialog.TitleText=string.Format("{0} hat ein Problem festgestellt und muss beendet werden. Klicken Sie auf \"Erweitert\", um sich detaillierte Informationen anzeigen zu lassen!", Application.ProductName);
		//    exceptionDialog.UnhandledException=true;
		//    exceptionDialog.ShowAppRestartCheckBox=true;
		//    exceptionDialog.ShowReportErrorLabel=true;
		//    exceptionDialog.AutoReport=false;

		//    exceptionDialog.SMTPServer="smtp.seeseekey.net";
        //    exceptionDialog.ToAdress="kontakt@seeseekey.net";
		//    exceptionDialog.ToName="seeseekey";

		//    ShowDebugButton(exceptionDialog);
		//    exceptionDialog.ShowDialog();
		//}

	/// <summary>
	/// Stellt Informationen des angegebenen ExceptionInfo-Objekts dar.
	/// </summary>
	public class ExceptionHandler : Form
	{
		#region Private Variables
		// Konstanten
		private const int DIALOG_HEIGHT_MAX=528;
		private const int DIALOG_HEIGHT_MIN=240;
		private const int LINE_HEIGHT=13;
		private const int LINE_COUNT=5;
		private const string STANDARD_TITLE_TEXT="In der Anwendung ist ein Fehler aufgetreten! Klicken Sie auf \"Erweitert\", um sich detailierte Informationen anzeigen zu lassen.";
		private const string STANDARD_MESSAGE="Es ist ein Fehler aufgetreten, der keinen Fehlertext besitzt.";
		private const string STANDARD_REPORT_TEXT="Diesen Fehler an {0} melden";
		private const string STANDARD_NO_HELP_TEXT="F�r die gew�hlte Ausnahme exisitiert keine Hilfe!";

		// Allgemeine Variablen
		private Exception m_exception;
		private bool m_extended=true;
		private bool m_initDialog=false;

		// Property-Variablen
		private ExceptionInfo m_exceptionInfo;
		private bool m_unhandledException=false;
		private bool m_showReportErrorLabel=false;
		private bool m_showAppRestartCheckBox=true;
		private bool m_showExtendedButton=true;
		private bool m_showDebugButton=false;
		private bool m_showHelpButton=true;
		private bool m_showExtended=false;
		private string m_titleText=STANDARD_TITLE_TEXT;
		#endregion

		#region Controls
		private System.Windows.Forms.TextBox txtErrorText;
		private System.Windows.Forms.PictureBox picIcon;
		private System.Windows.Forms.GroupBox groupBox1;
		public System.Windows.Forms.Button cmdOK;
		public System.Windows.Forms.Button cmdExtended;
		private System.Windows.Forms.TabPage tbpGeneral;
		private System.Windows.Forms.Label lblThreadUserText;
		private System.Windows.Forms.Label lblThreadIDText;
		private System.Windows.Forms.Label lblAppDomainText;
		private System.Windows.Forms.Label lblAssemblyText;
		private System.Windows.Forms.Label lblClassText;
		private System.Windows.Forms.Label lblFunctionText;
		private System.Windows.Forms.Label lblInnerException;
		private System.Windows.Forms.Label lblThreadUser;
		private System.Windows.Forms.Label lblThreadID;
		private System.Windows.Forms.Label lblAppDomain;
		private System.Windows.Forms.Label lblFunction;
		private System.Windows.Forms.Label lblClass;
		private System.Windows.Forms.Label lblAssembly;
		private System.Windows.Forms.TreeView tvwInnerExceptions;
		private System.Windows.Forms.TabPage tbpStackTrace;
		private System.Windows.Forms.TextBox txtStackTrace;
		private System.Windows.Forms.Button cmdDebug;
		private System.Windows.Forms.CheckBox chkAppRestart;
		private System.Windows.Forms.LinkLabel llblReportError;
		private System.Windows.Forms.Panel panTitleFrame;
		private System.Windows.Forms.Label lblTitleText;
		private System.Windows.Forms.Button cmdHelp;
		private System.Windows.Forms.TabControl tabExtended;
		private System.Windows.Forms.TabPage tbpApplication;
		private System.Windows.Forms.Label lblExecutablePathText;
		private System.Windows.Forms.Label lblExecutablePath;
		private System.Windows.Forms.Label lblProductVersionText;
		private System.Windows.Forms.Label lblProductNameText;
		private System.Windows.Forms.Label lblProductName;
		private System.Windows.Forms.Label lblProductVersion;
		private System.Windows.Forms.Label lblOperatingSystemText;
		private System.Windows.Forms.Label lblOperatingSystem;
		private System.Windows.Forms.Label lblFrameworkVersionText;
		private System.Windows.Forms.Label lblFrameworkVersion;
		private System.Windows.Forms.TabPage tbpExtended;
		private System.Windows.Forms.PropertyGrid propgrdExtended;
		private System.Windows.Forms.Label lblLineColumnText;
		private System.Windows.Forms.Label lblLineColumn;
		private System.Windows.Forms.Label lblOffsetText;
		private System.Windows.Forms.Label lblOffset;
		private System.Windows.Forms.Label lblWorkingSetText;
		private System.Windows.Forms.Label lblWorkingSet;
		private System.Windows.Forms.Label lblSourceFileText;
		private System.Windows.Forms.Label lblSourceFile;
		private System.ComponentModel.Container components=null;
		#endregion

		#region Constructors
		public ExceptionHandler()
		{
			InitializeComponent();
		}

		public ExceptionHandler(ExceptionInfo exInfo)
		{
			m_exceptionInfo=exInfo;
			InitializeComponent();
		}
		#endregion

		#region Overrides
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components!=null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager(typeof(ExceptionHandler));
			this.txtErrorText=new System.Windows.Forms.TextBox();
			this.panTitleFrame=new System.Windows.Forms.Panel();
			this.lblTitleText=new System.Windows.Forms.Label();
			this.picIcon=new System.Windows.Forms.PictureBox();
			this.groupBox1=new System.Windows.Forms.GroupBox();
			this.cmdOK=new System.Windows.Forms.Button();
			this.cmdExtended=new System.Windows.Forms.Button();
			this.tabExtended=new System.Windows.Forms.TabControl();
			this.tbpGeneral=new System.Windows.Forms.TabPage();
			this.lblSourceFileText=new System.Windows.Forms.Label();
			this.lblSourceFile=new System.Windows.Forms.Label();
			this.lblWorkingSetText=new System.Windows.Forms.Label();
			this.lblWorkingSet=new System.Windows.Forms.Label();
			this.lblOffsetText=new System.Windows.Forms.Label();
			this.lblOffset=new System.Windows.Forms.Label();
			this.lblLineColumnText=new System.Windows.Forms.Label();
			this.lblLineColumn=new System.Windows.Forms.Label();
			this.lblThreadUserText=new System.Windows.Forms.Label();
			this.lblThreadIDText=new System.Windows.Forms.Label();
			this.lblAppDomainText=new System.Windows.Forms.Label();
			this.lblAssemblyText=new System.Windows.Forms.Label();
			this.lblClassText=new System.Windows.Forms.Label();
			this.lblFunctionText=new System.Windows.Forms.Label();
			this.lblInnerException=new System.Windows.Forms.Label();
			this.lblThreadUser=new System.Windows.Forms.Label();
			this.lblThreadID=new System.Windows.Forms.Label();
			this.lblAppDomain=new System.Windows.Forms.Label();
			this.lblFunction=new System.Windows.Forms.Label();
			this.lblClass=new System.Windows.Forms.Label();
			this.lblAssembly=new System.Windows.Forms.Label();
			this.tvwInnerExceptions=new System.Windows.Forms.TreeView();
			this.tbpStackTrace=new System.Windows.Forms.TabPage();
			this.txtStackTrace=new System.Windows.Forms.TextBox();
			this.tbpApplication=new System.Windows.Forms.TabPage();
			this.lblFrameworkVersionText=new System.Windows.Forms.Label();
			this.lblFrameworkVersion=new System.Windows.Forms.Label();
			this.lblOperatingSystemText=new System.Windows.Forms.Label();
			this.lblOperatingSystem=new System.Windows.Forms.Label();
			this.lblExecutablePathText=new System.Windows.Forms.Label();
			this.lblExecutablePath=new System.Windows.Forms.Label();
			this.lblProductVersionText=new System.Windows.Forms.Label();
			this.lblProductNameText=new System.Windows.Forms.Label();
			this.lblProductName=new System.Windows.Forms.Label();
			this.lblProductVersion=new System.Windows.Forms.Label();
			this.tbpExtended=new System.Windows.Forms.TabPage();
			this.propgrdExtended=new System.Windows.Forms.PropertyGrid();
			this.chkAppRestart=new System.Windows.Forms.CheckBox();
			this.cmdHelp=new System.Windows.Forms.Button();
			this.cmdDebug=new System.Windows.Forms.Button();
			this.llblReportError=new System.Windows.Forms.LinkLabel();
			this.panTitleFrame.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
			this.tabExtended.SuspendLayout();
			this.tbpGeneral.SuspendLayout();
			this.tbpStackTrace.SuspendLayout();
			this.tbpApplication.SuspendLayout();
			this.tbpExtended.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtErrorText
			// 
			this.txtErrorText.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.txtErrorText.BackColor=System.Drawing.SystemColors.Control;
			this.txtErrorText.BorderStyle=System.Windows.Forms.BorderStyle.None;
			this.txtErrorText.Location=new System.Drawing.Point(8, 61);
			this.txtErrorText.Multiline=true;
			this.txtErrorText.Name="txtErrorText";
			this.txtErrorText.Size=new System.Drawing.Size(551, 75);
			this.txtErrorText.TabIndex=0;
			this.txtErrorText.TabStop=false;
			this.txtErrorText.Text="Es ist ein Fehler aufgetreten!";
			// 
			// panTitleFrame
			// 
			this.panTitleFrame.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.panTitleFrame.BackColor=System.Drawing.SystemColors.ActiveCaptionText;
			this.panTitleFrame.BorderStyle=System.Windows.Forms.BorderStyle.FixedSingle;
			this.panTitleFrame.Controls.Add(this.lblTitleText);
			this.panTitleFrame.Controls.Add(this.picIcon);
			this.panTitleFrame.Location=new System.Drawing.Point(-1, -1);
			this.panTitleFrame.Name="panTitleFrame";
			this.panTitleFrame.Size=new System.Drawing.Size(603, 55);
			this.panTitleFrame.TabIndex=5;
			// 
			// lblTitleText
			// 
			this.lblTitleText.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.lblTitleText.Location=new System.Drawing.Point(53, 5);
			this.lblTitleText.Name="lblTitleText";
			this.lblTitleText.Size=new System.Drawing.Size(501, 42);
			this.lblTitleText.TabIndex=2;
			this.lblTitleText.Text="In der Anwendung ist ein Fehler aufgetreten! Klicken Sie auf \"Erweitert\", um sich"+
				" detailierte Informationen anzeigen zu lassen.";
			// 
			// picIcon
			// 
			this.picIcon.Image=((System.Drawing.Image)(resources.GetObject("picIcon.Image")));
			this.picIcon.Location=new System.Drawing.Point(11, 11);
			this.picIcon.Name="picIcon";
			this.picIcon.Size=new System.Drawing.Size(32, 32);
			this.picIcon.TabIndex=1;
			this.picIcon.TabStop=false;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location=new System.Drawing.Point(-2, 458);
			this.groupBox1.Name="groupBox1";
			this.groupBox1.Size=new System.Drawing.Size(592, 6);
			this.groupBox1.TabIndex=6;
			this.groupBox1.TabStop=false;
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom|System.Windows.Forms.AnchorStyles.Right)));
			this.cmdOK.BackColor=System.Drawing.SystemColors.Control;
			this.cmdOK.Cursor=System.Windows.Forms.Cursors.Default;
			this.cmdOK.DialogResult=System.Windows.Forms.DialogResult.OK;
			this.cmdOK.ForeColor=System.Drawing.SystemColors.ControlText;
			this.cmdOK.Location=new System.Drawing.Point(482, 473);
			this.cmdOK.Name="cmdOK";
			this.cmdOK.RightToLeft=System.Windows.Forms.RightToLeft.No;
			this.cmdOK.Size=new System.Drawing.Size(77, 25);
			this.cmdOK.TabIndex=0;
			this.cmdOK.Text="OK";
			this.cmdOK.UseVisualStyleBackColor=false;
			this.cmdOK.Click+=new System.EventHandler(this.cmdOK_Click);
			// 
			// cmdExtended
			// 
			this.cmdExtended.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom|System.Windows.Forms.AnchorStyles.Left)));
			this.cmdExtended.BackColor=System.Drawing.SystemColors.Control;
			this.cmdExtended.Cursor=System.Windows.Forms.Cursors.Default;
			this.cmdExtended.ForeColor=System.Drawing.SystemColors.ControlText;
			this.cmdExtended.Location=new System.Drawing.Point(5, 473);
			this.cmdExtended.Name="cmdExtended";
			this.cmdExtended.RightToLeft=System.Windows.Forms.RightToLeft.No;
			this.cmdExtended.Size=new System.Drawing.Size(77, 25);
			this.cmdExtended.TabIndex=2;
			this.cmdExtended.Text="&Erweitert >";
			this.cmdExtended.UseVisualStyleBackColor=false;
			this.cmdExtended.Click+=new System.EventHandler(this.cmdExtended_Click);
			// 
			// tabExtended
			// 
			this.tabExtended.Anchor=((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom)
						|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.tabExtended.Controls.Add(this.tbpGeneral);
			this.tabExtended.Controls.Add(this.tbpStackTrace);
			this.tabExtended.Controls.Add(this.tbpApplication);
			this.tabExtended.Controls.Add(this.tbpExtended);
			this.tabExtended.Location=new System.Drawing.Point(5, 168);
			this.tabExtended.Multiline=true;
			this.tabExtended.Name="tabExtended";
			this.tabExtended.SelectedIndex=0;
			this.tabExtended.Size=new System.Drawing.Size(555, 287);
			this.tabExtended.TabIndex=6;
			// 
			// tbpGeneral
			// 
			this.tbpGeneral.Controls.Add(this.lblSourceFileText);
			this.tbpGeneral.Controls.Add(this.lblSourceFile);
			this.tbpGeneral.Controls.Add(this.lblWorkingSetText);
			this.tbpGeneral.Controls.Add(this.lblWorkingSet);
			this.tbpGeneral.Controls.Add(this.lblOffsetText);
			this.tbpGeneral.Controls.Add(this.lblOffset);
			this.tbpGeneral.Controls.Add(this.lblLineColumnText);
			this.tbpGeneral.Controls.Add(this.lblLineColumn);
			this.tbpGeneral.Controls.Add(this.lblThreadUserText);
			this.tbpGeneral.Controls.Add(this.lblThreadIDText);
			this.tbpGeneral.Controls.Add(this.lblAppDomainText);
			this.tbpGeneral.Controls.Add(this.lblAssemblyText);
			this.tbpGeneral.Controls.Add(this.lblClassText);
			this.tbpGeneral.Controls.Add(this.lblFunctionText);
			this.tbpGeneral.Controls.Add(this.lblInnerException);
			this.tbpGeneral.Controls.Add(this.lblThreadUser);
			this.tbpGeneral.Controls.Add(this.lblThreadID);
			this.tbpGeneral.Controls.Add(this.lblAppDomain);
			this.tbpGeneral.Controls.Add(this.lblFunction);
			this.tbpGeneral.Controls.Add(this.lblClass);
			this.tbpGeneral.Controls.Add(this.lblAssembly);
			this.tbpGeneral.Controls.Add(this.tvwInnerExceptions);
			this.tbpGeneral.Location=new System.Drawing.Point(4, 22);
			this.tbpGeneral.Name="tbpGeneral";
			this.tbpGeneral.Size=new System.Drawing.Size(547, 261);
			this.tbpGeneral.TabIndex=0;
			this.tbpGeneral.Text="Allgemein";
			// 
			// lblSourceFileText
			// 
			this.lblSourceFileText.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.lblSourceFileText.BorderStyle=System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblSourceFileText.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
			this.lblSourceFileText.Location=new System.Drawing.Point(96, 171);
			this.lblSourceFileText.Name="lblSourceFileText";
			this.lblSourceFileText.Size=new System.Drawing.Size(448, 18);
			this.lblSourceFileText.TabIndex=50;
			this.lblSourceFileText.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblSourceFile
			// 
			this.lblSourceFile.Location=new System.Drawing.Point(5, 171);
			this.lblSourceFile.Name="lblSourceFile";
			this.lblSourceFile.Size=new System.Drawing.Size(90, 18);
			this.lblSourceFile.TabIndex=49;
			this.lblSourceFile.Text="Source-Datei:";
			this.lblSourceFile.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblWorkingSetText
			// 
			this.lblWorkingSetText.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.lblWorkingSetText.BorderStyle=System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblWorkingSetText.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
			this.lblWorkingSetText.Location=new System.Drawing.Point(294, 217);
			this.lblWorkingSetText.Name="lblWorkingSetText";
			this.lblWorkingSetText.Size=new System.Drawing.Size(250, 18);
			this.lblWorkingSetText.TabIndex=48;
			this.lblWorkingSetText.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblWorkingSet
			// 
			this.lblWorkingSet.Location=new System.Drawing.Point(200, 217);
			this.lblWorkingSet.Name="lblWorkingSet";
			this.lblWorkingSet.Size=new System.Drawing.Size(96, 18);
			this.lblWorkingSet.TabIndex=47;
			this.lblWorkingSet.Text="Phys. Speicher:";
			this.lblWorkingSet.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblOffsetText
			// 
			this.lblOffsetText.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.lblOffsetText.BorderStyle=System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblOffsetText.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
			this.lblOffsetText.Location=new System.Drawing.Point(294, 194);
			this.lblOffsetText.Name="lblOffsetText";
			this.lblOffsetText.Size=new System.Drawing.Size(250, 18);
			this.lblOffsetText.TabIndex=46;
			this.lblOffsetText.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblOffset
			// 
			this.lblOffset.Location=new System.Drawing.Point(200, 194);
			this.lblOffset.Name="lblOffset";
			this.lblOffset.Size=new System.Drawing.Size(96, 18);
			this.lblOffset.TabIndex=45;
			this.lblOffset.Text="IL-/Native-Offset:";
			this.lblOffset.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblLineColumnText
			// 
			this.lblLineColumnText.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.lblLineColumnText.BorderStyle=System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblLineColumnText.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
			this.lblLineColumnText.Location=new System.Drawing.Point(96, 194);
			this.lblLineColumnText.Name="lblLineColumnText";
			this.lblLineColumnText.Size=new System.Drawing.Size(250, 18);
			this.lblLineColumnText.TabIndex=44;
			this.lblLineColumnText.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblLineColumn
			// 
			this.lblLineColumn.Location=new System.Drawing.Point(5, 194);
			this.lblLineColumn.Name="lblLineColumn";
			this.lblLineColumn.Size=new System.Drawing.Size(91, 18);
			this.lblLineColumn.TabIndex=43;
			this.lblLineColumn.Text="Zeile/Spalte:";
			this.lblLineColumn.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblThreadUserText
			// 
			this.lblThreadUserText.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.lblThreadUserText.BorderStyle=System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblThreadUserText.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
			this.lblThreadUserText.Location=new System.Drawing.Point(96, 240);
			this.lblThreadUserText.Name="lblThreadUserText";
			this.lblThreadUserText.Size=new System.Drawing.Size(448, 18);
			this.lblThreadUserText.TabIndex=42;
			this.lblThreadUserText.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblThreadIDText
			// 
			this.lblThreadIDText.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.lblThreadIDText.BorderStyle=System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblThreadIDText.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
			this.lblThreadIDText.Location=new System.Drawing.Point(96, 217);
			this.lblThreadIDText.Name="lblThreadIDText";
			this.lblThreadIDText.Size=new System.Drawing.Size(250, 18);
			this.lblThreadIDText.TabIndex=41;
			this.lblThreadIDText.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblAppDomainText
			// 
			this.lblAppDomainText.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.lblAppDomainText.BorderStyle=System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblAppDomainText.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
			this.lblAppDomainText.Location=new System.Drawing.Point(96, 148);
			this.lblAppDomainText.Name="lblAppDomainText";
			this.lblAppDomainText.Size=new System.Drawing.Size(448, 18);
			this.lblAppDomainText.TabIndex=40;
			this.lblAppDomainText.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblAssemblyText
			// 
			this.lblAssemblyText.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.lblAssemblyText.BorderStyle=System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblAssemblyText.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
			this.lblAssemblyText.Location=new System.Drawing.Point(96, 124);
			this.lblAssemblyText.Name="lblAssemblyText";
			this.lblAssemblyText.Size=new System.Drawing.Size(448, 18);
			this.lblAssemblyText.TabIndex=39;
			this.lblAssemblyText.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblClassText
			// 
			this.lblClassText.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.lblClassText.BorderStyle=System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblClassText.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
			this.lblClassText.Location=new System.Drawing.Point(96, 100);
			this.lblClassText.Name="lblClassText";
			this.lblClassText.Size=new System.Drawing.Size(448, 18);
			this.lblClassText.TabIndex=38;
			this.lblClassText.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblFunctionText
			// 
			this.lblFunctionText.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.lblFunctionText.BorderStyle=System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblFunctionText.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
			this.lblFunctionText.Location=new System.Drawing.Point(96, 75);
			this.lblFunctionText.Name="lblFunctionText";
			this.lblFunctionText.Size=new System.Drawing.Size(448, 18);
			this.lblFunctionText.TabIndex=37;
			// 
			// lblInnerException
			// 
			this.lblInnerException.Location=new System.Drawing.Point(5, 9);
			this.lblInnerException.Name="lblInnerException";
			this.lblInnerException.Size=new System.Drawing.Size(91, 13);
			this.lblInnerException.TabIndex=0;
			this.lblInnerException.Text="&Ausnahmen:";
			// 
			// lblThreadUser
			// 
			this.lblThreadUser.Location=new System.Drawing.Point(5, 240);
			this.lblThreadUser.Name="lblThreadUser";
			this.lblThreadUser.Size=new System.Drawing.Size(90, 18);
			this.lblThreadUser.TabIndex=24;
			this.lblThreadUser.Text="Thread-User:";
			this.lblThreadUser.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblThreadID
			// 
			this.lblThreadID.Location=new System.Drawing.Point(5, 217);
			this.lblThreadID.Name="lblThreadID";
			this.lblThreadID.Size=new System.Drawing.Size(90, 18);
			this.lblThreadID.TabIndex=22;
			this.lblThreadID.Text="Thread-ID:";
			this.lblThreadID.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblAppDomain
			// 
			this.lblAppDomain.Location=new System.Drawing.Point(5, 148);
			this.lblAppDomain.Name="lblAppDomain";
			this.lblAppDomain.Size=new System.Drawing.Size(90, 18);
			this.lblAppDomain.TabIndex=20;
			this.lblAppDomain.Text="AppDomain:";
			this.lblAppDomain.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblFunction
			// 
			this.lblFunction.Location=new System.Drawing.Point(5, 78);
			this.lblFunction.Name="lblFunction";
			this.lblFunction.Size=new System.Drawing.Size(91, 12);
			this.lblFunction.TabIndex=17;
			this.lblFunction.Text="Funktion:";
			// 
			// lblClass
			// 
			this.lblClass.Location=new System.Drawing.Point(5, 100);
			this.lblClass.Name="lblClass";
			this.lblClass.Size=new System.Drawing.Size(91, 18);
			this.lblClass.TabIndex=13;
			this.lblClass.Text="Klasse:";
			this.lblClass.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblAssembly
			// 
			this.lblAssembly.Location=new System.Drawing.Point(5, 124);
			this.lblAssembly.Name="lblAssembly";
			this.lblAssembly.Size=new System.Drawing.Size(91, 18);
			this.lblAssembly.TabIndex=11;
			this.lblAssembly.Text="Assembly:";
			this.lblAssembly.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tvwInnerExceptions
			// 
			this.tvwInnerExceptions.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.tvwInnerExceptions.Font=new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tvwInnerExceptions.HideSelection=false;
			this.tvwInnerExceptions.ItemHeight=16;
			this.tvwInnerExceptions.Location=new System.Drawing.Point(96, 6);
			this.tvwInnerExceptions.Name="tvwInnerExceptions";
			this.tvwInnerExceptions.Size=new System.Drawing.Size(448, 64);
			this.tvwInnerExceptions.TabIndex=1;
			this.tvwInnerExceptions.AfterSelect+=new System.Windows.Forms.TreeViewEventHandler(this.tvwInnerExceptions_AfterSelect);
			// 
			// tbpStackTrace
			// 
			this.tbpStackTrace.BorderStyle=System.Windows.Forms.BorderStyle.Fixed3D;
			this.tbpStackTrace.Controls.Add(this.txtStackTrace);
			this.tbpStackTrace.Location=new System.Drawing.Point(4, 22);
			this.tbpStackTrace.Name="tbpStackTrace";
			this.tbpStackTrace.Size=new System.Drawing.Size(492, 261);
			this.tbpStackTrace.TabIndex=1;
			this.tbpStackTrace.Text="Aufrufkette";
			this.tbpStackTrace.Visible=false;
			// 
			// txtStackTrace
			// 
			this.txtStackTrace.Anchor=((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom)
						|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.txtStackTrace.BackColor=System.Drawing.SystemColors.HighlightText;
			this.txtStackTrace.BorderStyle=System.Windows.Forms.BorderStyle.None;
			this.txtStackTrace.Location=new System.Drawing.Point(1, 0);
			this.txtStackTrace.Multiline=true;
			this.txtStackTrace.Name="txtStackTrace";
			this.txtStackTrace.ReadOnly=true;
			this.txtStackTrace.ScrollBars=System.Windows.Forms.ScrollBars.Both;
			this.txtStackTrace.Size=new System.Drawing.Size(487, 257);
			this.txtStackTrace.TabIndex=0;
			this.txtStackTrace.WordWrap=false;
			// 
			// tbpApplication
			// 
			this.tbpApplication.Controls.Add(this.lblFrameworkVersionText);
			this.tbpApplication.Controls.Add(this.lblFrameworkVersion);
			this.tbpApplication.Controls.Add(this.lblOperatingSystemText);
			this.tbpApplication.Controls.Add(this.lblOperatingSystem);
			this.tbpApplication.Controls.Add(this.lblExecutablePathText);
			this.tbpApplication.Controls.Add(this.lblExecutablePath);
			this.tbpApplication.Controls.Add(this.lblProductVersionText);
			this.tbpApplication.Controls.Add(this.lblProductNameText);
			this.tbpApplication.Controls.Add(this.lblProductName);
			this.tbpApplication.Controls.Add(this.lblProductVersion);
			this.tbpApplication.Font=new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbpApplication.Location=new System.Drawing.Point(4, 22);
			this.tbpApplication.Name="tbpApplication";
			this.tbpApplication.Size=new System.Drawing.Size(492, 261);
			this.tbpApplication.TabIndex=2;
			this.tbpApplication.Text="Anwendung";
			// 
			// lblFrameworkVersionText
			// 
			this.lblFrameworkVersionText.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.lblFrameworkVersionText.BorderStyle=System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblFrameworkVersionText.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
			this.lblFrameworkVersionText.Font=new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFrameworkVersionText.Location=new System.Drawing.Point(112, 141);
			this.lblFrameworkVersionText.Name="lblFrameworkVersionText";
			this.lblFrameworkVersionText.Size=new System.Drawing.Size(377, 18);
			this.lblFrameworkVersionText.TabIndex=49;
			this.lblFrameworkVersionText.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblFrameworkVersion
			// 
			this.lblFrameworkVersion.Font=new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFrameworkVersion.Location=new System.Drawing.Point(5, 141);
			this.lblFrameworkVersion.Name="lblFrameworkVersion";
			this.lblFrameworkVersion.Size=new System.Drawing.Size(107, 18);
			this.lblFrameworkVersion.TabIndex=48;
			this.lblFrameworkVersion.Text="Framework-Version:";
			this.lblFrameworkVersion.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblOperatingSystemText
			// 
			this.lblOperatingSystemText.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.lblOperatingSystemText.BorderStyle=System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblOperatingSystemText.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
			this.lblOperatingSystemText.Font=new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblOperatingSystemText.Location=new System.Drawing.Point(112, 117);
			this.lblOperatingSystemText.Name="lblOperatingSystemText";
			this.lblOperatingSystemText.Size=new System.Drawing.Size(377, 18);
			this.lblOperatingSystemText.TabIndex=47;
			this.lblOperatingSystemText.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblOperatingSystem
			// 
			this.lblOperatingSystem.Font=new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblOperatingSystem.Location=new System.Drawing.Point(5, 117);
			this.lblOperatingSystem.Name="lblOperatingSystem";
			this.lblOperatingSystem.Size=new System.Drawing.Size(107, 18);
			this.lblOperatingSystem.TabIndex=46;
			this.lblOperatingSystem.Text="Betriebsystem:";
			this.lblOperatingSystem.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblExecutablePathText
			// 
			this.lblExecutablePathText.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.lblExecutablePathText.BorderStyle=System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblExecutablePathText.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
			this.lblExecutablePathText.Font=new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblExecutablePathText.Location=new System.Drawing.Point(112, 53);
			this.lblExecutablePathText.Name="lblExecutablePathText";
			this.lblExecutablePathText.Size=new System.Drawing.Size(377, 59);
			this.lblExecutablePathText.TabIndex=45;
			// 
			// lblExecutablePath
			// 
			this.lblExecutablePath.Font=new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblExecutablePath.Location=new System.Drawing.Point(5, 56);
			this.lblExecutablePath.Name="lblExecutablePath";
			this.lblExecutablePath.Size=new System.Drawing.Size(107, 12);
			this.lblExecutablePath.TabIndex=44;
			this.lblExecutablePath.Text="Startverzeichnis:";
			// 
			// lblProductVersionText
			// 
			this.lblProductVersionText.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.lblProductVersionText.BorderStyle=System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblProductVersionText.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
			this.lblProductVersionText.Font=new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblProductVersionText.Location=new System.Drawing.Point(112, 29);
			this.lblProductVersionText.Name="lblProductVersionText";
			this.lblProductVersionText.Size=new System.Drawing.Size(377, 18);
			this.lblProductVersionText.TabIndex=42;
			this.lblProductVersionText.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblProductNameText
			// 
			this.lblProductNameText.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.lblProductNameText.BorderStyle=System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblProductNameText.FlatStyle=System.Windows.Forms.FlatStyle.Flat;
			this.lblProductNameText.Font=new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblProductNameText.Location=new System.Drawing.Point(112, 6);
			this.lblProductNameText.Name="lblProductNameText";
			this.lblProductNameText.Size=new System.Drawing.Size(377, 18);
			this.lblProductNameText.TabIndex=41;
			// 
			// lblProductName
			// 
			this.lblProductName.Font=new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblProductName.Location=new System.Drawing.Point(5, 9);
			this.lblProductName.Name="lblProductName";
			this.lblProductName.Size=new System.Drawing.Size(107, 12);
			this.lblProductName.TabIndex=40;
			this.lblProductName.Text="Titel:";
			// 
			// lblProductVersion
			// 
			this.lblProductVersion.Font=new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblProductVersion.Location=new System.Drawing.Point(5, 29);
			this.lblProductVersion.Name="lblProductVersion";
			this.lblProductVersion.Size=new System.Drawing.Size(107, 18);
			this.lblProductVersion.TabIndex=39;
			this.lblProductVersion.Text="Version:";
			this.lblProductVersion.TextAlign=System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tbpExtended
			// 
			this.tbpExtended.Controls.Add(this.propgrdExtended);
			this.tbpExtended.Location=new System.Drawing.Point(4, 22);
			this.tbpExtended.Name="tbpExtended";
			this.tbpExtended.Size=new System.Drawing.Size(492, 261);
			this.tbpExtended.TabIndex=3;
			this.tbpExtended.Text="Erweitert";
			// 
			// propgrdExtended
			// 
			this.propgrdExtended.Anchor=((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Bottom)
						|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.propgrdExtended.HelpVisible=false;
			this.propgrdExtended.LineColor=System.Drawing.SystemColors.ScrollBar;
			this.propgrdExtended.Location=new System.Drawing.Point(0, 0);
			this.propgrdExtended.Name="propgrdExtended";
			this.propgrdExtended.PropertySort=System.Windows.Forms.PropertySort.Alphabetical;
			this.propgrdExtended.Size=new System.Drawing.Size(491, 260);
			this.propgrdExtended.TabIndex=51;
			this.propgrdExtended.ToolbarVisible=false;
			// 
			// chkAppRestart
			// 
			this.chkAppRestart.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Right)));
			this.chkAppRestart.Location=new System.Drawing.Point(421, 144);
			this.chkAppRestart.Name="chkAppRestart";
			this.chkAppRestart.Size=new System.Drawing.Size(144, 16);
			this.chkAppRestart.TabIndex=5;
			this.chkAppRestart.Text="Anwendung neu &starten";
			// 
			// cmdHelp
			// 
			this.cmdHelp.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom|System.Windows.Forms.AnchorStyles.Right)));
			this.cmdHelp.Location=new System.Drawing.Point(395, 473);
			this.cmdHelp.Name="cmdHelp";
			this.cmdHelp.Size=new System.Drawing.Size(77, 25);
			this.cmdHelp.TabIndex=1;
			this.cmdHelp.Text="&Hilfe";
			this.cmdHelp.Click+=new System.EventHandler(this.cmdHelp_Click);
			// 
			// cmdDebug
			// 
			this.cmdDebug.Anchor=((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom|System.Windows.Forms.AnchorStyles.Left)));
			this.cmdDebug.Font=new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmdDebug.Location=new System.Drawing.Point(91, 473);
			this.cmdDebug.Name="cmdDebug";
			this.cmdDebug.Size=new System.Drawing.Size(77, 25);
			this.cmdDebug.TabIndex=3;
			this.cmdDebug.Text="&Debug";
			this.cmdDebug.Click+=new System.EventHandler(this.cmdDebug_Click);
			// 
			// llblReportError
			// 
			this.llblReportError.Anchor=((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top|System.Windows.Forms.AnchorStyles.Left)
						|System.Windows.Forms.AnchorStyles.Right)));
			this.llblReportError.Location=new System.Drawing.Point(8, 144);
			this.llblReportError.Name="llblReportError";
			this.llblReportError.Size=new System.Drawing.Size(394, 16);
			this.llblReportError.TabIndex=4;
			this.llblReportError.TabStop=true;
			this.llblReportError.Text="Diesen Fehler an {1} melden";
			this.llblReportError.LinkClicked+=new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblReportError_LinkClicked);
			// 
			// frmExceptionInfo
			// 
			this.AcceptButton=this.cmdOK;
			this.AutoScaleBaseSize=new System.Drawing.Size(5, 14);
			this.CancelButton=this.cmdOK;
			this.ClientSize=new System.Drawing.Size(564, 503);
			this.Controls.Add(this.llblReportError);
			this.Controls.Add(this.cmdHelp);
			this.Controls.Add(this.chkAppRestart);
			this.Controls.Add(this.tabExtended);
			this.Controls.Add(this.cmdDebug);
			this.Controls.Add(this.cmdOK);
			this.Controls.Add(this.cmdExtended);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.panTitleFrame);
			this.Controls.Add(this.txtErrorText);
			this.Font=new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox=false;
			this.MinimizeBox=false;
			this.Name="frmExceptionInfo";
			this.ShowInTaskbar=false;
			this.SizeGripStyle=System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition=System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text="Fehler in der Anwendung";
			this.Load+=new System.EventHandler(this.frmExceptionInfo_Load);
			this.Closing+=new System.ComponentModel.CancelEventHandler(this.frmExceptionInfo_Closing);
			this.panTitleFrame.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
			this.tabExtended.ResumeLayout(false);
			this.tbpGeneral.ResumeLayout(false);
			this.tbpStackTrace.ResumeLayout(false);
			this.tbpStackTrace.PerformLayout();
			this.tbpApplication.ResumeLayout(false);
			this.tbpExtended.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Gibt das ExceptionInfo-Objekt an, das angezeigt werden soll.
		/// </summary>
		public ExceptionInfo ExceptionInfo
		{
			get { return m_exceptionInfo; }
			set { m_exceptionInfo=value; }
		}

		/// <summary>
		/// Bestimmt, den Titeltext, der angezeigt wird.
		/// </summary>
		public string TitleText
		{
			get { return m_titleText; }
			set { m_titleText=value; }
		}

		/// <summary>
		/// Bestimmt, ob die aufgetretene Exception nicht abgefangen wurde.
		/// </summary>
		public bool UnhandledException
		{
			get { return m_unhandledException; }
			set { m_unhandledException=value; }
		}

		/// <summary>
		/// Bestimmt, ob das LinkLabel "Fehler melden" angezeigt wird.
		/// </summary>
		public bool ShowReportErrorLabel
		{
			get { return m_showReportErrorLabel; }
			set { m_showReportErrorLabel=value; }
		}

		/// <summary>
		/// Bestimmt, ob die Checkbox "Anwendung neu starten" bei unbehandelten Ausnahmen angezeigt wird.
		/// </summary>
		public bool ShowAppRestartCheckBox
		{
			get { return m_showAppRestartCheckBox; }
			set { m_showAppRestartCheckBox=value; }
		}

		/// <summary>
		/// Bestimmt, ob die "Erweitert"-Schaltfl�che angezeigt wird.
		/// </summary>
		public bool ShowExtendedButton
		{
			get { return m_showExtendedButton; }
			set { m_showExtendedButton=value; }
		}

		/// <summary>
		/// Bestimmt, ob die "Debug"-Schaltfl�che angezeigt wird.
		/// </summary>
		public bool ShowDebugButton
		{
			get { return m_showDebugButton; }
			set { m_showDebugButton=value; }
		}

		/// <summary>
		/// Bestimmt, ob die "Hilfe"-Schaltfl�che angezeigt wird.
		/// </summary>
		public bool ShowHelpButton
		{
			get { return m_showHelpButton; }
			set { m_showHelpButton=value; }
		}

		/// <summary>
		/// Bestimmt, ob der Dialog in erweiterter Form angezeigt wird.
		/// </summary>
		public bool ShowExtended
		{
			get { return m_showExtended; }
			set { m_showExtended=value; }
		}

		public string SMTPServer {get; set;}

		public string FromAdress {get; set;}
		public string ToAdress {get; set;}

		public string ToName{get; set;}

		public bool AutoReport{ get; set; }
		#endregion

		#region Private Functions
		/// <summary>
		/// Initialisiert den Dialog.
		/// </summary>
		private void InitDialog()
		{
			// Dialog-Controls initialisieren
			m_initDialog=true;
			this.chkAppRestart.Visible=(m_unhandledException==true&&m_showAppRestartCheckBox==true);
			this.chkAppRestart.Checked=this.chkAppRestart.Visible;
			this.cmdExtended.Visible=m_showExtendedButton;
			this.cmdHelp.Visible=m_showHelpButton;
			this.cmdDebug.Visible=m_showDebugButton;
			this.lblTitleText.Text=TitleText;

			// Dialog ein-/ausklappen
			this.ShowDlgExtended(m_showExtended);
			m_extended=m_showExtended;

			if (null!=m_exceptionInfo)
			{
				m_exception=m_exceptionInfo.Exception;

				// Exception-Informationen anzeigen
				this.ShowException();

				// InnerException-Tree f�llen
				this.CreateInnerExceptionTree(m_exceptionInfo.Exception);

				// ExceptionInfo-Informationen anzeigen
				this.lblAssemblyText.Text=m_exceptionInfo.AssemblyName;
				this.lblAppDomainText.Text=m_exceptionInfo.AppDomainName;
				this.lblThreadIDText.Text=m_exceptionInfo.ThreadId.ToString();
				this.lblThreadUserText.Text=m_exceptionInfo.ThreadUser;
				this.lblWorkingSetText.Text=m_exceptionInfo.WorkingSet.ToString();
				this.lblProductNameText.Text=m_exceptionInfo.ProductName;
				this.lblProductVersionText.Text=m_exceptionInfo.ProductVersion;
				this.lblExecutablePathText.Text=m_exceptionInfo.ExecutablePath;
				this.lblOperatingSystemText.Text=m_exceptionInfo.OperatingSystem.ToString();
				this.lblFrameworkVersionText.Text=m_exceptionInfo.FrameworkVersion.ToString();

				// "Fehler melden"-LinkLabel ein-/ausblenden
				this.llblReportError.Visible=m_showReportErrorLabel;
				this.llblReportError.Text=string.Format(STANDARD_REPORT_TEXT, m_exceptionInfo.CompanyName);
			}
			else
				MessageBox.Show("Die �bergebene ExceptionInfo-Objekt ist ung�ltig!",
					"Fehler",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			m_initDialog=false;
		}


		/// <summary>
		/// Zeigt die Bestandteile der �bergebenen Exception im Fenster an.
		/// </summary>
		private void ShowException()
		{
			if (null!=m_exception)
			{
				if (m_exception.Message.Length>0)
					txtErrorText.Text=m_exception.Message;
				else
					txtErrorText.Text=STANDARD_MESSAGE;

				// ggf. vertikale Scrollbars im Exception-Text-Control anzeigen.
				Graphics gr=txtErrorText.CreateGraphics();
				SizeF stringSize=new SizeF(txtErrorText.Width, txtErrorText.Height);
				SizeF newSize=gr.MeasureString(txtErrorText.Text, new Font(this.Font.Name, this.Font.Size, this.Font.Style), stringSize);
				decimal rowCount=System.Math.Round((decimal)(newSize.Height/LINE_HEIGHT));
				if (rowCount>LINE_COUNT)
					txtErrorText.ScrollBars=ScrollBars.Vertical;

				// Zusatz-Informationen aus dem StackTrace der Exception ermitteln
				this.lblClassText.Text=m_exceptionInfo.GetClassName(m_exception);
				this.lblFunctionText.Text=m_exceptionInfo.GetMethodName(m_exception);
				this.lblSourceFileText.Text=m_exceptionInfo.GetFileName(m_exception);
				this.txtStackTrace.Text=m_exceptionInfo.GetStackTrace(m_exception);
				this.lblLineColumnText.Text=m_exceptionInfo.GetFileLineNumber(m_exception).ToString()+"/"+m_exceptionInfo.GetFileColumnNumber(m_exception).ToString();
				this.lblOffsetText.Text=m_exceptionInfo.GetILOffset(m_exception).ToString()+"/"+m_exceptionInfo.GetNativeOffset(m_exception).ToString();

				// Exception an PropertyGrid binden
				this.propgrdExtended.SelectedObject=m_exception;
			}
			else
				MessageBox.Show("Die �bergebene Exception ist ung�ltig!",
					"Fehler",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
		}


		/// <summary>
		/// F�llt den InnerException-Treeview.
		/// </summary>
		/// <param name="ex"></param>
		private void CreateInnerExceptionTree(Exception ex)
		{
			tvwInnerExceptions.Nodes.Clear();
			TreeNode rootNode=new TreeNode(ex.GetType().ToString());
			rootNode.Tag=ex;
			tvwInnerExceptions.Nodes.Add(rootNode);
			TreeNode parentNode=rootNode;
			Exception currentException=ex;
			while (currentException.InnerException!=null)
			{
				currentException=currentException.InnerException;
				TreeNode currentNode=new TreeNode(currentException.GetType().ToString());
				currentNode.Tag=currentException;
				parentNode.Nodes.Add(currentNode);
				parentNode=currentNode;
			}
			rootNode.ExpandAll();
			tvwInnerExceptions.SelectedNode=rootNode;
		}


		/// <summary>
		/// Erweitert/Verringert den Dialog.
		/// </summary>
		private void ShowDlgExtended(bool extended)
		{
			if (extended)
			{
				this.Height=DIALOG_HEIGHT_MAX;
				cmdExtended.Text="< &Erweitert";
			}
			else
			{
				this.Height=DIALOG_HEIGHT_MIN;
				cmdExtended.Text="&Erweitert >";
			}
			this.tabExtended.Visible=extended;
			this.cmdDebug.Visible=(extended==true&&m_showDebugButton==true);

			if (extended)
				tvwInnerExceptions.Focus();
		}

		/// <summary>
		/// Erweitert/Verringert den Dialog.
		/// </summary>
		private void ShowDlgExtended()
		{
			if (m_extended)
			{
				m_extended=false;
			}
			else
			{
				m_extended=true;
			}
			ShowDlgExtended(m_extended);
		}
		#endregion

		#region Event Handling

		private void frmExceptionInfo_Load(object sender, System.EventArgs e)
		{
			if (AutoReport)
			{
				m_showReportErrorLabel=false;
			}

			InitDialog();

			if (AutoReport)
			{
				llblReportError_LinkClicked(null, null);
			}
		}

		private void frmExceptionInfo_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// Unbehandelte Ausnahme?
			if (m_unhandledException)
			{
				// ggf. Anwendung neu starten
				if (m_showAppRestartCheckBox==true&&this.chkAppRestart.Checked)
					ProcessHelpers.StartProcess(Application.ExecutablePath, "");

				// Anwendung beenden
				Environment.Exit(0);
			}
		}

		private void cmdDebug_Click(object sender, System.EventArgs e)
		{
			Debugger.Launch();
		}

		private void cmdExtended_Click(object sender, System.EventArgs e)
		{
			ShowDlgExtended();
		}

		private void tvwInnerExceptions_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (e.Node!=null&&m_initDialog==false)
			{
				m_exception=(Exception)e.Node.Tag;
				ShowException();
			}
		}

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void cmdHelp_Click(object sender, System.EventArgs e)
		{
			if (m_exception.HelpLink!=null)
				Help.ShowHelp(this, m_exception.HelpLink);
			else
				MessageBox.Show(STANDARD_NO_HELP_TEXT, "Hilfe", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void llblReportError_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			string betreff=String.Format("Bugreport: {0}", m_exceptionInfo.ProductName);
			string message="";

			//Message zusammenbauen
			message+=String.Format("*Bugreport: {0}*\r", m_exceptionInfo.ProductName);
			message+="\r";
			DateTime dt=DateTime.Now;
			message+=String.Format("Datum: {0}\r", dt.ToLongDateString());
			message+=String.Format("Zeit: {0} Uhr\r", dt.ToShortTimeString());
			message+="\r";
			message+="*Fehlermeldung*\r";
			message+=txtErrorText.Text+"\r";
			message+="\r";
			message+="*Allgemeine Informationen*";
			message+="\r";
			foreach (TreeNode i in tvwInnerExceptions.Nodes)
			{
				message+=String.Format("Ausnahmen: {0}\r", i.Text.ToString());
			}
			//message+=String.Format("Ausnahmen: {0}\r", tvwInnerExceptions.Nodes.ToString());
			message+=String.Format("Funktion: {0}\r", lblFunctionText.Text.ToString());
			message+=String.Format("Klasse: {0}\r", lblClassText.Text.ToString());
			message+=String.Format("Assembly: {0}\r", lblAssemblyText.Text.ToString());
			message+=String.Format("AppDomain: {0}\r", lblAppDomainText.Text.ToString());
			message+=String.Format("Source-Datei: {0}\r", lblSourceFileText.Text.ToString());
			message+=String.Format("Zeile/Spalte: {0}\r", lblLineColumnText.Text.ToString());
			message+=String.Format("IL-/Native-Offset: {0}\r", lblOffsetText.Text.ToString());
			message+=String.Format("Thread-ID: {0}\r", lblThreadIDText.Text.ToString());
			message+=String.Format("Phys. Speicher: {0}\r", lblWorkingSetText.Text.ToString());
			message+=String.Format("Thread-User: {0}\r", lblThreadUserText.Text.ToString());
			message+="\r";
			message+="*Stacktrace*";
			message+="\r";
			message+=txtStackTrace.Text.ToString()+"\r"; ;
			message+="\r";
			message+="*Anwendung*";
			message+="\r";
			message+=String.Format("Titel: {0}\r", lblProductNameText.Text.ToString());
			message+=String.Format("Version: {0}\r", lblProductVersionText.Text.ToString());
			message+=String.Format("Startverzeichnis: {0}\r", lblExecutablePathText.Text.ToString());
			message+=String.Format("Betriebsystem: {0}\r", lblOperatingSystemText.Text.ToString());
			message+=String.Format("Framework-Version: {0}\r", lblFrameworkVersionText.Text.ToString());

			SMTP.SendMailMessage(SMTPServer, FromAdress, "Bugmailer", ToAdress, ToName, betreff, message);

			if (!AutoReport)
			{
				MessageBox.Show("Der Fehler wurde gemeldet. Vielen Dank f�r Ihre Unterst�zung.");
			}
		}
		#endregion
	}
}
