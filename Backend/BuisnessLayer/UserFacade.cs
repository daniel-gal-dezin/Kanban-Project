using IntroSE.Kanban.Backend.DataLayer;
using Microsoft.VisualBasic.CompilerServices;

namespace IntroSE.Kanban.Backend.Backend;
using System;
using IntroSE.Kanban.Backend.BuisnessLayer;
using System.Collections.Generic;
using System.Linq;
using log4net;

public class UserFacade
{
    protected Dictionary<string, User> users;// = new Dictionary<string, User>;
    private  UserController userController = new UserController();
    private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public UserFacade()
    {
        users = new Dictionary<string, User>();
    }





    public void LoadData()
    {
        
        List<UserDTO> real = userController.LoadDatausers();
        foreach (var VARIABLE in real)
        {
            User temp = new User(VARIABLE);
            Users.Add(temp.Email,temp);
        }
        
    }

    public void DeleteData()
    {
        this.userController.DeleteData();
    }
    
    
    
    
    
    
    

    // a side function made for us checking if a user is logged.
    public Boolean isLoggeding(string mail)
    {
        return users[mail].isLoggedIn();
    }

    public LinkedList<int> GetBoardsId(string email)
    {
        LinkedList<int> allBoards = new LinkedList<int>();
        foreach (var board in Users[email].GetBoards())
        {
            allBoards.AddLast(board.BoardId);
        }

        return allBoards;
    }

    //Input - email
    //Output - LinkedList with all og the users in-progress tasks as a value
    public LinkedList<Task> GetAllInProgTasks(string email)
    {
        //checks if the user exist and logged in
        if (!users.ContainsKey(email))
        {
            Log.Warn("a user tried to access his in progress tasks he did not register to the system");
            throw new ArgumentException("this username does not exist");
        }
        if (!this.isLoggeding(email))
        {
            Log.Warn($"the user {users[email].ToString()}tried to access his in progress tasks but wasn't logged in");
            throw new ArgumentException("user not logged in");
        }

        User u = Users[email];
        //calls for the function in the 'User' class.
        return users[email].GetAllInProgTasks(email,u);
    }
    
    //Input - email, password
    //Output - User event, if the login occured properly
    public User logIn(string email,string password)
    {
        if (!users.ContainsKey(email))
        {
            Log.Warn("a user tried to log in but he did not register to the system");
            throw new ArgumentException("user does not exist");
        }   

        User user = users[email];
        
        //calls the boolean function from the user class.
        if (user.logIn(password))
            return user;
        return null;
    }
    
    //Input - email, password
    //Output - User event of the new user registered, if the event occured properly.
    public User register(string email, string password)
    {
        if (users.ContainsKey(email))
        {
            Log.Warn("a user tried to register but was already registered to the system");
            throw new ArgumentException("user already exist");
        }
        User user = new User(email, password);
        UserDTO u = new UserDTO(email, password);
        if (!u.Insert())
        {
            Log.Warn("couldnt insert user into db");
            throw new ArgumentException("user not inserted to db");
        }
        users.Add(email, user);
       
        
        Log.Info("a new user was added to the system");
        return this.logIn(email, password); //when a new user is registered he automatically becomes logged in
    }
    
    //intput - email
    //output- *void. logs out the user given.
    public void logout(string email)
    {
        if (!users.ContainsKey(email))
        {
            Log.Error("user tried to logout but he does not exist in the system");
            throw new ArgumentException("the user does not exist in the system");
        }
      
        users[email].logout();
    }


    
    
    
    
    

    public Dictionary<string, User> Users
    {
        get => users;
        set => users = value;
    }

    public UserController UserController
    {
        get => userController;
        set => userController = value;
    }
    
    
}