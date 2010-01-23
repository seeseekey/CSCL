using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CSCL.Network
{
	/// <summary>
	/// Ping functionality in .Net 
	/// </summary>
	public class Ping
	{
		protected Socket socket;
		protected bool isOpen;
		protected ManualResetEvent readComplete;
		protected byte lastSequenceNr=0;
		protected byte[] pingCommand;
		protected byte[] pingResult;

		/// <summary>
		/// Initializes a new instance of the <see cref="Pinger"/> class.
		/// </summary>
		public Ping()
		{
			pingCommand=new byte[8];
			pingCommand[0]=8; // Type
			pingCommand[1]=0; // Subtype
			pingCommand[2]=0; // Checksum
			pingCommand[3]=0;
			pingCommand[4]=1; // Identifier
			pingCommand[5]=0;
			pingCommand[6]=0; // Sequence number
			pingCommand[7]=0;

			pingResult=new byte[pingCommand.Length+1000];
		}

		/// <summary>
		/// Opens this instance.
		/// </summary>
		public void Open()
		{
			socket=new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
			isOpen=true;
			readComplete=new ManualResetEvent(false);
		}

		/// <summary>
		/// Closes this instance.
		/// </summary>
		public void Close()
		{
			isOpen=false;
			socket.Close();
			readComplete.Close();
		}

		/// <summary>
		/// Sends the Ping to the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="timeout">The timeout.</param>
		/// <returns></returns>
		public TimeSpan Send(string address, TimeSpan timeout)
		{
			while(socket.Available>0)
				socket.Receive(pingResult, System.Math.Min(socket.Available, pingResult.Length), SocketFlags.None);

			readComplete.Reset();
			DateTime timeSend=DateTime.Now;
			pingCommand[6]=lastSequenceNr++;
			SetChecksum(pingCommand);
			int iSend=socket.SendTo(pingCommand, new IPEndPoint(IPAddress.Parse(address), 0));
			socket.BeginReceive(pingResult, 0, pingResult.Length, SocketFlags.None, new AsyncCallback(CallBack), null);

			if(readComplete.WaitOne(timeout, false))
			{
				if((pingResult[20]==0)&&(pingCommand[4]==pingResult[24])&&(pingCommand[5]==pingResult[25])&&
					(pingCommand[6]==pingResult[26])&&(pingCommand[7]==pingResult[27]))
					return DateTime.Now.Subtract(timeSend);
			}
			return TimeSpan.MaxValue;
		}

		/// <summary>
		/// CallBack.
		/// </summary>
		/// <param name="result">The result.</param>
		protected void CallBack(IAsyncResult result)
		{
			if(isOpen)
			{
				try
				{
					socket.EndReceive(result);
				}
				catch(Exception)
				{
				}
				readComplete.Set();
			}
		}

		/// <summary>
		/// Sets the checksum.
		/// </summary>
		/// <param name="tel">The tel.</param>
		private void SetChecksum(byte[] tel)
		{
			tel[2]=0;
			tel[3]=0;
			uint cs=0;

			for(int i=0; i<pingCommand.Length; i=i+2)
				cs+=BitConverter.ToUInt16(pingCommand, i);
			cs=~((cs&0xffffu)+(cs>>16));
			tel[2]=(byte)cs;
			tel[3]=(byte)(cs>>8);
		}
	}
}
