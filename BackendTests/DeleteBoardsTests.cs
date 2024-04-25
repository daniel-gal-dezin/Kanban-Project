using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;
namespace BackendTests
{
    public class DeleteBoardsTests
    {
        private BoardService bs;

        public DeleteBoardsTests(BoardService b)
        {
            bs = b;
        }

        /// this test will check the feature of delete board by user.
        /// This function tests Requirement 9
        public void RunTests()
        {
            Response res =JsonSerializer.Deserialize<Response>(bs.DeleteBoard("user3@gmail.com", "test board 2"));
            if (res.ErrorMessage != null)
                Console.WriteLine("not legal arguments");
            else
            {
                Console.WriteLine("Board was deleted successfully");
            }

            Response res1 = JsonSerializer.Deserialize<Response>(bs.DeleteBoard("user1@gmail.com", "Menshe's test Board"));
            if (res1.ErrorMessage != null)
                Console.WriteLine("not legal arguments");
            else
            {
                Console.WriteLine("Board was deleted successfully");
            }

         

          
        }
    }
}
