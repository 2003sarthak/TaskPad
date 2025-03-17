using Newtonsoft.Json;

using ConsoleTables;

using System.Xml;

class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public int Priority { get; set; }
    public string Status { get; set; }
}

class TaskPad
{
    private List<Task> taskList = new List<Task>();
    private string dataFile = "tasks.json";
    public TaskPad()
    {
        LoadTasks();
    }
    // Create new task
    public void CreateTask()
    {
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("=== Create New Task ===");
        Console.ResetColor();
        Task task = new Task();
        task.Id = taskList.Count > 0 ? taskList.Max(t => t.Id) + 1 : 1;
        Console.Write("Title: ");
        task.Title = Console.ReadLine();
        Console.Write("Description: ");
        task.Description = Console.ReadLine();
        while (true)
        {
            Console.Write("Due Date (yyyy-mm-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime dueDate) && dueDate >= DateTime.Now)
            {
                task.DueDate = dueDate;
                break;
            }
            else
            {
                Console.WriteLine("Invalid date! Make sure the date is not in the past.");
            }
        }
        while (true)
        {
            Console.Write("Priority (0-3, with 3 being highest): ");
            if (int.TryParse(Console.ReadLine(), out int priority) && priority >= 0 && priority <= 3)
            {
                task.Priority = priority;
                break;
            }
            else
            {
                Console.WriteLine("Invalid priority! Enter a value between 0 and 3.");
            }
        }
        task.Status = "Pending";
        taskList.Add(task);
        SaveTasks();
        Console.WriteLine("Task created successfully!");
    }

    // Display tasks in a table format using ConsoleTable
    public void DisplayTasks()
    {
        Console.Clear();
        if (taskList.Count == 0)
        {
            Console.WriteLine("No tasks found!");
            return;
        }
        var table = new ConsoleTable("ID", "Title", "Due Date", "Priority", "Status");
        foreach (var task in taskList)
        {
            table.AddRow(task.Id, task.Title, task.DueDate.ToString("yyyy-MM-dd"), task.Priority, task.Status);
        }
        table.Write(Format.Alternative);
    }

    public void MarkTaskAsComplete()
    {
        Console.WriteLine("Existing Tasks:");
        DisplayTasks();
        if (taskList.Count == 0)
        {
            Console.WriteLine("No tasks available to mark as complete.");
            return;
        }
        Console.Write("Enter the Task ID to mark as complete: ");
        if (int.TryParse(Console.ReadLine(), out int userInputTaskId))
        {
            var task = taskList.FirstOrDefault(t => t.Id == userInputTaskId);
            if (task.Status == "Complete")
            {
                Console.WriteLine("Task is already completed!");
            }
            else if (task != null)
            {
                task.Status = "Complete";
                SaveTasks();
                Console.WriteLine($"Task ID {userInputTaskId} marked as complete successfully!");
            }
            else
            {
                Console.WriteLine($"Task with ID {userInputTaskId} not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input! Please enter a valid Task ID.");
        }
    }
    public void EditTask()
    {
        Console.BackgroundColor = ConsoleColor.DarkMagenta; 
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;  
        Console.WriteLine("Existing Tasks:");
        DisplayTasks();
        if (taskList.Count == 0)
        {
            Console.WriteLine("No tasks available to edit.");
            return;
        }

        Console.Write("Enter the Task ID to edit: ");
        if (int.TryParse(Console.ReadLine(), out int userInputTaskId))
        {
            var task = taskList.FirstOrDefault(t => t.Id == userInputTaskId);
            if (task != null)
            {
                Console.WriteLine($"Editing Task ID {userInputTaskId}:");

                Console.Write($"Title ({task.Title}): ");
                string newTitle = Console.ReadLine();
                if (!string.IsNullOrEmpty(newTitle))
                {
                    task.Title = newTitle;
                }

                Console.Write($"Description ({task.Description}): ");
                string newDescription = Console.ReadLine();
                if (!string.IsNullOrEmpty(newDescription))
                {
                    task.Description = newDescription;
                }

                while (true)
                {
                    Console.Write($"Due Date ({task.DueDate:yyyy-MM-dd}): ");
                    string newDueDateInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(newDueDateInput))
                    {
                        break;
                    }
                    else if ((DateTime.TryParse(newDueDateInput, out DateTime newDueDate) && newDueDate >= DateTime.Now))
                    {
                        if (!string.IsNullOrEmpty(newDueDateInput))
                        {
                            task.DueDate = newDueDate;
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid date! Make sure the date is not in the past.");
                    }
                }

                while (true)
                {
                    Console.Write($"Priority ({task.Priority}): ");
                    string newPriorityInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(newPriorityInput))
                    {
                        break;
                    }
                    else if ((int.TryParse(newPriorityInput, out int newPriority) && newPriority >= 0 && newPriority <= 3))
                    {
                        if (!string.IsNullOrEmpty(newPriorityInput))
                        {
                            task.Priority = newPriority;
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid priority! Enter a value between 0 and 3.");
                    }
                }

                Console.WriteLine($"Status ({task.Status}): ");
                string newStatus = Console.ReadLine();
                if (!string.IsNullOrEmpty(newStatus))
                {
                    task.Status = newStatus;
                }

                SaveTasks();
                Console.WriteLine($"Task ID {userInputTaskId} updated successfully!");
            }
            else
            {
                Console.WriteLine($"Task with ID {userInputTaskId} not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input! Please enter a valid Task ID.");
        }
        Console.ResetColor();
    }


    // Save tasks to file
    private void SaveTasks()
    {
        File.WriteAllText(dataFile, JsonConvert.SerializeObject(taskList, Newtonsoft.Json.Formatting.Indented));
    }

    // Load tasks from file
    private void LoadTasks()
    {
        if (File.Exists(dataFile))
        {
            taskList = JsonConvert.DeserializeObject<List<Task>>(File.ReadAllText(dataFile)) ?? new List<Task>();
        }
    }
    public void SortTasksByPriority()
    {
        taskList = taskList.OrderByDescending(t => t.Priority).ToList();
        DisplayTasks();
    }
    public void SortTasksByDate()
    {
        taskList = taskList.OrderBy(t => t.DueDate).ToList();
        DisplayTasks();
    }

    public void ScheduleReminder()
    {
        DisplayTasks();
        Console.Write("Enter the Task ID to edit: ");
        if (int.TryParse(Console.ReadLine(), out int userInputTaskId))
        {
            var task = taskList.FirstOrDefault(t => t.Id == userInputTaskId);
            if (task != null)
            {
                TimeSpan timeUntilDue = task.DueDate - DateTime.Now;
                Console.WriteLine($"Reminder: {timeUntilDue} (this is an object)");
                timeUntilDue = TimeSpan.FromSeconds(5);
                if (timeUntilDue > TimeSpan.Zero)
                {
                    Console.WriteLine($"Reminder set for Task ID {task.Id}: {task.Title} at {task.DueDate}");
                    Thread.Sleep(timeUntilDue);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Reminder: Task ID {task.Id}: {task.Title} is due now!");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine($"Task with ID {userInputTaskId} not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input! Please enter a valid Task ID.");
        }
    }
    public void SearchTaskByTitle()
    {
        Console.Write("Enter the task title to search: ");
        string title = Console.ReadLine();
        var foundTasks = taskList.Where(t => t.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();

        if (foundTasks.Count > 0)
        {
            Console.WriteLine("Search Results:");
            var table = new ConsoleTable("ID", "Title", "Due Date", "Priority", "Status");
            foreach (var task in foundTasks)
            {
                table.AddRow(task.Id, task.Title, task.DueDate.ToString("yyyy-MM-dd"), task.Priority, task.Status);
            }
            table.Write(Format.Alternative);
        }
        else
        {
            Console.WriteLine("No tasks found with the given title.");
        }
    }

    public void DeleteTask()
    {
        Console.WriteLine("Existing Tasks:");
        DisplayTasks();
        if (taskList.Count == 0)
        {
            Console.WriteLine("No tasks available to delete.");
            return;
        }

        Console.Write("Enter the Task ID to delete: ");
        if (int.TryParse(Console.ReadLine(), out int userInputTaskId))
        {
            var task = taskList.FirstOrDefault(t => t.Id == userInputTaskId);
            if (task != null)
            {
                taskList.Remove(task);
                SaveTasks();
                Console.WriteLine($"Task ID {userInputTaskId} deleted successfully!");
            }
            else
            {
                Console.WriteLine($"Task with ID {userInputTaskId} not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input! Please enter a valid Task ID.");
        }
    }
}