    using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
    using System.Threading.Tasks;
    using IntroSE.Kanban.Backend.Backend;
using IntroSE.Kanban.Backend.BuisnessLayer;
    using IntroSE.Kanban.Backend.DataLayer;
    using log4net;
using Microsoft.SqlServer.Server;
using Microsoft.VisualBasic;
using Task = IntroSE.Kanban.Backend.BuisnessLayer.Task;
public class BoardFacade
{
    
    private int boardIDCounter;
    private Boardcontroller _boardcontroller;
    private BoardUsercontroller _boardUsercontroller;
    private TaskController _taskcontroller;
    private  Dictionary<string,Dictionary<string,Board>> boards = new Dictionary<string, Dictionary<string ,Board>>();
   private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
   private UserFacade uf;

  
   //first string for user mail, second string for Board name this to use as keys to get specific user
   //these are our special unique keys. 
   public BoardFacade(UserFacade userFacade)
   {
       this.uf = userFacade;
       boardIDCounter = 1;
       _boardcontroller = new Boardcontroller();
       _boardUsercontroller = new BoardUsercontroller();
       _taskcontroller = new TaskController();
   }
   
   //side function to check if a given user is logged in.
   public Boolean isLoggeding(string mail)
   {
      return uf.Users[mail].isLoggedIn();
   }
   
   //intput - owner email, board name
   //output- Board event of the board created.
   public Board CreateBoard(string mail, string boardName)
   {
        
        if (!uf.Users.ContainsKey(mail))
        {
            Log.Warn("Board creation failed because the user does not exist");
            throw new ArgumentException("No user with that ID");
        }
      if (!this.isLoggeding(mail))
      {
            Log.Warn("Board creation failed because the user is not logged in");
            throw new ArithmeticException("user not logged-in");
      }

      if (boards.ContainsKey(mail) && boards[mail].ContainsKey(boardName)) //check exists
      {
            Log.Warn($"Board creation failed because the user already has a board with this name");
            throw new ArgumentException("board already exist to that user");
      }
      
      Board newBoard = new Board(uf.Users[mail],boardName, boardIDCounter);
      //If a user event exists in the dictionary, but the board is not.
      if (boards.ContainsKey(mail) && !boards[mail].ContainsKey(boardName)){
          
         boards[mail].Add(boardName, newBoard);
         uf.Users[mail].AddBoard(newBoard);
         BoardUserDTO buDto = new BoardUserDTO(mail,boardIDCounter, 1);
         
         boardIDCounter++;
         
         BoardDTO bDto = new BoardDTO(newBoard.BoardId, newBoard.Name,newBoard.Ownership, newBoard.TaskLimitations[0],newBoard.TaskLimitations[1],newBoard.TaskLimitations[2],newBoard.CreationDate.ToString("yyyy-MM-dd HH:mm:ss"));
         if (!bDto.Insert())
         {
             Log.Warn("couldn't insert board into db");
             throw new ArgumentException("user not inserted to db");
         }

         
         if (!buDto.insertboarduser())
         {
             Log.Warn("couldn't insert user2board relation into db");
             throw new ArgumentException("user not inserted to db");
         }
         
         return newBoard;
      }

      //if a user event doesnt exist in the dictionary (case this is the first board a user owns).
      boards.Add(mail, new Dictionary<string, Board>());
      boards[mail].Add(boardName, newBoard);
      uf.Users[mail].AddBoard(newBoard);
      BoardUserDTO buDto2 = new BoardUserDTO(mail,boardIDCounter, 1);
      boardIDCounter++;
      
      BoardDTO boardDto = new BoardDTO(newBoard.BoardId, newBoard.Name,newBoard.Ownership, newBoard.TaskLimitations[0],newBoard.TaskLimitations[1],newBoard.TaskLimitations[2],newBoard.CreationDate.ToString("yyyy-MM-dd HH:mm:ss"));
      if (!boardDto.Insert())
      {
          Log.Warn("couldn't insert board into db");
          throw new ArgumentException("user not inserted to db");
      }
      
      if (!buDto2.insertboarduser())
      {
          Log.Warn("couldn't insert user2board relation into db");
          throw new ArgumentException("user not inserted to db");
      }
      
      return newBoard;
   }

   //intput - email, board name, task ordinal, task limit
   //output- void*. Limits the given task column.
   public void LimitTask(string mail, string boardName, int taskOrdinal, int taskLimitation )
   {

        if (!uf.Users.ContainsKey(mail))
        {
            Log.Warn("task limitation failed because the user does not exist");
            throw new ArgumentException("No user with that ID");
        }
        if (!uf.Users[mail].isLoggedIn())
        {
            Log.Warn("task limitation failed because the user is not logged in");
            throw new ArgumentException("user not logged in");
        }
        if (!boards.ContainsKey(mail))
        {
            Log.Warn("task limitation failed because the user does not have any boards");
            throw new ArgumentException("User has no boards");
        }
        if (boards.ContainsKey(mail) && !boards[mail].ContainsKey(boardName))
         {
            Log.Warn("task limitation failed because the board does not exist");
            throw new ArgumentException("Board not exist");
         }
      
      boards[mail][boardName].LimitTasks(taskLimitation,taskOrdinal);
      _boardcontroller.updateLimit(boards[mail][boardName].BoardId, taskOrdinal, taskLimitation);
   }

   //intput - email, board name
   //output- void*. Deletes given board.
   public void DeleteBoard(string mail, string boardName)
   {
        if (!uf.Users.ContainsKey(mail))
        {
            Log.Warn($"the system tried to delete a board of a user but the user does not exist");
            throw new ArgumentException("No user with that ID");
        }
        if (!uf.Users[mail].isLoggedIn())
        {
            Log.Warn($"the system tried to delete a board of a user but the user is not logged in");
            throw new ArgumentException("user not looged in");
        }

        if (!boards.ContainsKey(mail))
        {
            Log.Warn($"the system tried to delete a board but the user doesnt own a board");
            throw new ArgumentException("User doesn't owns boards");
        }
        if (!boards[mail].ContainsKey(boardName))
        {
            Log.Warn($"the system tried to delete a board but the board wasn't found under the user");
            throw new ArgumentException("User is not the owner of the board or the board doesn't exist");
        }

        Board boardToRemove = boards[mail][boardName];
        //removes the board from all the members board's list
        LinkedListNode<User> member = boards[mail][boardName].Members.First;
        while (member != null) 
        {
            member.Value.RemoveBoard(boardToRemove.BoardId);
            _boardUsercontroller.deleteBoardUser(member.Value.Email, boardToRemove.BoardId);
            member = member.Next;
        }
        
        //removes the board from the owner-boards dictionary
        boards[mail].Remove(boardName);
        _boardcontroller.DeleteBoard(boardToRemove.BoardId);

   }

   public string GetBoardName(int boardID)
   {
       foreach (var ownerBoards in boards.Values)
       {
           foreach (var board in ownerBoards.Values)
           {
               if (board.BoardId == boardID)
                   return board.Name;
           }
       }

       return null;
   }
   

   public void ChnageOwnership(string currentOwnerMail, string newOwnerMail, string boardName)
   {
       if (!uf.Users.ContainsKey(currentOwnerMail) || !uf.Users.ContainsKey(newOwnerMail))
       {
           if (!uf.Users.ContainsKey(currentOwnerMail))
           {
               Log.Warn("the system tried to change ownership in a board but the owner wasn't found");
               throw new ArgumentException("no user");
           }
           Log.Warn("the system tried to change ownership in a board to a member to a board but that member wasn't found");
           throw new ArgumentException("no user");
       }
       if (!isLoggeding(currentOwnerMail))
       {
           Log.Warn("the system tried to change ownership in a board but the owner isn't logged in");
           throw new ArgumentException("user not logged in");
       }

       if (!boards.ContainsKey(currentOwnerMail))
       {
           Log.Warn("the system tried to change ownership in a board but the owner doesn't have boards");
           throw new ArgumentException("user not logged in");
       }

       if (!boards[currentOwnerMail].ContainsKey(boardName))
       {
           Log.Warn("the system tried to change ownership in a board but the owner doesn't have a board in that name");
           throw new ArgumentException("board wasn't found");
       }

       if (boards[currentOwnerMail][boardName].FindUser(newOwnerMail) == null)
       {
           Log.Warn("the system tried to change ownership in a board but the user isn't a member");
           throw new ArgumentException("not a member");
       }

       boards[currentOwnerMail][boardName].Ownership = newOwnerMail;
       Board temp = boards[currentOwnerMail][boardName];
       boards[currentOwnerMail].Remove(temp.Name);
       
       //Case member has no boards until now.
       if (!boards.ContainsKey(newOwnerMail))
       {
           boards.Add(newOwnerMail, new Dictionary<string, Board>());
           boards[newOwnerMail].Add(temp.Name,temp);
           return;
       }
       
       boards[newOwnerMail].Add(temp.Name,temp);
   }

   public void AssignTask(string activeEmail, string boardName,int columOrdinal, int taskId, string assignEmail)
   {
        if (!uf.Users.ContainsKey(activeEmail))//check if user exist
        {
            Log.Warn("try to assign task but user not exist");
            throw new ArgumentException("user not exist");  
        }
        if (!this.isLoggeding(activeEmail))//check if user login
        {
            Log.Warn("try to assign task but user not loggedin");
            throw new ArgumentException("user not log in");
            
        }
        if (!uf.Users.ContainsKey(assignEmail))//chekc if assigni exist
        {
            Log.Warn("try to assign task but assigni not exist");
            throw new ArgumentException("assigni not exist");
        }

        Board boardtoassign = uf.Users[activeEmail].getBoard(boardName);//check if activemail member in the board
        if (boardtoassign == null)
        {
            throw new ArgumentException("activemail not member in this board or board doesn't exist");
        }
        
        User assigni = uf.Users[assignEmail];
        if (uf.Users[assignEmail].getBoard(boardName) == null)
        {
            Log.Warn("try to assign task but assigni is not a member");
            throw new ArgumentException("assignee is not a member");
        }

       User assign = uf.Users[activeEmail];
       boardtoassign.AssignTask(assign, columOrdinal, taskId, assigni);//update ing task.
        Log.Info("assign task secssufly in boards");
        
       








    }
   
   public void AddMemberToBoard(string emailToAdd, int boardId)
   {
       if (!uf.Users.ContainsKey(emailToAdd))
       {
           Log.Warn("the system tried to add a member to a board but the user does not exist");
           throw new ArgumentException("no user");
       }
       if (!this.isLoggeding(emailToAdd))
       {
           Log.Warn("the system tried to add a member to a board but the user is not logged in");
           throw new ArgumentException("user not logged in");
       }
       
       Board board = FindBoard(boardId);
       if (board == null)
       {
           Log.Warn("the system tried to add a member to a board but the board wasnt found");
           throw new ArgumentException("No board with that ID");
       }
       if (board.FindUser(emailToAdd) != null)
       {
           Log.Warn("the system tried to add a member to a board but the user is already a member of the board");
           throw new ArgumentException("Already a member");
       }
       
       uf.Users[emailToAdd].AddBoard(board);
       board.AddMember(uf.Users[emailToAdd]);

       BoardUserDTO b2userDTO = new BoardUserDTO(emailToAdd, board.BoardId, 0);
       b2userDTO.insertboarduser();
   }

   public Board FindBoard(int boardId)
   {
       foreach (var ownerBoards in boards.Values)
       {
           foreach (var board in ownerBoards.Values)
           {
               if (board.BoardId == boardId)
                   return board;
           }
       }

       return null;
   }
   
   public void RemoveMemberFromBoard(string emailToRemove, int boardID)
   {
       if (!uf.Users.ContainsKey(emailToRemove))
       {
           Log.Warn("the system tried to remove a member from a board but the user does not exist");
           throw new ArgumentException("no user");
       }
       if (!this.isLoggeding(emailToRemove))
       {
           Log.Warn("the system tried to remove a member from a board but the user is not logged in");
           throw new ArgumentException("user not logged in");
       }
       
       Board board = FindBoard(boardID);
       if (board == null)
       {
           Log.Warn("the system tried to remove a member from a board but the board wasnt found");
           throw new ArgumentException("No board with that ID");
       }
       if (board.FindUser(emailToRemove) == null)
       {
           Log.Warn("the system tried to remove a member from a board but the user is not a member of the board");
           throw new ArgumentException("Not a member");
       }

       if (board.Ownership == emailToRemove)
       {
           Log.Warn("the system tried to remove a member from a board but it found that its the owner, please transfer ownership to exit the board");
           throw new ArgumentException("Removing the owner");
       }
       
       board.RemoveMember(emailToRemove);
       uf.Users[emailToRemove].RemoveBoard(board.BoardId);
       _boardUsercontroller.deleteBoardUser(emailToRemove, board.BoardId);
   }
   
   
   //intput - email, 
   //output - dictionary of boards with their names as the keys/
   public Dictionary<string,Board> GetBoards(string mail)
   {
        if (!uf.Users[mail].isLoggedIn())
        {
            Log.Warn("the system tried to get boards information but the user is not logged in");
            throw new ArgumentException("user not logged in");
        }
        if (!boards.ContainsKey(mail))
        {
            Log.Warn($"the system tried to get boards information but the user does not exist int the system");
            throw new ArgumentException("user not exist");
        }

        return boards[mail];
      
   }


   //intput - email, board name, task ordinal
   //output- List of the given board and the given task ordinal tasks.
   public LinkedList<Task> GetTasks(string mail, string boardName, int taskOrdinal)
   {
      if (!boards.ContainsKey(mail) || !boards[mail].ContainsKey(boardName))
      {
            Log.Warn("the system tried to get tasks infromation of a board but the user does not exist");
            throw new Exception("user not exist");
      }

      if (!this.isLoggeding(mail))
      {
            Log.Warn("the system tried to get tasks infromation of a board but the user is not logged in");
            throw new ArgumentException("user not logged in");
      }

      if (!boards[mail].ContainsKey(boardName))
      {
            Log.Warn("the system tried to get tasks infromation of a board but the board does not exist");
            throw new ArgumentException("board not exist");
      }
      else
      {
         return boards[mail][boardName].GetTasks(taskOrdinal);
      }
   }

   //input - email, board name, due date, title, description
   //output - void*. Adds the given task to the board.
   public void AddTask(string mail, string boardName, DateTime dueDate, string title,string description)
   {
        if (!boards.ContainsKey(mail) || !boards[mail].ContainsKey(boardName))
        {
            Log.Warn($"the system tried to add a task  but the user does not have the reffered board");
            throw new Exception("board or users do not exist do not exist");
        }

        if (!this.isLoggeding(mail))
        {
            Log.Warn($"the system tried to add a task but the user is not logged in");
            throw new Exception("User is not logged in");
        }
        if (title == null || title.Length > 50)
        {
            Log.Warn($"the system tried to add a task  but the title was invalid");
            throw new Exception("Title input null");
        }

        if (description != null && description.Length > 300)
        {
            Log.Warn($"the system tried to add a task but the description did not meet the requirements");
            throw new Exception("Improper description input");
        }

        if (dueDate < DateTime.Now)
        {
            Log.Warn($"the system tried to add a task but the dueDate did not meet the requirements");
            throw new Exception("Improper input");
        }
        if (boards[mail][boardName].FindUser(mail) == null)
       {
            Log.Warn("the system tried to add a task to a board but the user was not a member of the board");
            throw new ArgumentException("Not a member");
        }

        if (boards[mail][boardName].Containtask(title))
        {
            Log.Warn("the system tried to add a task but its name already exist in the board");
            throw new ArgumentException("Same title");
        }

        boards[mail][boardName].AddTask(title,description,dueDate);
        
        
      
   }

   //intput - email, board name, task id, task ordinal
   //output- void*. Advancing the given task.
   public void ForwardTask(string mail, string boardName, int taskId,int taskOrdinal)
   {
        if (!uf.Users.ContainsKey(mail))
        {
            Log.Warn($"the system tried to forward a task of user but the user does no exist in the system");
            throw new ArgumentException("No user with that ID");
        }
        if (!uf.Users[mail].isLoggedIn())
        {
            Log.Warn($"the system tried to forward a task of user but the user is not logged in");
            throw new ArgumentException("user not looged in");
        }
        if (!boards.ContainsKey(mail))
        {
            Log.Warn($"the system tried to forward a task of user but the user does not have any boards");
            throw new ArgumentException("User has no boards");
        }
        

        else
        {
            User u = uf.Users[mail];
            Board b = boards[mail][boardName];
            Task t = null;
            foreach (var task in b.Tasks[taskOrdinal])
            {
                if (task.Id == taskId)
                {
                    t = task;
                }
            }

            if (t == null)
            {
                Log.Warn($"the system tried to forward a task of user but the task does not exist");
                throw new ArgumentException("task not exist");
            }
            if (!t.Assign.Email.Equals(mail))
            {
                Log.Warn($"the system tried to forward a task of user but the user is not assigned to this task");
                throw new ArgumentException("the user is not assigned to this task");
            }
            boards[mail][boardName].ForwardTask(mail,taskOrdinal,taskId);
      }
      
   }

   //intput - email, board name, task details
   //output- void* Updates the given tasks by the new attributes.
   public void UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
   {
        if (!uf.Users.ContainsKey(email))
        {
            Log.Warn($"the system tried to update a user's task title but he does not exist in the system");
            throw new ArgumentException("No user with that ID");
        }
        if (!uf.Users[email].isLoggedIn())
        {
            Log.Warn($"the system tried to update a task's title of a user but he is not logged in");
            throw new ArgumentException("user not logged in");
        }
        if (!boards.ContainsKey(email))
        {
            Log.Warn($"the system tried to update a task's title of a user but he does not have ny boards");
            throw new ArgumentException("User has no boards");
        }

        //check if the new title meets requirements.
        if (title.Length > 50 | title == null)
        {
            Log.Warn($"the system tried to update a task's title but it did not meet the requirements");
            throw new ArgumentException("Title input is improper");
        }
       
        LinkedList<Task> tasks = boards[email][boardName].Tasks[columnOrdinal];
      foreach (var task in tasks)
      {
            if (!task.Assign.Email.Equals(email)){
                Log.Warn($"the system tried to update a task title of a user but the user is not assigned to this task");
                throw new ArgumentException("the user is not assigned to this task");
            }
            if (task.Id == taskId)
         {
            task.UpdateTask(new DateTime(2999,1,1),title,null);
            return;
         }
      }
   }
   
   //intput - email, board name, task details
   //output- void* Updates the given tasks by the new attributes.
   public void UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
   {

        if (!uf.Users.ContainsKey(email))
        {
            Log.Warn($"the system tried to update a user's task DueDate but he does not exist in the system");
            throw new ArgumentException("No user with that ID");
        }
        if (!uf.Users[email].isLoggedIn())
        {
            Log.Warn($"the system tried to update a task's DueDate of a user but he is not logged in");
            throw new ArgumentException("user not logged in");
        }
        if (!boards.ContainsKey(email))
        {
            Log.Warn($"the system tried to update a task's DueDate of a user but he does not have any boards");
            throw new ArgumentException("User has no boards");
        }

        if (dueDate <= DateTime.Now)
        {
            Log.Warn($"the system tried to update a task's DueDate but it did not meet the requirements");
            throw new ArgumentException("due date has already passed");
        }
      Boolean exist = false;
      var ans = boards[email][boardName].Tasks[columnOrdinal].First;
      while (ans != null)
      {
         if (ans.Value.Id == taskId)
            exist = true;
         ans = ans.Next;
      }

      //checks if the task id wasn't found
      if (exist == false)
      {
            Log.Warn($"the system tried to update a task's DueDate but the task id did not found");
            throw new ArgumentException("task id didnt found");
      }
      LinkedList<Task> tasks = boards[email][boardName].Tasks[columnOrdinal];
      foreach (var task in tasks)
      {
            if (!task.Assign.Email.Equals(email))
            {
                Log.Warn($"the system tried to update a task title of a user but the user is not assigned to this task");
                throw new ArgumentException("the user is not assigned to this task");
            }
            if (task.Id == taskId)
         {
            task.UpdateTask(dueDate,null,null);
            return;
         }
      }
   }
   
   //intput - email, board name, task details
   //output- void* Updates the given tasks by the new attributes.
   public void UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
   {
        if (!uf.Users.ContainsKey(email))
        {
            Log.Warn($"the system tried to update a user's task description but he does not exist in the system");
            throw new ArgumentException("No user with that ID");
        }
        if (!uf.Users[email].isLoggedIn())
        {
            Log.Warn($"the system tried to update a task's description of a user but he is not logged in");
            throw new ArgumentException("user not logged in");
        }
        if (!boards.ContainsKey(email))
        {
            Log.Warn($"the system tried to update a task's description of a user but he does not have ny boards");
            throw new ArgumentException("User has no boards");
        }

        //checks if the description meets requirements.
        if (description.Length > 300)
        {
            Log.Warn($"the system tried to update a task's description but it did not meet the requirements");
            throw new ArgumentException("Description input is improper");
        }
      
      LinkedList<Task> tasks = boards[email][boardName].Tasks[columnOrdinal];
      foreach (var task in tasks)
      {
            if (!task.Assign.Email.Equals(email))
            {
                Log.Warn($"the system tried to update a task title of a user but the user is not assigned to this task");
                throw new ArgumentException("the user is not assigned to this task");
            }
            if (task.Id == taskId)
         {
            task.UpdateTask(new DateTime(2999,1,1),null,description);
            return;
         }
      }
   }

   //all data loading
   public void LoadData()
   {
       LoadBoardsData();
       LoadBoard2UserData();
       LoadTasks();
       
       

   }
   public void LoadBoardsData()
   {
       
       List<BoardDTO> real = _boardcontroller.LoadBoards();
       foreach (var VARIABLE in real)
       {
           Board temp = new Board(VARIABLE);
           if(boards.ContainsKey(temp.Ownership)) 
               boards[temp.Ownership].Add(temp.Name, temp);
           else
           {
               boards.Add(temp.Ownership, new Dictionary<string, Board>());
               boards[temp.Ownership].Add(temp.Name, temp);
           }
       }
   }
   
    public void LoadBoard2UserData()
   {
      
       List<BoardUserDTO> real = _boardUsercontroller.LoadBoard2User();
       foreach (var b2user in real)
       {
           FindBoard(b2user.Boardid1).Members.AddLast(uf.Users[b2user.Email1]);
           uf.Users[b2user.Email1].AddBoard(FindBoard(b2user.Boardid1));
       }
        
   }
    public void LoadTasks()
    {
        List<TaskDTO> real = _taskcontroller.LoadTasks();
        foreach (var VARIABLE in real)
        {
            DateTime duedate = DateTime.ParseExact(VARIABLE.DueDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            Task t = new Task(VARIABLE.Id, duedate, VARIABLE.Title, VARIABLE.Description);
            if(VARIABLE.AssignEmail != null)
                t.Assign = uf.Users[VARIABLE.AssignEmail];
            Boolean found = false;
            
            outerLoop:
            foreach (var ownerBoard in boards.Values)
            {
                foreach (var board in ownerBoard.Values)
                {
                    if (VARIABLE.Boardid == board.BoardId)
                    {
                        board.Tasks[VARIABLE.Colum].AddLast(t);
                        found = true;
                        break;
                    }

                }
                
                if(found)
                    break;
            }

        }
    }

    public void DeleteData()
    {
        _boardcontroller.DeleteData();
        _boardUsercontroller.DeleteData();
        _taskcontroller.DeleteData();
    }


    public Boardcontroller Boardcontroller
    {
        get => _boardcontroller;
        set => _boardcontroller = value;
    }

    public BoardUsercontroller BoardUsercontrollerS
    {
        get => _boardUsercontroller;
        set => _boardUsercontroller = value;
    }
}