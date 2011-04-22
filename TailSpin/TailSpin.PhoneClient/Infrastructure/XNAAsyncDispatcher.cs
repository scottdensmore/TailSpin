namespace TailSpin.PhoneClient.Infrastructure
{
    using System;
    using System.Windows;
    using System.Windows.Threading;
    using Microsoft.Xna.Framework;

    public class XnaAsyncDispatcher : IApplicationService
    {
        private readonly DispatcherTimer frameworkDispatcherTimer;

        public XnaAsyncDispatcher(TimeSpan dispatchInterval)
        {
            this.frameworkDispatcherTimer = new DispatcherTimer();
            this.frameworkDispatcherTimer.Tick += (s, e) => FrameworkDispatcher.Update();
            this.frameworkDispatcherTimer.Interval = dispatchInterval;
        }

        void IApplicationService.StartService(ApplicationServiceContext context)
        {
            this.frameworkDispatcherTimer.Start();
        }

        void IApplicationService.StopService()
        {
            this.frameworkDispatcherTimer.Stop();
        }
    }
}
