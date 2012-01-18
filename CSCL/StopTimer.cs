//
//  StopTimer.cs
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
using System.Collections.Generic;
using System.Text;

namespace CSCL
{
    /// <summary>
    /// Stellt einen Timer bereit
    /// </summary>
    public class StopTimer
    {
        //TODO: Pause und Resume implementieren

        enum TimerState
        {
            enStop,
            enStart,
            enPause,
            enResume
        }

        TimerState InternalTimerState = TimerState.enStop;

        Int64 StartTime;
        Int64 StopTime;

        public void Start()
        {
            if(InternalTimerState == TimerState.enStart)
            {
                throw new Exception("Timer is already started!");
            }

            StartTime = DateTime.Now.Ticks;
            InternalTimerState = TimerState.enStart;
        }

        public void Stop()
        {
            StopTime = DateTime.Now.Ticks;
            InternalTimerState = TimerState.enStop;
        }

        #region Eigenschaften
        Int64 Difference
        {
            get
            {
                switch(InternalTimerState)
                {
                    case TimerState.enStart:
                        {
                            return DateTime.Now.Ticks - StartTime;
                        }
                    case TimerState.enStop:
                        {
                            return DateTime.Now.Ticks - StartTime;
                        }
                    default:
                        {
                            return 0;
                        }
                }
            }
        }

        public double MicroSeconds
        {
            get
            {
                return Difference / (10.0);
            }
        }

        public double MilliSeconds
        {
            get
            {
                return Difference / (10000.0);
            }
        }

        public double Seconds
        {
            get
            {
                return Difference / (10000000.0);
            }
        }

        public double Minutes
        {
            get
            {
                return Difference / (10000000.0 * 60);
            }
        }

        public double Hours
        {
            get
            {
                return Difference / (10000000.0 * 60 * 60);
            }
        }

        public double Days
        {
            get
            {
                return (10000000.0 * 60 * 60 * 24);
            }
        }

        public double Weeks
        {
            get
            {
                return Difference / (10000000.0 * 60 * 60 * 24 * 7);
            }
        }

        public double Months
        {
            get
            {
                return Difference / (10000000.0 * 60 * 60 * 24 * 30.44);
            }
        }

        public double Years
        {
            get
            {
                return Difference / (10000000.0 * 60 * 60 * 24 * 365.25);
            }
        }
        #endregion
    }
}
