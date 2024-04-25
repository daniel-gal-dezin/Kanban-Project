using System.Text.Json;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace BackendTests;

internal class OwnershipTests
{
    private BoardService bs;

    public OwnershipTests(BoardService B)
    {
        bs = B;
    }

    /// this test will check the feutue of changeing the ownership of a board.
    /// These tests will check if requirement number 13.
    public void RunTests()
    {
        
        Response res = JsonSerializer.Deserialize<Response>(bs.ChangeOwnership("user2@gmail.com", "user3@gmail.com", "test board u2"));
        if (res.ErrorMessage != null)
            Console.WriteLine("not legal arguments");
        else
        {
            Console.WriteLine("changed ownership successfully");
        }

        Response res1 = JsonSerializer.Deserialize<Response>(bs.ChangeOwnership("shaiManorgmail.com","not-part-of-the-board" ,"test board"));
        if (res1.ErrorMessage != null)
            Console.WriteLine("not legal arguments");
        else
        {
            Console.WriteLine("changed ownership successfully");
        }
    }
}