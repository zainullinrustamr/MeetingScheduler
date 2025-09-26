using MeetingScheduler.Export;
using MeetingScheduler.Models;
using MeetingScheduler.Services;

namespace MeetingScheduler;

class Program
{
    private static IMeetingService _meetingService = new MeetingService();
    private static NotificationService _notificationService;
    private static MeetingExporter _exporter = new MeetingExporter();

    static void Main(string[] args)
    {
        _notificationService = new NotificationService(_meetingService);
        _notificationService.Start();

        Console.WriteLine("=== МЕНЕДЖЕР ВСТРЕЧ ===");

        ShowMainMenu();
   
    }

    static void ShowMainMenu()
    {
        while (true)
        {
            Console.WriteLine("\nГЛАВНОЕ МЕНЮ:");
            Console.WriteLine("1. Просмотреть встречи на день");
            Console.WriteLine("2. Добавить встречу");
            Console.WriteLine("3. Редактировать встречу");
            Console.WriteLine("4. Удалить встречу");
            Console.WriteLine("5. Экспорт встреч в файл");
            Console.WriteLine("0. Выход");

            Console.Write("Выберите действие: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewMeetingsForDay();
                    break;
                case "2":
                    AddMeeting();
                    break;
                case "3":
                    EditMeeting();
                    break;
                case "4":
                    DeleteMeeting();
                    break;
                case "5":
                    ExportMeetings();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }
    }

    static void ViewMeetingsForDay()
    {
        Console.Write("Введите дату (дд.мм.гггг): ");
        if (DateTime.TryParse(Console.ReadLine(), out var date))
        {
            var meetings = _meetingService.GetMeetingsByDate(date);

            Console.WriteLine($"\nВстречи на {date:dd.MM.yyyy}:");
            Console.WriteLine("==============================");

            if (!meetings.Any())
            {
                Console.WriteLine("Встреч нет");
            }
            else
            {
                foreach (var meeting in meetings)
                {
                    Console.WriteLine($"ID: {meeting.ID}");
                    Console.WriteLine($"Встреча: {meeting.Title}");
                    Console.WriteLine($"Описание: {meeting.Description}");
                    Console.WriteLine($"Время: {meeting.StartTime:HH:mm} - {meeting.EndTime:HH:mm}");
                    Console.WriteLine($"Напоминание: за {meeting.NotificationTime.TotalMinutes} минут");
                    Console.WriteLine("------------------------------");
                }
            }
        }
        else
        {
            Console.WriteLine("Неверный формат даты!");
        }
    }

    static void AddMeeting()
    {
        try
        {
            var meeting = new Meeting();

            Console.Write("Название встречи: ");
            meeting.Title = Console.ReadLine() ?? "";

            Console.Write("Описание: ");
            meeting.Description = Console.ReadLine() ?? "";

            Console.Write("Дата и время начала (дд.мм.гггг чч:мм): ");
            if (!DateTime.TryParse(Console.ReadLine(), out var startTime))
            {
                Console.WriteLine("Неверный формат даты!");
                return;
            }
            meeting.StartTime = startTime;

            Console.Write("Дата и время окончания (дд.мм.гггг чч:мм): ");
            if (!DateTime.TryParse(Console.ReadLine(), out var endTime))
            {
                Console.WriteLine("Неверный формат даты!");
                return;
            }
            meeting.EndTime = endTime;

            Console.Write("Напоминание за (минут): ");
            if (int.TryParse(Console.ReadLine(), out var minutes))
            {
                meeting.NotificationTime = TimeSpan.FromMinutes(minutes);
            }

            var addedMeeting = _meetingService.AddMeeting(meeting);
            Console.WriteLine($"Встреча добавлена! ID: {addedMeeting.ID}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void EditMeeting()
    {
        Console.Write("Введите ID встречи для редактирования: ");
        if (int.TryParse(Console.ReadLine(), out var id))
        {
            var meeting = _meetingService.GetMeeting(id);
            if (meeting == null)
            {
                Console.WriteLine("Встреча не найдена!");
                return;
            }

            try
            {
                Console.Write($"Название ({meeting.Title}): ");
                var title = Console.ReadLine();
                if (!string.IsNullOrEmpty(title))
                    meeting.Title = title;

                Console.Write($"Описание ({meeting.Description}): ");
                var description = Console.ReadLine();
                if (!string.IsNullOrEmpty(description))
                    meeting.Description = description;

                Console.Write($"Дата и время начала ({meeting.StartTime:dd.MM.yyyy HH:mm}): ");
                var startInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(startInput) && DateTime.TryParse(startInput, out var startTime))
                    meeting.StartTime = startTime;

                Console.Write($"Дата и время окончания ({meeting.EndTime:dd.MM.yyyy HH:mm}): ");
                var endInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(endInput) && DateTime.TryParse(endInput, out var endTime))
                    meeting.EndTime = endTime;

                Console.Write($"Напоминание за (минут) (текущее: {meeting.NotificationTime.TotalMinutes}): ");
                var notificationInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(notificationInput) && int.TryParse(notificationInput, out var minutes))
                    meeting.NotificationTime = TimeSpan.FromMinutes(minutes);

                meeting.NotificationSend = false;
               
                Console.WriteLine("Встреча обновлена!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Неверный ID!");
        }
    }

    static void DeleteMeeting()
    {
        Console.Write("Введите ID встречи для удаления: ");
        if (int.TryParse(Console.ReadLine(), out var id))
        {
            if (_meetingService.DeleteMeeting(id))
            {
                Console.WriteLine("Встреча удалена!");
            }
            else
            {
                Console.WriteLine("Встреча не найдена!");
            }
        }
        else
        {
            Console.WriteLine("Неверный ID!");
        }
    }

    static void ExportMeetings()
    {
        Console.Write("Введите дату для экспорта (дд.мм.гггг): ");
        if (DateTime.TryParse(Console.ReadLine(), out var date))
        {
            var fileName = $"meetings_{date:yyyyMMdd}.txt";
            var meetings = _meetingService.GetMeetingsByDate(date);

            if (_exporter.ExportToFile(meetings, date, fileName))
            {
                Console.WriteLine($"Данные экспортированы в файл: {fileName}");
            }
            else
            {
                Console.WriteLine("Ошибка при экспорте!");
            }
        }
        else
        {
            Console.WriteLine("Неверный формат даты!");
        }
    }
}