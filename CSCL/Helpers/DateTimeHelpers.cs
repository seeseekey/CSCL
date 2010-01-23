using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace CSCL.Helpers
{
	public partial class DateTimeHelpers
	{
		public static string GetDateTimeAsSortableString()
		{
			System.DateTime dt=System.DateTime.Now;
			string DateString=String.Format("{0}{1}{2}-{3}{4}{5}-{6}",
				dt.Year.ToString().PadLeft(4, '0'),
				dt.Month.ToString().PadLeft(2, '0'),
				dt.Day.ToString().PadLeft(2, '0'),
				dt.Hour.ToString().PadLeft(2, '0'),
				dt.Minute.ToString().PadLeft(2, '0'),
				dt.Second.ToString().PadLeft(2, '0'),
				dt.Millisecond.ToString().PadLeft(3, '0'));
			return DateString;
		}

		/// <summary>
		/// Gets the first day of week.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <returns>the first day of the week</returns>
		public static System.DateTime GetFirstDayOfWeek(System.DateTime dateTime)
		{
			while (dateTime.DayOfWeek!=DayOfWeek.Monday)
				dateTime=dateTime.Subtract(new TimeSpan(1, 0, 0, 0));
			return new System.DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
		}

		/// <summary>
		/// Gets the last day of week.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <returns>the last day of the week</returns>
		public static System.DateTime GetLastDayOfWeek(System.DateTime dateTime)
		{
			while (dateTime.DayOfWeek!=DayOfWeek.Sunday)
				dateTime=dateTime.AddDays(1);
			return new System.DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
		}

		/// <summary>
		/// Gets the first day of the month.
		/// </summary>
		/// <param name="givenDate">The given date.</param>
		/// <returns>the first day of the month</returns>
		public static System.DateTime GetFirstDayOfMonth(System.DateTime givenDate)
		{
			return new System.DateTime(givenDate.Year, givenDate.Month, 1);
		}

		/// <summary>
		/// Gets the last day of month.
		/// </summary>
		/// <param name="givenDate">The given date.</param>
		/// <returns>the last day of the month</returns>
		public static System.DateTime GetTheLastDayOfMonth(System.DateTime givenDate)
		{
			return GetFirstDayOfMonth(givenDate).AddMonths(1).Subtract(new TimeSpan(1, 0, 0, 0, 0));
		}

		/// <summary>
		/// ermittelt die Anzahl der Wochentage zwischen zwei Daten 
		/// </summary>
		/// <param name="startTime"></param>
		/// <param name="endTime"></param>
		/// <returns></returns>
		public static int CountWeekdays(System.DateTime startTime, System.DateTime endTime)
		{
			TimeSpan timeSpan=endTime-startTime;
			System.DateTime dateTime;
			int weekdays=0;
			for (int i=0; i<timeSpan.Days; i++)
			{
				dateTime=startTime.AddDays(i);
				if (IsWeekDay(dateTime))
					weekdays++;
			}
			return weekdays;
		}

		/// <summary>
		/// Ermittelt ob der Tag ein WOchentag ist
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static bool IsWeekDay(System.DateTime input)
		{
			if (input.DayOfWeek==DayOfWeek.Saturday) return false;
			if (input.DayOfWeek==DayOfWeek.Sunday) return false;
			return true;
		}

		/// <summary>
		/// Gets the calendarweek.
		/// </summary>
		/// <param name="datetime">The datetime.</param>
		/// <returns>the calendarweek</returns>
		public static int GetCalendarweek(System.DateTime datetime)
		{
			CultureInfo culture=CultureInfo.CurrentCulture;
			int calendarweek=culture.Calendar.GetWeekOfYear(datetime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
			return calendarweek;
		}

		/// <summary>
		/// Berechnet, wann das nächste Quartalsende ist
		/// </summary>
		/// <param name="datum"></param>
		/// <returns></returns>
		public static System.DateTime BerQuartalsEnde(System.DateTime datum)
		{
			int month=((datum.Month-1)/3+1)*3;
			System.DateTime quartal=new System.DateTime(datum.Year, month, 1);
			quartal=quartal.AddMonths(1).AddDays(-1);
			return quartal;
		}
	}
}