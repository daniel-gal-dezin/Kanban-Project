using Frontend.Model;
using IntroSE.Kanban.Backend.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    public class BoardViewModel : NotifiableObject
    {
        private BoardModel b;
        public UserModel user { get; set; }
        public LinkedList<TaskModel> BacklogTasks { get; set; }
        public LinkedList<TaskModel> InProgressTasks { get; set; }
        public LinkedList<TaskModel> DoneTasks { get; set; }
        public BackendController Controller { get; private set; }
        public BoardViewModel(BoardModel b, BackendController controller)
        {
            this.b = b;
            this.Controller = controller;
            BacklogTasks = b.BackLogTasks;
            InProgressTasks = b.InProgressTasks;
            DoneTasks = b.DoneTasks;
        }
   /*     public void GetBacklogTasks(string email)
        {
            Controller.GetTasks(b.BoardName, email,0);
        }
        public void GetInprogressTasks(string email)
        {
            Controller.GetTasks(b.BoardName, email, 1);
        }
        public void GetDoneTasks(string email)
        {
            Controller.GetTasks(b.BoardName, email, 2);
        }
   */   
    }
}
