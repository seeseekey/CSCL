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
