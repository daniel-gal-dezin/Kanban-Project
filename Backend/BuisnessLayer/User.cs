using IntroSE.Kanban.Backend.BuisnessLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using IntroSE.Kanban.Backend.DataLayer;

namespace IntroSE.Kanban.Backend.Backend;

public class User
{
    private string email;
    private string password;
    private bool loggedIn;
    private LinkedList<Board> boards =new LinkedList<Board>();

    private static readonly ILog Log =
        LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


    public User(string email, string password)
    {
        //2 exception that are throwed if the password doesn't meet requirements
        if (password.Length < 6 || password.Length > 20)
        {
            Log.Warn("the constructure failed to create a new user because the password is not in the right length");
            throw new ArgumentException("password length is illegal");
        }

        if (!checkPass(password))
        {
            Log.Warn(
                "the constructure failed to create a new user because the password does not meet the requirements");
            throw new ArgumentException("password length is illegal");
        }

        this.email = email;
        this.password = password;
        this.loggedIn = false;
        
      
        
    }

    public User(UserDTO u)
    {
        this.email = u.Email;
        this.password = u.Password;
        this.loggedIn = false;
        

    }
    
    
    
    
    
    
    
    

    // Checks if the password has at least one uppercase letter,
    // one small character, and a number
    private bool checkPass(string password)
    {
        Regex reg = new Regex("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)");
        Match match = reg.Match(password);
        return match.Success;
    }

    //logs in the user if the password is correct
    public bool logIn(string password)
    {
        if (this.password.Equals(password))
        {
            this.loggedIn = true;
            Log.Info($"the user {this.Email.ToString()} logged in successfully");
            return true;
        }

        Log.Warn(
            $"the user {this.Email.ToString()} failed to log in because the password that was entered is incorrect");
        return false;
    }

    //logs out a given user
    public void logout()
    {
        if (!isLoggedIn())
        {
            Log.Warn($"the user {this.Email.ToString()} tried to log out but he was already logged out");
            throw new ArgumentException("the user is already logged out");
        }

        this.loggedIn = false;
        Log.Info($"the user {this.Email.ToString()} logged out successfully");
    }

    public bool isLoggedIn()
    {
        return this.loggedIn;
    }

    public bool FindBoard(string boardName)
    {
        LinkedListNode<Board> current = boards.First;
        while (current != null)
        {
            if (current.Value.Name == boardName)
                return true;
            current = current.Next;
        }

        return false;
    }

    public LinkedList<Board> GetBoards()
    {
        LinkedList<Board> boardList = this.boards;
        Log.Info($"the user {this.Email.ToString()} got his boards successfully");
        return boardList;
    }

    //the Users function to get all in progress tasks
    //returns linkedlist of tasks.
    public LinkedList<Task> GetAllInProgTasks(String mail,User u)
    {
        LinkedList<Task> inProgTasks = new LinkedList<Task>();

        //iterate boards
        foreach (var var1 in boards)
        {
            LinkedList<Task> boardInProgTasks = var1.GetTasks(1);

            //iterate tasks of the inprogress column for each board.
            foreach (Task task in boardInProgTasks)
                if (var1.Ownership == mail)
                {
                    inProgTasks.AddLast(task);
                }
                else
                {
                    if (task.Assign.Equals(u))
                    {
                        inProgTasks.AddLast(task);  
                    }
                    
                }
        }

        Log.Info($"the user {this.Email.ToString()} got all of his in progress tasks");
        return inProgTasks;
    }

    public void AddBoard(Board newBoard)
    {
        this.boards.AddLast(newBoard);
        Log.Info($"the user {this.Email.ToString()} added a new board successfully");
    }

    public void RemoveBoard(int boardId)
    {
        foreach (var board in boards)
        {
            if (board.BoardId == boardId)
            {
                boards.Remove(board);
                Log.Info($"the user {this.Email.ToString()} removed a board successfully");
                return;
            }
        }
    }

    public Board GetBoard(int boardId)
    {
        foreach (var board in boards)
        {
            if (board.BoardId == boardId)
            {
                return board;
            }
        }

        return null;
    }

    public Board getBoard(string boardName)
    {
        foreach (var board in boards)
        {
            if (board.Name == boardName)
            {
                return board;
            }
        }

        return null;
    }

   

    public string Email
    {
        get => email;
        set => email = value;
    }

    public string Password
    {
        get => password;
        set => password = value;
    }
}