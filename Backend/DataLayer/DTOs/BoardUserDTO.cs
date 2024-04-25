using System;

namespace IntroSE.Kanban.Backend.DataLayer;

public class BoardUserDTO:DalDTO
{
    private BoardUsercontroller controller;
    private String Email;
    private int Boardid;
    private int Owner;
    private bool ispersist;

    public string Email1
    {
        get => Email;
        set => Email = value;
    }

    public int Boardid1
    {
        get => Boardid;
        set => Boardid = value;
    }

    public int Owner1
    {
        get => Owner;
        set => Owner = value;
    }

    public bool Ispersist
    {
        get => ispersist;
        set => ispersist = value;
    }


    public BoardUserDTO(String email,int boarid,int owner) : base(new BoardUsercontroller())
    {
        controller = new BoardUsercontroller();
        this.Email = email;
        this.Boardid = boarid;
        this.Owner = owner;
        ispersist = false;
    }


  

    internal bool insertboarduser()

    {
        if (!ispersist)
        {
            bool work = controller.insertBoardUser(this);
            if (work)
            {
                ispersist = true;
            }

            return work;
        }

        throw new ArgumentException("already persist");
    }


    internal bool updateOwner()
    {
        if (this.ispersist)
        {
            bool work = controller.updateOwner(this.Email, this.Owner);
            return work;
        }

        throw new ArgumentException("not persist");
    }
    
    
    internal bool deleteBoardUser()
    {
        if (this.ispersist)
        {
            bool work = controller.deleteBoardUser(this.Email, this.Boardid);
            return work;
        }

        throw new ArgumentException("not persist");
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    

   



    
    
    
    
    
    
    
    
    
    
    
    
    
    
}