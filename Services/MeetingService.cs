using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetingScheduler.Models;
using MeetingScheduler.Services;

namespace MeetingScheduler.Services
{
    public class MeetingService : IMeetingService
    {
        /// <summary>
        /// Приватное поле - список всех встреч в памяти
        /// </summary>
        private readonly List<Meeting> _meetings = new();
        /// <summary>
        /// Счетчик для генерации уникальных ID встреч
        /// </summary>
        private int _nextId = 1;
        /// <summary>
        /// Mетода добавления встречи
        /// </summary>
        /// <param name="meeting"></param>
        /// <returns></returns>
        public Meeting AddMeeting(Meeting meeting)
        {
            if (meeting.StartTime <= DateTime.Now)
                throw new ArgumentException("Встреча должна планироваться на будущее время");
            if (meeting.EndTime <= meeting.StartTime)
                throw new ArgumentException("Время окончания должно быть позже времени начала");
            if (!IsTimeSlotAvailable(meeting.StartTime, meeting.EndTime))
                throw new ArgumentException("Встреча пересекается с существующей встречей");
            meeting.ID = _nextId++;
            _meetings.Add(meeting);
            return meeting;
        }
        public bool IsTimeSlotAvailable(DateTime startTime, DateTime endTime, int? excludedMeetingId = null)
        {
            return !_meetings.Any(m => m.ID != excludedMeetingId && startTime < m.EndTime && endTime > m.StartTime);
        }

        public Meeting? GetMeeting(int id)
        {
            return _meetings.FirstOrDefault(m => m.ID == id);
        }

        public IEnumerable<Meeting> GetAllMeetings()
        {
            return _meetings.OrderBy(m => m.StartTime);
        }

        public IEnumerable<Meeting> GetMeetingsByDate(DateTime date)
        { 
            return _meetings.Where(m => m.StartTime.Date == date.Date).OrderBy(m => m.StartTime);
        }

        public bool DeleteMeeting(int id)
        {
            var meeting = GetMeeting(id);
            if (meeting != null)
            {
                return _meetings.Remove(meeting);
            }
            return false;
        }


    }
}
