using System.Text.Json;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace BackendTests
{
    internal class ForwardTaskTests
    {
        private BoardService bs;

        public ForwardTaskTests(BoardService B)
        {
            bs = B;
        }
        /// this test will check the feature of moving a task from 'backlog' to 'in progress' and from 'in progress' to 'done' by a user.
        /// This function tests Requirement 14
        public void RunTests()
        {
            Response res = JsonSerializer.Deserialize<Response>(bs.ForwardTask("user3@gmail.com", "test board", 1,0));
            if (res.ErrorMessage != null)
                Console.WriteLine("not legal arguments");
            else
            {
                Console.WriteLine("the task got forward successfully");
            }
            Response res1 = JsonSerializer.Deserialize<Response>(bs.ForwardTask("user3@gmail.com", "test board", 1,1));
            if (res1.ErrorMessage != null)
                Console.WriteLine("not legal arguments");
            else
            {
                Console.WriteLine("the task got forward successfully");
            }

            Response res3 = JsonSerializer.Deserialize<Response>(bs.ForwardTask("user2@gmail.com", "test board u2", 1, 0));
            if (res3.ErrorMessage != null)
                Console.WriteLine("not legal arguments");
            else
            {
                Console.WriteLine("task was added successfully");
            }
         
        }
    }
}
