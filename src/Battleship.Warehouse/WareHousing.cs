namespace Battleship.Warehouse
{
    using System;

    using Battleship.Warehouse.Scheduler;

    public static class WareHousing
    {
        #region Methods

        public static void IntervalInDays(int hour, int min, double interval, Action task)
        {
            interval = interval * 24;
            TaskScheduler.Instance.ScheduleTask(hour, min, interval, task);
        }

        #endregion
    }
}