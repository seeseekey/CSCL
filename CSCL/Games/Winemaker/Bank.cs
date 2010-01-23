using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Games
{
	public partial class Winemaker
	{
		public class Bank
		{
			int i_ExportDuty; //Exportzoll in %
			int i_CreditLimit; //Kreditrahmen
			int i_BaseRate; //Kreditzinsen in %

			int i_CurrentCredit; //Kredit zur Zeit
			int i_CurrentCreditRate; //Derzeitige Kreditrate

			Winemaker i_this;

			public int CurrentCredit
			{
				get
				{
					return i_CurrentCredit;
				}
			}

			#region Eigenschaften
			public int ExportDuty
			{
				get
				{
					return i_ExportDuty;
				}
			}

			public int CreditLimit
			{
				get
				{
					return i_CreditLimit;
				}
			}

			public int BaseRate
			{
				get
				{
					return i_BaseRate;
				}
			}
			#endregion

			public Bank(Winemaker wm)
			{
				i_ExportDuty=1;
				i_CreditLimit=30000;
				i_BaseRate=7;

				i_this=wm;
			}

			public bool GetCredit(int Money, int months)
			{
				if(i_CurrentCredit!=0) return false;
				if(Money>i_CreditLimit) return false;

				i_CurrentCredit=Money+(Money/100*i_BaseRate);
				i_CurrentCreditRate=Money/months;

				i_this.PlayerInfo.Money+=Money;

				return true;
			}

			void PayCredit()
			{
				if(i_CurrentCredit==0) return;

				if(i_CurrentCredit<i_CurrentCreditRate)
				{
					i_this.PlayerInfo.Money-=i_CurrentCredit;
					i_CurrentCredit=0;
				}
				else
				{
					i_this.PlayerInfo.Money-=i_CurrentCreditRate;
					i_CurrentCredit-=i_CurrentCreditRate;
				}
			}

			internal void NextMonth()
			{
				PayCredit();
			}

			public bool PayCreditComplete()
			{
				if(i_this.PlayerInfo.Money<i_CurrentCredit) return false;

				i_this.PlayerInfo.Money-=i_CurrentCredit;
				i_CurrentCredit=0;

				return true;
			}
		}
	}
}
