namespace Battleship.Warehouse.Scheduler
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Microservices.Infrastructure.Components;

    public class TaskScheduler : ComponentBase
    {
        #region Fields

        private static TaskScheduler _instance;
        private readonly List<Timer> timers = new List<Timer>();

        #endregion

        #region Constructors

        private TaskScheduler()
        {
        }

        #endregion

        #region Properties

        public static TaskScheduler Instance
        {
            get
            {
                lock (ComponentBase.SyncObject) return TaskScheduler._instance ??= new TaskScheduler();
            }
        }

        #endregion

        #region Methods

        public void ScheduleTask(int hour, int min, double intervalInHour, Action task)
        {
            var now = DateTime.Now;
            var firstRun = new DateTime(now.Year, now.Month, now.Day, hour, min, 0, 0);
            if (now > firstRun) firstRun = firstRun.AddDays(1);

            var timeToGo = firstRun - now;
            if (timeToGo <= TimeSpan.Zero) timeToGo = TimeSpan.Zero;

            var timer = new Timer(x => { task.Invoke(); }, null, timeToGo, TimeSpan.FromHours(intervalInHour));

            this.timers.Add(timer);
        }

        #endregion
    }
}