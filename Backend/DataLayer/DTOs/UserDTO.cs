using System;
using System.Diagnostics.SymbolStore;
using System.Runtime.CompilerServices;
using IntroSE.Kanban.Backend.Backend;

namespace IntroSE.Kanban.Backend.DataLayer;

public class UserDTO:DalDTO
{
    private UserController ucontroller;
    private String email;
    private String password;
    private Boolean ispersist;



    public UserDTO(string Email, string Password) : base(new UserController())
    {
        this.ucontroller = new UserController();
        this.email = Email;
        this.password = Password;
        this.ispersist = false;

    }


internal bool Insert()
     {
         Boolean work = this.ucontroller.Insert(this);
        if (work)
        {
            this.ispersist = true;
        }
    
        return work;
    }

    


  
    




    public string Email
    {
        get => email;
    }

    public string Password
    {
        get => password;
    }

 

    public bool Ispersist
    {
        get => ispersist;
    }
}