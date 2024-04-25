using System;
using IntroSE.Kanban.Backend.Backend;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text.Json;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BuisnessLayer;
using Task = IntroSE.Kanban.Backend.BuisnessLayer.Task;

namespace IntroSE.Kanban.Backend.ServiceLayer;

public class UserService
{
    private UserFacade uf;
    
    public UserService(UserFacade uFac)
    {
        this.uf = uFac;
    }
    
    //Input - email, password
    //Output - Json seriliazed response with the user email if the registeration worked, or with an error if it didn't
    public string Register(string email, string password)
    {
        try
        {
            User user = uf.register(email,password);
            Response res = new Response(user.Email, null);
            return JsonSerializer.Serialize(res);
        }
        catch(Exception e) 
        {
            return JsonSerializer.Serialize(new Response(null, e.Message));
        }
       
    }


    public String LoadData()
    {
        try
        {
            uf.LoadData();
            Response res = new Response(null, null);
            return JsonSerializer.Serialize(res);
        }
        catch
        {
            return JsonSerializer.Serialize(new Response(null, "Argument Exception - not load data"));
        }
    }
    
    
    public String DeleteData()
    {
        try
        {
            uf.DeleteData();
            Response res = new Response(null, null);
            return JsonSerializer.Serialize(res);
        }
        catch
        {
            return JsonSerializer.Serialize(new Response(null, "Argument Exception - not load data"));
        }
    }
    
    
    
    
    
    
    

    //Input - email and password
    //Output - Json serliazed response with the user email if the login worked, or with an error if it didn't
    public string Login(string email, string password)
    {
        try
        {
            User user = uf.logIn(email, password);
            Response res = new Response(user.Email, null);
            return JsonSerializer.Serialize(res);
        }
        catch(Exception e)
        {
            return JsonSerializer.Serialize(new Response(null, e.Message));
        }
    }

    public string GetUserBoards(string email)
    {
        try
        {
            LinkedList<int> boardID = uf.GetBoardsId(email);
            
            
            Response res = new Response(boardID, null);
            
            return JsonSerializer.Serialize(res);
        }
        catch
        {
            return JsonSerializer.Serialize(new Response(null, "Argument Exception - GetBoards collapsed"));
        }
    }
    
    //Input - email
    //Output - Json serliazed response with task collection if the function worked, or with an error if it didn't
    public string GetAllInProgTasks(string actorEmail)
    {
        try
        {
            LinkedList<Task> inProgTasks = uf.GetAllInProgTasks(actorEmail);
            List<TaskToSend> columnToSend = new List<TaskToSend>(); //Creates a copy with 'taskToSend' Object instead of Task.
            foreach (var task in inProgTasks)
            {
                columnToSend.Add(new TaskToSend(task));
            }
            
            return JsonSerializer.Serialize(new Response(columnToSend, null));
        }
        catch
        {
            return JsonSerializer.Serialize(new Response(null, "Argument Exception - GetAllInProgTasks collapsed"));
        }
    }

    //Input - email
    //Output - Json serliazed empty respone, or a response with an error if it didn't
    public string LogOut(string actorEmail)
    {
        try
        {
            uf.logout(actorEmail);
            return JsonSerializer.Serialize(new Response(null, null));
        }
        catch
        {
            return JsonSerializer.Serialize(new Response(null, "Argument Exception - Logout collapsed"));
        }
    }
}