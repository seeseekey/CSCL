using System;
using System.Threading;

namespace CSCL.Network.Ftp
{
    internal class FTPSemaphore
    {
        private object syncLock = new object();
        long count = 0;

        internal FTPSemaphore(int initCount)
        {
            count = initCount;
        }

        internal void WaitOne(int timeoutMillis)
        {
            lock (syncLock)
            {
                while (count == 0)
                    Monitor.Wait(syncLock, timeoutMillis);
                count--;
            }
        }

        internal void Release()
        {
            lock (syncLock)
            {
                count++;
                Monitor.Pulse(syncLock);
            }
        }
    }
}
