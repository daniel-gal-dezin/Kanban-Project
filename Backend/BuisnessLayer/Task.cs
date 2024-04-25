using IntroSE.Kanban.Backend.Backend;
using IntroSE.Kanban.Backend.DataLayer;
using log4net;
using System;


namespace IntroSE.Kanban.Backend.BuisnessLayer;

public class Task
{
    private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private int id;
    private User assign;
    private DateTime creationTime;
    private DateTime dueDate;
    private string title;
    private string description;
    private TaskDTO taskDTO;
    

    public Task(int id, DateTime dueDate, string title, string description)
    {
        this.id = id;
        this.creationTime = DateTime.Today;
        this.assign = null;
        this.dueDate = dueDate;
        if (title.Length > 50)
            throw new Exception("title length is not valid");
        this.title = title;
        if (description != null && description.Length > 300)
        {
            throw new Exception("description is not valid");
        }
        this.description = description;
        this.taskDTO = new TaskDTO(id,null, title, description, creationTime.ToString("yyyy-MM-dd HH:mm:ss"), dueDate.ToString("yyyy-MM-dd HH:mm:ss"),0);
    }

    

    //Input - new due date, title, description.
    //Output - *void. Updates every input that isnt null.
    public void UpdateTask(DateTime newDueDate, string newTitle, string newDescription)
    {
        if (newTitle != null)
        {
            if (newTitle.Length <= 50)
            {
                taskDTO.Title = newTitle;
                this.title = newTitle;
                Log.Info($"task's title was updated");
            }
            else
            {
                Log.Warn($"the task did not update because the title's length was too big");
                throw new ArgumentException("Title length is too big");
            }
        }
        
        //Year '2999' acts as "null" input.
        if (newDueDate.Year != 2999)
        {
            taskDTO.DueDate = newDueDate.ToString("yyyy-MM-dd HH:mm:ss");
            this.dueDate = newDueDate;
            Log.Info($"task's DueDate was updated");
        }
        
        if (newDescription != null)
        {
            if (newDescription.Length <= 300)
            {
                taskDTO.Description = newDescription;
                this.description = newDescription;
                Log.Info($"task's description was updated");
            }
            else
            {
                Log.Warn($"the task did not update because the description's length was too big");
                throw new ArgumentException("Description length is too big");
            }
         
        }
        
        

    }

    public int Id
    {
        get => id;
        set => id = value;
    }

    public DateTime CreationDate
    {
        get => creationTime;
        set => creationTime = value;
    }

    public DateTime DueDate
    {
        get => dueDate;
        set => dueDate = value;
    }

    public string Title
    {
        get => title;
        set => title = value;
    }

    public string Description
    {
        get => description;
        set => description = value;
    }

    public User Assign
    {
        get => assign;
        set => assign = value;
    }
    public TaskDTO TaskDTO
    { 
        get => taskDTO;
        set => taskDTO = value;
    }

}
