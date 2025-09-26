using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetingScheduler.Models;


namespace MeetingScheduler.Export
{
    public class MeetingExporter
    {
        public bool ExportToFile(IEnumerable<Meeting> meetings, DateTime date, string filePuth)
        {
            try
            {
                var lines = new List<string>
                {
                    $"Расписание встреч на  {date:dd.MM.yyyy}",
                    "======================================",
                    ""
                };

                var dayMeetings = meetings.Where(m => m.StartTime.Date == date.Date).OrderBy(m => m.StartTime);

                if (!dayMeetings.Any())
                {
                    lines.Add("Встреч нет");
                }
                else
                {
                    foreach (var meeting in dayMeetings)
                    {
                        lines.Add($"Встреча: {meeting.Title}"); 
                        lines.Add($"Описание: {meeting.Description}");
                        lines.Add($"Время: {meeting.StartTime:HH:mm} - {meeting.EndTime:HH:mm}");
                        lines.Add($"Напоминание: за {meeting.NotificationTime.TotalMinutes} минут");
                        lines.Add("--------------------------------------");
                    }
                }

                File.WriteAllLines(filePuth, lines);

                return true;

            }
            catch 
            { 
                return false;
            }
        }      
    }
}
