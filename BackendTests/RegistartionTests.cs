using System.Text.Json;
using IntroSE.Kanban.Backend.ServiceLayer;


namespace BackendTests;

public class RegistartionTests
{
    private UserService us;

    /// this test will check the registration feature by authenticate email and password.
    /// This function tests Requirement 7
    public RegistartionTests(UserService uService)
    {
        us = uService;
    }
    
    public void RunTests()
    {
        Response res = JsonSerializer.Deserialize<Response>(us.Register("user1@gmail.com", "D1234a5"));
        if(res.ErrorMessage != null)
            Console.WriteLine("not legal arguments");
        else
        {
            Console.WriteLine("register successfully");
        }

        
        
        Response res1 = JsonSerializer.Deserialize<Response>(us.Register("user2@gmail.com", "Gs8w73"));
        if(res1.ErrorMessage != null)
            Console.WriteLine("not legal arguments");
        else
        {
            Console.WriteLine("register successfully");
        }


        Response res2 = JsonSerializer.Deserialize<Response>(us.Register("user3@gmail.com", "Gs8w73"));
        if(res2.ErrorMessage != null)
            Console.WriteLine("not legal arguments");
        else
        {
            Console.WriteLine("register successfully");
        }


        Response res3 = JsonSerializer.Deserialize<Response>(us.Register(null, "Gs8w73"));
        if(res3.ErrorMessage != null)
            Console.WriteLine("not legal arguments");
        else
        {
            Console.WriteLine("register successfully");
        }

        Response res4 = JsonSerializer.Deserialize<Response>(us.Register("user4@gmail.com", "efS2fsdf@@$$!"));
        if(res4.ErrorMessage != null)
            Console.WriteLine("not legal arguments");
        else
        {
            Console.WriteLine("register successfully");
        }



    }
}