using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.Backend;
using System.Threading;
using IntroSE.Kanban.Backend.DataLayer;

namespace IntroSE.Kanban.Backend.BuisnessLayer;

public class Board
{
    private int boardId;
    private string name;
    private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private DateTime creationDate;
    private string owner;
    private int[] taskLimitations;
    private LinkedList<Task>[] tasks;
    private int taskIDCounter;
    private LinkedList<User> members;

    public Board(User owner, string name, int boardId)
    {
        this.boardId = boardId;
        this.name = name;
        this.owner = owner.Email;
        creationDate = DateTime.Today;
        taskLimitations = new int[3];
        //'-1' acts as "No limit".
        for (int i = 0; i < taskLimitations.Length; i++)
            taskLimitations[i] = -1;
        tasks = new LinkedList<Task>[3];
        tasks[0] = new LinkedList<Task>();
        tasks[1] = new LinkedList<Task>();
        tasks[2] = new LinkedList<Task>();
        taskIDCounter = 1;
        members = new LinkedList<User>();
        members.AddFirst(owner);
    }

    public Board(BoardDTO boardDto)
    {
        this.boardId = boardDto.BoardId;
        this.name = boardDto.BoardName;
        this.owner = boardDto.Owner;
        this.members = new LinkedList<User>();
        this.taskLimitations = new int[3];
        this.taskLimitations[0] = boardDto.LimitBack;
        this.taskLimitations[1] = boardDto.LimitInprog;
        this.taskLimitations[2] = boardDto.LimitDone;
        tasks = new LinkedList<Task>[3];
        tasks[0] = new LinkedList<Task>();
        tasks[1] = new LinkedList<Task>();
        tasks[2] = new LinkedList<Task>();
        taskIDCounter = 1;
        DateTime.TryParseExact(boardDto.CreationDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out this.creationDate);

    }

    public void AddMember(User u)
    {
        members.AddLast(u);
    }

    
    
    public void RemoveMember(string email)
    {
        foreach (var member in members)
        {
            if (member.Email == email)
            {
                members.Remove(member);
                Log.Info("Member has been removed from a board successfully");
                //removes the member from his assigned tasks that are not done
                UnAssignTasks(member);
                return;
            }
        }
        
    }
    private void UnAssignTasks(User member)
    {
        foreach (var task in tasks[0])
        {
            if (task.Assign.Equals(member))
            {
                Log.Info("Task in backlog has been unAssigned successfully");
                task.Assign = null;
                task.TaskDTO.AssignEmail = null;
            }

        }
        foreach (var task in tasks[1])
        {
            if (task.Assign.Equals(member))
            {
                Log.Info("Task in Inprogress has been unAssigned successfully");
                task.Assign = null;
                task.TaskDTO.AssignEmail = null;
            }

        }

    }

    public User FindUser(string email)
    {
        foreach (var member in members)
        {
            if (member.Email == email)
                return member;
        }

        return null;
    }
    
    //side function to check if a task name already exists.
    public Boolean Containtask(string title)
    {
        LinkedListNode<Task> first = tasks[0].First;
        while (first != null)
        {
            if (first.Value.Title == title)
                return true;
           first =  first.Next;
           
        }
        first = tasks[1].First;
        while (first != null)
        {
            if (first.Value.Title == title)
                return true;
            first = first.Next;
        }
        
        first = tasks[2].First;
        while (first != null)
        {
            if (first.Value.Title == title)
                return true;
            first =  first.Next;
        }
        return false;
    }
    
    //input - title, description, due date
    //output - void*. Adds the task to the backlog list.
    public void AddTask(string title, string description, DateTime dueDate)
    {
        //check if a task with that name already exists.
        if (Containtask(title))
        {
            Log.Warn("task was not added because the task already exists");
            throw new Exception("task already exists");
        }
        
        //checks that the limitation didnt reach.
        if ((taskLimitations[0] == -1 | taskLimitations[0] - tasks[0].Count() > 0))
        {
            Task t = new Task(taskIDCounter, dueDate, title, description);
            tasks[0].AddLast(t);
            BoardDTO boardDto = new BoardDTO(boardId, name, owner, this.taskLimitations[0], this.taskLimitations[1], this.taskLimitations[2], creationDate.ToString());
            boardDto.AddTask(t.TaskDTO);
            Log.Info("task was added successfully");
            taskIDCounter++; //Updates the task ID counter of the board.
            return;
        }
        Log.Warn("task was not added because the task limitation was reached");
        throw new Exception("Task limitation reached.");
        
    }

    public void LimitTasks(int taskLimitation, int taskOrdinal)
    {
        Log.Info("task was limited successfully");
        taskLimitations[taskOrdinal] = taskLimitation;
    }
    
    //input - task ordinal, task id
    //output - void*. Advances the given task id.
    public void ForwardTask(string mail, int taskOrdinal, int taskID)
    {
        
        //exception if a done task tried to be advanced.
        if (taskOrdinal == 2)
        {
            Log.Warn("user tried to forward a task that was already in done");
            throw new ArgumentException("Cannot forward done tasks!");
        }

            if (taskOrdinal == 0 || taskOrdinal == 1)
            {
                LinkedListNode<Task> currentNode = tasks[taskOrdinal].First;
                while (currentNode != null)
                {
                    if (currentNode.Value.Id == taskID)
                    {
                        tasks[taskOrdinal + 1].AddLast(currentNode.Value);
                        tasks[taskOrdinal].Remove(currentNode);
                        //updating the colum on the database
                        int colum = currentNode.Value.TaskDTO.Colum;
                        colum = colum + 1;
                        currentNode.Value.TaskDTO.updatecolum(colum);
                        Log.Info("task has been forwarded successfully");
                            return;
                    }

                    currentNode = currentNode.Next;
                }

                Log.Warn("user tried to forward a task but the task does not exist");
                throw new Exception("Task ID wasn't found");
            }

     
        
    }
    

    //gets task list of a given column ordinal.
    public LinkedList<Task> GetTasks(int taskOrdinal)
    {
        Log.Info("tasks information retrieved successfully");
        return tasks[taskOrdinal];
    }

    public void AssignTask(User assign,int columnOrdinal,int taskid,User Assignee)
    {
        if (!members.Contains(assign) & !(assign.Email==this.owner))
        {
            Log.Warn("try to assign but user not member in the board");
            throw new ArgumentException("user not member in board");
        }
        if (!members.Contains(Assignee)& !(Assignee.Email==this.owner))
        {
            Log.Warn("try to assign but assigni not member in the board");
            throw new ArgumentException("asigni not member in board");
        }
        Task ans = null;
        foreach (var task in tasks[columnOrdinal]) {
            if(task.Id == taskid)
            {
                ans = task;
                break;

            }

        }

        if (ans!=null)
        {
            if(ans.Assign==null || ans.Assign.Equals(assign))
            {
                Log.Info("assign successfully");
                ans.Assign = Assignee;
                ans.TaskDTO.AssignEmail = Assignee.Email;
            }
            else
            {
                Log.Warn("try to assign but the one trying to assign is not the current assignee");
                throw new ArgumentException("try to assign but the one trying to assign is not the current assignee");
            }
        }
        else 
        {
            Log.Warn("try to assign but task not exist");
            throw new ArgumentException("task not exist in this ordinal");
        }
        
        
       
    }
   





    public string Name
    {
        get => name;
        set => name = value;
    }

    public DateTime CreationDate
    {
        get => creationDate;
        set => creationDate = value;
    }

    public int[] TaskLimitations
    {
        get => taskLimitations;
        set => taskLimitations = value;
    }

    public LinkedList<Task>[] Tasks
    {
        get => tasks;
        set => tasks = value;
    }

    public int[] getTaskLimit()
    {
        return this.TaskLimitations;
    }

    public string Ownership
    {
        get => owner;
        set => owner = value;
    }

    public LinkedList<User> Members
    {
        get => members;
        set => members = value;
    }

    public int BoardId
    {
        get => boardId;
        set => boardId = value;
    }
}