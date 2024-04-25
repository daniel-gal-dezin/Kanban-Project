using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BuisnessLayer;
using Task = IntroSE.Kanban.Backend.BuisnessLayer.Task;

namespace IntroSE.Kanban.Backend.ServiceLayer;

public class BoardService
{
    private BoardFacade bf;
    

    public BoardService(BoardFacade bFac)
    {
        this.bf = bFac;
    }

    //Input - email, board name, coulmn ordinal, task id, new title
    //Output - Json serliazed response with the board as a value if the update occured properly
    //or with an error if it didn't work
    public string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
    {
        try
        {
            bf.UpdateTaskTitle(email,boardName,columnOrdinal,taskId,title);
            return JsonSerializer.Serialize(new Response("success", null));
        }
        catch 
        {
            return JsonSerializer.Serialize(new Response(null, "Update Task collapsed"));
        }
    }
    
    //Input - email, board name, column ordinal, task id, new description
    //Output - Json serliazed response with the "success" as a value if the update occured properly
    //or with an error if it didn't work
    public string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
    {
        try
        {
            bf.UpdateTaskDescription(email,boardName,columnOrdinal,taskId,description);
            return JsonSerializer.Serialize(new Response("success", null));
        }
        catch 
        {
            return JsonSerializer.Serialize(new Response(null, "Update Task collapsed"));
        }
    }
    
    public string RemoveMemberFromBoard(int boardID, string memberEmail)
    {
        try
        {
            bf.RemoveMemberFromBoard(memberEmail,boardID);
            return JsonSerializer.Serialize(new Response("success", null));
        }
        catch 
        {
            return JsonSerializer.Serialize(new Response(null, "Removing member from board collapsed"));
        }
    }
    
    public string AddMemberToBoard(string memberEmail, int boardID)
    {
        try
        {
            bf.AddMemberToBoard(memberEmail,boardID);
            return JsonSerializer.Serialize(new Response("success", null));
        }
        catch 
        {
            return JsonSerializer.Serialize(new Response(null, "Adding member to board collapsed"));
        }
    }
    
    public string ChangeOwnership(string oldOwnerMail, string newOwnerMail, string boardName)
    {
        try
        {
            bf.ChnageOwnership(oldOwnerMail,newOwnerMail,boardName);
            return JsonSerializer.Serialize(new Response("success", null));
        }
        catch 
        {
            return JsonSerializer.Serialize(new Response(null, "Update Task collapsed"));
        }
    }
    
    public string GetBoardName(int boardID)
    {
        try
        {
            string name = bf.GetBoardName(boardID);
            return JsonSerializer.Serialize(new Response(name, null));
        }
        catch 
        {
            return JsonSerializer.Serialize(new Response(null, "Update Task collapsed"));
        }
    }
    
    //Input - email, board name, coulmn ordinal, task id, new due date
    //Output - Json serliazed response with the board as a value if the update occured properly
    //or with an error if it didn't work
    public string UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
    {
        try
        {
            bf.UpdateTaskDueDate(email,boardName,columnOrdinal,taskId,dueDate);
            return JsonSerializer.Serialize(new Response("success", null));
        }
        catch 
        {
            return JsonSerializer.Serialize(new Response(null, "Update Task collapsed"));
        }
    }
    
    //Input - email, board name, column ordinal
    //Output - Json serliazed response with the task collection as a value if the creation occured properly
    //or with an error if it didn't work
    public string GetTasks(string activeEmail, string boardName, int taskCord)
    {
        try
        {
            LinkedList<Task> tasks = bf.GetTasks(activeEmail, boardName, taskCord);
            
            // Here we copy the list to a new collection, this time the Object is 'TaskToSend' 
            // and not 'Task', we did that in order to send the Task in the exact json format required in 'grading service'
            List<TaskToSend> columnToSend = new List<TaskToSend>(); 
            foreach (var task in tasks)
            {
                columnToSend.Add(new TaskToSend(task));
            }
            Response res = new Response(columnToSend, null);
            
            return JsonSerializer.Serialize(res);
        }
        catch
        {
            return JsonSerializer.Serialize(new Response(null, "Argument Exception - GetTasks collapsed"));
        }

        
    }

    //Input - email of the owner, board name
    //Output - Json serliazed response with the board as a value if the creation occured properly
    //or with an error if it didn't work
    public string CreateBoard(string activeEmail, string boardName)
    {
        try
        {
            Board board = bf.CreateBoard(activeEmail, boardName);
            Response res = new Response(JsonSerializer.Serialize(new BoardToSend(activeEmail,board.Name)), null);
            return JsonSerializer.Serialize(res);
        }
        catch
        {
            return JsonSerializer.Serialize(new Response(null, "Argument Exception - createBoard collapsed"));
        }
    }

    //Input - email, board name, task id, column ordinal
    //Output - Json serliazed response with "success" as a value if the advancing occured properly
    //or with an error if it didn't work
    public string ForwardTask(string activeEmail, string boardName, int taskID,int taskordinal)
    {
        try
        {
            bf.ForwardTask(activeEmail, boardName,taskID,taskordinal);
            return JsonSerializer.Serialize(new Response("succsess", null));
        }
        catch
        {
            return JsonSerializer.Serialize(new Response(null, "Argument Exception - ForwardTask collapsed"));
        }
    }
    
    //Input - email, board name
    //Output - Json serliazed response with "success" as a value if the deletion occured properly
    //or with an error if it didn't work
    public string DeleteBoard(string activeEmail, string boardName)
    {
        try
        {
            bf.DeleteBoard(activeEmail, boardName);
            return JsonSerializer.Serialize(new Response("sucsses",null));
        }
        catch
        {
            return JsonSerializer.Serialize(new Response(null, "Argument Exception - deleteBoard collapsed"));
        }
    }

    // 0 - BacklogTasks, 1 - inProgTasks, 2 - Done tasks
    //Input - email, board name, column ordinal, the limit to insert
    //Output - Json serliazed response with "success" as a value if the the limiation occured properly
    //or with an error if it didn't work
    public string LimitTasks(string actorEmail, string boardName, int taskOrdinal, int limit)
    {
        try
        {
            bf.LimitTask(actorEmail, boardName,taskOrdinal,  limit);
            return JsonSerializer.Serialize(new Response("success",null));
        }
        catch
        {
            return JsonSerializer.Serialize(new Response(null, "Argument Exception - LimitTask collapsed"));
        }

    }

    //Input - email, board name, column ordinal
    //Output - Json serliazed response with the the task limitation in the board and the column asked (if there is one)
    //or with an error if the function didnt worked
    public string GetColumnLimits(string actorEmail, string boardName, int whichColumn)
    {
        try
        {
            Dictionary<string, Board> board = bf.GetBoards(actorEmail);
            int limit = board[boardName].getTaskLimit()[whichColumn];
            Response res = new Response((Object)limit, null);
            return JsonSerializer.Serialize(res);
        }
        catch
        {
            return JsonSerializer.Serialize(new Response(null, "Argument Exception - getcolumnLimits collapsed"));
        }
    }

    
    //Input - email, board name, column ordinal
    //Output - Json serliazed response with the column name in its value, error if an error occured.
    public string GetColumnName(string email, string boardName, int columnOrdinal)
    {
        try
        {
            if (!bf.isLoggeding(email))
            {
                throw new ArgumentException("user is not loggged-in");
            }

            if (columnOrdinal > 2 || columnOrdinal < 0)
            {
                throw new ArgumentException("column ordinal is not in range");
            }

            if (columnOrdinal == 0)
            {
                Response res = new Response("backlog", null);
                return JsonSerializer.Serialize(res);
            }
            else if (columnOrdinal == 1)
            {
                Response res = new Response("in progress", null);
                return JsonSerializer.Serialize(res);
            }
            else
            {
                Response res = new Response("done", null);
                return JsonSerializer.Serialize(res);
            }
        }

        catch
        {
            return JsonSerializer.Serialize(new Response(null, "Error occured"));
        }

    }
    
    


}