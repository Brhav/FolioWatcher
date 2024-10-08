﻿using Quartz;

namespace FolioWatcher
{
    public class ScheduledTaskJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            // This method will be executed at 7 AM daily
            Console.WriteLine("Scheduled method executed at: " + DateTime.Now);

            var downloader = new Downloader();

            await downloader.RunAsync();
        }
    }
}