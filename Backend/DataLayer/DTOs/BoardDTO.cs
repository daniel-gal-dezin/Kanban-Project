using System;
using System.Diagnostics.CodeAnalysis;
using IntroSE.Kanban.Backend.BuisnessLayer;

namespace IntroSE.Kanban.Backend.DataLayer;


public class BoardDTO:DalDTO
{
    private Boardcontroller _boardcontroller;
    private int boardId;
    private string boardName;
    private string owner;
    private int limitBack;
    private int limitInprog;
    private int limitDone;
    private bool ispersist;
    private string creationDate;
    

    /*public BoardDTO(Board b) : base(new Boardcontroller())
    {
        boardId = b.BoardId;
        boardName = b.Name;
        owner = b.Ownership;
        limitBack = b.getTaskLimit()[0];
        limitInprog = b.getTaskLimit()[1];
        limitDone = b.getTaskLimit()[2];
        creationDate = b.CreationDate.ToString("yyyy-MM-dd HH:mm:ss"); 
        ispersist = false;
        _boardcontroller = new Boardcontroller();
    }*/

    public BoardDTO( int boardId, string boardName, string owner, int limitBack, int limitInprog, int limitDone, string createDate) : base(new Boardcontroller())
    {
        this.boardId = boardId;
        this.boardName = boardName;
        this.owner = owner;
        this.limitBack = limitBack;
        this.limitInprog = limitInprog;
        this.limitDone = limitDone;
        this.creationDate = createDate;
        ispersist = false;
        _boardcontroller = new Boardcontroller();
    }

    internal bool Insert()
    {
        if (this._boardcontroller.Insert(this))
            this.ispersist = true;
        return ispersist;
    }
    public void AddTask(TaskDTO task)
    {
        task.Boardid = this.boardId;
        task.persist();
    }

    public Boardcontroller Boardcontroller
    {
        get => _boardcontroller;
        set => _boardcontroller = value;
    }

    public int BoardId
    {
        get => boardId;
        set => boardId = value;
    }

    public string BoardName
    {
        get => boardName;
        set => boardName = value;
    }

    public string Owner
    {
        get => owner;
        set => owner = value;
    }

    public int LimitBack
    {
        get => limitBack;
        set => limitBack = value;
    }

    public int LimitInprog
    {
        get => limitInprog;
        set => limitInprog = value;
    }

    public int LimitDone
    {
        get => limitDone;
        set => limitDone = value;
    }

    public string CreationDate
    {
        get => creationDate;
        set => creationDate = value;
    }
}