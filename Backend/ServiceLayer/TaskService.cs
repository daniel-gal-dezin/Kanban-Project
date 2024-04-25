using System.Collections.Generic;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Task = IntroSE.Kanban.Backend.BuisnessLayer.Task;

namespace IntroSE.Kanban.Backend.ServiceLayer;

public class TaskService
{
    private BoardFacade bf;
    

    public TaskService(BoardFacade bFac)
    {
        this.bf = bFac;
    }

    public String AssignTask(string activeEmail, string boardName,int columOrdinal, int taskID, string assignEmail)
    {
        try
        {
            bf.AssignTask(activeEmail,boardName,columOrdinal,taskID,assignEmail);
            Response res = new Response("succses", null);
            return JsonSerializer.Serialize(res);
        }
        catch 
        {
            return JsonSerializer.Serialize(new Response(null, "Argument Exception - GetTasks collapsed"));
        }
    }
    
    //Input - email, board name, task details
    //Output - Json serliazed response with "success" as a value if the function worked, or with an error if it didn't work
    public String AddTask(string actorEmail, string boardName, string title, string desc, DateTime dueTime)
    {
        try
        {
            bf.AddTask(actorEmail,boardName,dueTime,title,desc);
            Response res = new Response("succses", null);
            return JsonSerializer.Serialize(res);
        }
        catch
        {
            return JsonSerializer.Serialize(new Response(null, "Argument Exception - GetTasks collapsed"));
        }

    }
       
    //Input - email, boardname, columnOrdinal
    //Output - Json serliazed respone with the column inside a task collection, or with an error if it didn't work
    public string GetTasks(string activeEmail, string boardName, int taskCord)
    {
        try
        {
            LinkedList<Task> tasks = bf.GetTasks(activeEmail, boardName, taskCord);
            Response res = new Response(JsonSerializer.Serialize(tasks), null);
            return JsonSerializer.Serialize(res);
        }
        catch
        {
            return JsonSerializer.Serialize(new Response(null, "Argument Exception - GetTasks collapsed"));
        }

        
    }
    
}