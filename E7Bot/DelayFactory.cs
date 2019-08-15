using System;
using System.Windows.Threading;

namespace E7Bot
{
    public static class DelayFactory
    {
        public static void DelayAction(int millisecond, System.Action action)
        {
            var timer = new DispatcherTimer();
            timer.Tick += delegate

            {
                action.Invoke();
                timer.Stop();
            };

            timer.Interval = TimeSpan.FromMilliseconds(millisecond);
            timer.Start();
        }
    }
}