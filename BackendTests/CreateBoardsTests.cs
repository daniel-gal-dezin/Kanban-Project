using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;


namespace BackendTests
{
    internal class CreateBoardsTests
    {
        private BoardService bs;

        public CreateBoardsTests(BoardService b)
        {
            bs = b;
        }

        /// this test will check the feature of Create a new Board by user.
        /// This function tests Requirement 9
        public void RunTests()
        {
            Response res = JsonSerializer.Deserialize<Response>(bs.CreateBoard("user2@gmail.com", "test board u2"));
            if (res.ErrorMessage != null)
                Console.WriteLine("not legal arguments");
            else
            {
                Console.WriteLine("Board was created successfully");
            }

            Response res1 =JsonSerializer.Deserialize<Response> (bs.CreateBoard("user2@gmail.com", "test board 2 u2"));
            if (res1.ErrorMessage != null)
                Console.WriteLine("not legal arguments");
            else
            {
                Console.WriteLine("Board was cerated successfully");
            }

            Response res2 =JsonSerializer.Deserialize<Response>(bs.CreateBoard("user3@gmail.com", "test board"));
            if (res2.ErrorMessage != null)
                Console.WriteLine("not legal arguments");
            else
            {
                Console.WriteLine("Board was cerated successfully");
            }

            Response res3 = JsonSerializer.Deserialize<Response>(bs.CreateBoard("user1@gmail.com", "Menshe's test Board"));
            if (res3.ErrorMessage != null)
                Console.WriteLine("not legal arguments");
            else
            {
                Console.WriteLine("Board was cerated successfully");
            }
            
            Response res4 = JsonSerializer.Deserialize<Response>(bs.CreateBoard("user3@gmail.com","test board 2" ));
            if (res4.ErrorMessage != null)
                Console.WriteLine("not legal arguments");
            else
            {
                Console.WriteLine("Board was cerated successfully");
            }
        }
        
    }
}
