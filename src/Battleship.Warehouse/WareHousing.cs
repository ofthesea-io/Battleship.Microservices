namespace Battleship.Warehouse
{
    using System;
    using Scheduler;

    public static class WareHousing
    {
        public static void IntervalInDays(int hour, int min, double interval, Action task)
        {
            interval = interval * 24;
            TaskScheduler.Instance.ScheduleTask(hour, min, interval, task);
        }
    }
}