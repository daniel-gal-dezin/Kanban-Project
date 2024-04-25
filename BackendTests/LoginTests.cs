using System.Text.Json;
using IntroSE.Kanban.Backend.Backend;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace BackendTests;

public class LoginTests
{
    private UserService us;

    /// this test will check the login feature by authenticate email and password.
    /// This function tests Requirement 8
    public LoginTests(UserService userService)
    {
        us = userService;
    }

    public void RunTests()
    {
        Response res = JsonSerializer.Deserialize<Response>(us.Login("user4@gmail.com", "efS2fsdf@@$$!"));
        if(res.ErrorMessage != null)
            Console.WriteLine("not legal arguments");
        else
            Console.WriteLine("youre been logged in");
        
        

        Response res1 = JsonSerializer.Deserialize<Response>(us.Login("user3@gmail.com", "Gs8w73"));
        if(res1.ErrorMessage != null)
            Console.WriteLine("not legal arguments");
        else
        {
            Console.WriteLine("youre been logged in");
        }

        
        
        Response res2 = JsonSerializer.Deserialize<Response>(us.Login("user2@gmail.com", "Hgs32"));
        if(res2.ErrorMessage != null)
            Console.WriteLine("not legal arguments");
        else
        {
            Console.WriteLine("youre been logged in");
        }


        Response res3 = JsonSerializer.Deserialize<Response>(us.Login("user2@gmail.com", "Gs8w73"));
        if(res3.ErrorMessage != null)
            Console.WriteLine("not legal arguments");
        else
        {
            Console.WriteLine("youre been logged in");
        }
        
        Response res4 = JsonSerializer.Deserialize<Response>(us.Login("user1@gmail.com", "D1234a5"));
        if(res4.ErrorMessage != null)
            Console.WriteLine("not legal arguments");
        else
        {
            Console.WriteLine("youre been logged in");
        }






    }
}