using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;

using IntroSE.Kanban.Backend.BuisnessLayer;

namespace Frontend.Model
{
    public class BackendController
    {
        private GradingService gs;
        public BackendController(GradingService g) {
            this.gs = g;
            gs.LoadData();
        }
        public BackendController(){
            this.gs = new GradingService();
            gs.LoadData();

            }
        
        /// <summary>
        ///connect to the login in service
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal UserModel login(string email, string password)
        {
            string user = gs.Login(email, password);
            gs.CreateBoard(email, "board1");
            gs.CreateBoard(email, "board2");
            gs.AddTask(email,"board1","backlog","123",DateTime.Today.AddDays(2));
            gs.AddTask(email, "board1", "in progress", "123", DateTime.Today.AddDays(2));
            gs.AssignTask(email, "board1", 0, 2, email);
            gs.AdvanceTask(email, "board1", 0, 2);
            gs.AddTask(email, "board1", "done", "123", DateTime.Today.AddDays(2));
            gs.AssignTask(email, "board1", 0, 3, email);
            gs.AdvanceTask(email, "board1", 0, 3);
            gs.AdvanceTask(email, "board1", 1, 3);
       
            Response res = JsonSerializer.Deserialize<Response>(user);


            if(res.ReturnValue == null)
            {
                throw new Exception(res.ErrorMessage);
            }
            return new UserModel(this,email);


        }

        /// <summary>
        ///logout user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal void LogOut(string email)
        {
            string json = gs.Logout(email);
            Response res = JsonSerializer.Deserialize<Response>(json);


            if (res.ErrorMessage != null)
            {
                throw new Exception(res.ErrorMessage);
            }


        }


        /// <summary>
        /// connect to the register in service
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <exception cref="Exception"></exception>
        internal void Register(string email, string password)
        {
            string user = gs.Register(email, password);
            Response res = JsonSerializer.Deserialize<Response>(user);
            if (res.ErrorMessage!=null)
            {
                throw new Exception(res.ErrorMessage);
            }
            
        }

        internal LinkedList<int> GetBoards(string email)
        {
            string json = gs.GetUserBoards(email);
            Response res = JsonSerializer.Deserialize<Response>(json);
            if (res.ErrorMessage != null)
            {
                throw new Exception(res.ErrorMessage);
            }

            int[] array = JsonSerializer.Deserialize<int[]>(res.ReturnValue.ToString());
            LinkedList<int> linkedList = new LinkedList<int>(array);

            return linkedList;

        }

        internal string GetBoardName(int id)
        {
            string json = gs.GetBoardName(id);
            Response res = JsonSerializer.Deserialize<Response>(json);
            if (res.ErrorMessage != null)
            {
                throw new Exception(res.ErrorMessage);
            }
            return res.ReturnValue.ToString();

        }
        internal LinkedList<TaskModel> GetTasks(string email, string boardName, int ordinal)
        {
            string json = gs.GetColumn(email,boardName,ordinal);
            Response res = JsonSerializer.Deserialize<Response>(json);

            if (res.ErrorMessage != null)
            {
                throw new Exception(res.ErrorMessage);
            }

            LinkedList<Task> array = JsonSerializer.Deserialize<LinkedList<Task>>(res.ReturnValue.ToString());
            LinkedList<TaskModel> linkedList = new LinkedList<TaskModel>();
            foreach (Task task in array)
                linkedList.AddLast(new TaskModel(this, task.Id, task.Title, task.Description, task.DueDate,task.CreationDate));

            return linkedList;
        }






    }
}
