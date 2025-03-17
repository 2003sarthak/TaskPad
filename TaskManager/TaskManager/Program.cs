TaskPad taskPad = new TaskPad();

while (true)
{
    Console.Clear();
    Console.BackgroundColor = ConsoleColor.DarkGreen;
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("=== TaskPad Menu ===");
    Console.ResetColor();
    Console.WriteLine("1. Create Task");
    Console.WriteLine("2. View Tasks");
    Console.WriteLine("3. Mark Task as Complete");
    Console.WriteLine("4. Sort Tasks by Priority");
    Console.WriteLine("5. Sort Tasks by Date");
    Console.WriteLine("6. Edit Task");
    Console.WriteLine("7. Schedule Reminder");
    Console.WriteLine("8. Search Task by Title");
    Console.WriteLine("9. Delete Task");
    Console.WriteLine("10. Exit");
    Console.Write("Choose an option: ");

    if (int.TryParse(Console.ReadLine(), out int choice))
    {
        switch (choice)
        {
            case 1:
                taskPad.CreateTask();
                break;
            case 2:
                taskPad.DisplayTasks();
                break;
            case 3:
                taskPad.MarkTaskAsComplete();
                break;
            case 4:
                taskPad.SortTasksByPriority();
                break;
            case 5:
                taskPad.SortTasksByDate();
                break;
            case 6:
                taskPad.EditTask();
                break;
            case 7:
                taskPad.ScheduleReminder();
                break;
            case 8:
                taskPad.SearchTaskByTitle();
                break;
            case 9:
                taskPad.DeleteTask();
                break;
            case 10:
                return;
            default:
                Console.WriteLine("Invalid choice!");
                break;
        }
    }
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}