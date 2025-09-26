using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetingScheduler.Models;

namespace MeetingScheduler.Models
{
    public class Meeting
    {
        /// <summary>
        /// Уникальный ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Название встречи
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Описание встречи
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Дата и время начала встречи
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// Дата и время окончания встречи
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// Время напоминания
        /// </summary>
        public TimeSpan NotificationTime { get; set; }
        /// <summary>
        /// Флаг - было ли уже отправлено напоминание
        /// </summary>
        public bool NotificationSend { get; set; }
    }
}
