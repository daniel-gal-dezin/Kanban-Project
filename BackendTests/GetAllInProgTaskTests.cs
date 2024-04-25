using System.Text.Json;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace BackendTests;

public class GetAllInProgTaskTests
{
    private UserService us;

    public GetAllInProgTaskTests(UserService u)
    {
        us = u;
    }
    /// this test will check the feature of get all in progress tasks by user ask.
    /// This function tests Requirement 17
    
    
    public void RunTests()
    {
        Response res = JsonSerializer.Deserialize<Response>(us.GetAllInProgTasks("user2@gmail.com"));
        if(res.ErrorMessage != null)
            Console.WriteLine("not legal arguments");
        else
            Console.WriteLine("got all progTasks");
        
        
        Response res1 = JsonSerializer.Deserialize<Response>(us.GetAllInProgTasks(null));
        if(res1.ErrorMessage != null)
            Console.WriteLine("not legal arguments");
        else
            Console.WriteLine("got all progTasks");


    }
    
    
    
    
    
    
    
    
    
    
    
    
}