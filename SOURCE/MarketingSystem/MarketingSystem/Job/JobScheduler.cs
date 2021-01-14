using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketingSystem.Job
{
    public class JobScheduler
    {
        public async void Start()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<JobClass>().Build();

            ITrigger trigger = TriggerBuilder.Create()

                .WithIdentity("HelloJob ", "GreetingGroup")

                //.WithSimpleSchedule(s => s.WithIntervalInHours(1).RepeatForever())
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(23, 27))
                .StartAt(DateTime.UtcNow)

                .WithPriority(1)

                .Build();
            await scheduler.ScheduleJob(job, trigger);
        }

        public async void ReStart()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            await scheduler.Shutdown();
            Start();
        }
    }
}