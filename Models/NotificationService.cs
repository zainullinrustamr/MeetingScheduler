using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetingScheduler.Models;
using MeetingScheduler.Services;

namespace MeetingScheduler.Models
{
    public class NotificationService
    {
        private readonly IMeetingService _meetingService;

        public NotificationService(IMeetingService meetingService)
        { 
            _meetingService = meetingService;
        }

        public async Task Start()
        {
            while (true)
            {
                CheckNotifications();
                
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }

        private void CheckNotifications()
        {
            var meetings = _meetingService.GetAllMeetings();
            var now = DateTime.Now;

            foreach (var meeting in meetings.Where(m => m.StartTime > now)) 
            {
                if (!meeting.NotificationSend && now >= (meeting.StartTime - meeting.NotificationTime))
                {
                    Console.WriteLine($"\nНАПОМИНАНИЕ: Встреча '{meeting.Title}' начнется в {meeting.StartTime:HH:mm}");
                    meeting.NotificationSend = true;
                }
            }
        }

    }

}
