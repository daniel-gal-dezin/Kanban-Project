using IntroSE.Kanban.Backend.Backend;
using System;

namespace IntroSE.Kanban.Backend.DataLayer;

public class TaskDTO:DalDTO
{
    private TaskController tcontroller;
    private int id;
    private string assignEmail;
    private string title;
    private string description;
    private string creationTime;
    private string dueDate;
    private int colum;
    private int boardid = -1;
    private bool ispersist;


    public TaskDTO(int id, string assignEmail, string title, string description, string creationTime, string dueDate, int colum) : base(new TaskController())
    {
        this.tcontroller = new TaskController();
        this.id = id;
        this.assignEmail = assignEmail;
        this.title = title;
        this.description = description;
        this.creationTime = creationTime;
        this.dueDate = dueDate; 
        this.colum = colum;
        this.ispersist = false;
    }
    
    public TaskDTO(int id, string assignEmail, string title, string description, string creationTime, string dueDate, int colum, int bID) : base(new TaskController())
    {
        this.tcontroller = new TaskController();
        this.id = id;
        this.assignEmail = assignEmail;
        this.title = title;
        this.description = description;
        this.creationTime = creationTime;
        this.dueDate = dueDate; 
        this.colum = colum;
        this.ispersist = false;
        this.boardid = bID;
    }
    internal void persist()
    {
        bool work = this.tcontroller.Insert(this);
        if (work)
        {
            this.ispersist = true;
        }

    }
    internal void updatecolum(int newcolum)
    {
        tcontroller.updatecolum(newcolum, this.id);
    }
    public int Id
    {
        get => id; 
    }
    public string AssignEmail
    {
        get => assignEmail;
        set
        {
            if (tcontroller.updateAssign(value, this.id)){
                this.assignEmail = value;
            }
        }
    }
    public string Title
    { 
        get => title;
        set
        {
            if (tcontroller.updateTitle(value, this.id))
            {
                this.title = value;
            }
        }
    }
    public string Description
    {
        get => description;
        set
        {
            if (tcontroller.updateDescription(value, this.id))
            {
                this.description = value;
            }
        }
    }
    
    public string CreationTime
    {
        get => creationTime; 
    }
    public string DueDate
    {
        get => dueDate;
        set
        {
            if (tcontroller.updateDuedate(value, this.id))
            {
                this.dueDate = value;
            }
        }
    }
    public int Colum
    {
        get => colum;
    }
    public int Boardid
    {
        get=> boardid;
        set => boardid = value;
    }
    public bool Ispersist
    {
        get => ispersist;
        set => ispersist = value;
    }
}