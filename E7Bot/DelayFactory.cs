using System;
using System.Windows.Threading;

namespace E7Bot
{
    public static class DelayFactory
    {
        public static void DelayAction(int millisecond, System.Action action, System.Action postInvoke = null)
        {
            var timer = new DispatcherTimer();
            timer.Tick += delegate
            {
                action.Invoke();
                timer.Stop();
                postInvoke?.Invoke();
            };
            timer.Interval = TimeSpan.FromMilliseconds(millisecond);
            timer.Start();
        }
    }
}