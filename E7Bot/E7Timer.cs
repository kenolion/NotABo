using System;
using System.Timers;

namespace E7Bot
{
    [Serializable]
    public class E7Timer
    {
        public Timer aTimer;

        public bool isStart;
        public bool isActive;
        

        public E7Timer(double interval)
        {
            aTimer = new Timer(interval);
            isStart = false;
            isActive = true;
        }

        public void SetFunction(ElapsedEventHandler funcTimer)
        {
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += funcTimer;
        }
        public void Start()
        {
            if (!aTimer.Enabled)
            {
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }

            if (isStart)
                aTimer.Stop();
            else
                aTimer.Start();

            isStart = !isStart;
        }

        public void Stop()
        {
            aTimer.Stop();
            isStart = false;
            
        }
    }
}