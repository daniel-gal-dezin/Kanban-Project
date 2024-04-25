using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;
namespace BackendTests
{
    internal class AddTaskTests
    {
        private TaskService ts;

        public AddTaskTests(TaskService t)
        {
            ts = t;
        }
        /// this test will check the feature adding a new task.
        /// This function tests Requirement 13
        public void RunTests()
        {
            Response res =JsonSerializer.Deserialize<Response>(ts.AddTask("user3@gmail.com", "test board", "test task1", "01/01/2030", DateTime.Today.AddYears(1)));
            if (res.ErrorMessage != null)
                Console.WriteLine("not legal arguments");
            else
            {
                Console.WriteLine("task was added successfully");
            }
            Response res2 = JsonSerializer.Deserialize<Response>(ts.AddTask("user3@gmail.com", "test board", "test task3", "01/01/2030",  DateTime.Today.AddYears(1)));
            if (res2.ErrorMessage != null)
                Console.WriteLine("not legal arguments");
            else
            {
                Console.WriteLine("task was added successfully");
            }
            Response res3 = JsonSerializer.Deserialize<Response>(ts.AddTask("Shai......Manor@gmail.com", "test board", "test task4", "01/01/2030", DateTime.Today.AddYears(1) ));
            if (res3.ErrorMessage != null)
                Console.WriteLine("not legal arguments");
            else
            {
                Console.WriteLine("task was added successfully");
            }
            Response res4 = JsonSerializer.Deserialize<Response>(ts.AddTask("user3@gmail.com", "test board", "test task3", null, DateTime.Today.AddYears(1)));
            if (res4.ErrorMessage != null)
                Console.WriteLine("not legal arguments");
            else
            {
                Console.WriteLine("task was added successfully");
            }
            Response res5 = JsonSerializer.Deserialize<Response>(ts.AddTask("user2@gmail.com", "test board", "task 6", "domr task", DateTime.Now.AddDays(1)));
            if (res5.ErrorMessage != null)
                Console.WriteLine("not legal arguments");
            else
            {
                Console.WriteLine("task was added successfully");
            }
        }
    }
}
