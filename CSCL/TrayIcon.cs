using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace CSCL
{
	/// <summary>
	/// Stellt ein Trayicon zur Verfügung
	/// </summary>
	public class TrayIcon
	{
		#region Variablen
		NotifyIcon InstNotifyIcon;
		#endregion

		#region Konstruktor
		public TrayIcon(Icon icon)
		{
			InstNotifyIcon=new NotifyIcon();
			InstNotifyIcon.Visible=true;
			InstNotifyIcon.Icon=icon;
		}

		public TrayIcon(Icon icon, ContextMenu cmenu, string text)
		{
			InstNotifyIcon=new NotifyIcon();
			InstNotifyIcon.Text=text;
			InstNotifyIcon.Visible=true;
			InstNotifyIcon.Icon=icon;
			InstNotifyIcon.ContextMenu=cmenu;
		}

		public TrayIcon(Icon icon, ContextMenuStrip cmenu, string text)
		{
			InstNotifyIcon=new NotifyIcon();
			InstNotifyIcon.Text=text;
			InstNotifyIcon.Visible=true;
			InstNotifyIcon.Icon=icon;
			InstNotifyIcon.ContextMenuStrip=cmenu;
		}

		public TrayIcon(Icon icon, ContextMenu cmenu, string text, bool visible)
		{
			InstNotifyIcon=new NotifyIcon();
			InstNotifyIcon.Text=text;
			InstNotifyIcon.Visible=visible;
			InstNotifyIcon.Icon=icon;
			InstNotifyIcon.ContextMenu=cmenu;
		}

		public TrayIcon(Icon icon, ContextMenuStrip cmenu, string text, bool visible)
		{
			InstNotifyIcon=new NotifyIcon();
			InstNotifyIcon.Text=text;
			InstNotifyIcon.Visible=visible;
			InstNotifyIcon.Icon=icon;
			InstNotifyIcon.ContextMenuStrip=cmenu;
		}
		#endregion

		#region Events
		public EventHandler DoubleClick
		{
			set
			{
				InstNotifyIcon.DoubleClick+=value;
			}
		}
		#endregion

		#region Methoden
		public void Dispose()
		{
			InstNotifyIcon.Dispose();
		}
		#endregion
	}
}
