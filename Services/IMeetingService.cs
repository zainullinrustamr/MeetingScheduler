using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetingScheduler.Models;

namespace MeetingScheduler.Services
{
    public interface IMeetingService
    {
        /// <summary>
        /// Метод добавления новой встречи
        /// </summary>
        Meeting AddMeeting(Meeting meeting);
        /// <summary>
        /// Получение встречи по ID 
        /// </summary>
        Meeting? GetMeeting(int id);
        /// <summary>
        /// Получение всех встреч в виде перечисляемой коллекции
        /// </summary>
        /// <returns></returns>
        IEnumerable<Meeting> GetAllMeetings();
        /// <summary>
        /// Получение встреч на конкретную дату
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        IEnumerable<Meeting> GetMeetingsByDate(DateTime date);
        /// <summary>
        /// Удаление встречи по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteMeeting(int id);
        /// <summary>
        /// Проверка свободно ли время для новой встречи
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="excludedMeeting"></param>
        /// <returns></returns>
        bool IsTimeSlotAvailable(DateTime startTime, DateTime endTime, int? excludedMeetingId = null);
    }
}
