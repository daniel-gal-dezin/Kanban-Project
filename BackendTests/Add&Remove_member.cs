using System.Text.Json;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace BackendTests;

public class Add_Remove_member
{
    private BoardService bs;

    public Add_Remove_member(BoardService b)
    {
        bs = b;
    }

    public void RunTests()
    {
        
        //this function will check the future of'add and remove' members to and from a board.
        //this tests are made for requirement number 12.
        /*
        Response res =JsonSerializer.Deserialize<Response>(bs.AddMemberToBoard("user1@gmail.com",1));
        if (res.ErrorMessage != null)
            Console.WriteLine("not legal arguments");
        else
        {
            Console.WriteLine("task was added successfully");
        }
        Response res2 = JsonSerializer.Deserialize<Response>(bs.AddMemberToBoard("user1@gmail.com", 2));
        if (res2.ErrorMessage != null)
            Console.WriteLine("not legal arguments");
        else
        {
            Console.WriteLine("task was added successfully");
        }*/
        Response res3 =JsonSerializer.Deserialize<Response>(bs.RemoveMemberFromBoard(1,"user1@gmail.com"));
        if (res3.ErrorMessage != null)
            Console.WriteLine("not legal arguments");
        else
        {
            Console.WriteLine("task was added successfully");
        }
        Response res4 = JsonSerializer.Deserialize<Response>(bs.RemoveMemberFromBoard(2, "user4@gmail.com"));
        if (res4.ErrorMessage != null)
            Console.WriteLine("not legal arguments");
        else
        {
            Console.WriteLine("task was added successfully");
        }
        
    }
    
    
}