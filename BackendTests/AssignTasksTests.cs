using System.Text.Json;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace BackendTests;

public class AssignTasksTests
{
    private TaskService ts;

    public AssignTasksTests(TaskService t)
    {
        ts = t;
    }

    /// this test will check the feuture of assigning a user to a taskl
    /// these tests check requirement number 23.
    public void RunTests()
    {
        
        Response res = JsonSerializer.Deserialize<Response>(ts.AssignTask("user3@gmail.com", "test board",0,1,"user3@gmail.com"));
        if (res.ErrorMessage != null)
            Console.WriteLine("not legal arguments");
        else
        {
            Console.WriteLine("the task got assign sucssufly ");
        }

        Response res1 = JsonSerializer.Deserialize<Response>(ts.AssignTask("ShaiManor@gmail.com", "test board",2,1,"non-exsitent-user"));
        if (res1.ErrorMessage != null)
            Console.WriteLine("not legal arguments");
        else
        {
            Console.WriteLine("the task got assign sucssufly");
        }
    }
}