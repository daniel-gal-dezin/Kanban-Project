using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class BoardModel : NotifiableModelObject
    {
        private int boardId;
        private string boardName;
        public LinkedList<TaskModel> BackLogTasks { get; set; }
        public LinkedList<TaskModel> InProgressTasks { get; set; }
        public LinkedList<TaskModel> DoneTasks { get; set; }

        public BoardModel(BackendController backendController, int boardID, string boardName, string email) : base(backendController)
        {
            this.boardId = boardID;
            this.boardName = boardName;
            BackLogTasks = backendController.GetTasks(email, boardName, 0);
            InProgressTasks = backendController.GetTasks(email, boardName, 1);
            DoneTasks = backendController.GetTasks(email, boardName, 2);


        }

        public int BoardID
        {
            get => boardId;
            set
            {
                boardId = value;
                RaisePropertyChanged("email");
            }
        }
        public string BoardName
        {
            get => boardName;
            set
            {
                boardName = value;
                RaisePropertyChanged("BoardName");
            }
        }
     
    }
}
