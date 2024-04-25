using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public  class UserModel : NotifiableModelObject
    {
        private string email;
        public ObservableCollection<BoardModel> Boards { get; set; }
        public UserModel(BackendController controller, string email) : base(controller)
        {
            this.email = email;
            LinkedList<int> boardIds = controller.GetBoards(email);
            Boards = new ObservableCollection<BoardModel>();
            foreach (int id in boardIds)
            {
                Boards.Add(new BoardModel(controller, id, controller.GetBoardName(id),email));
            }
        }

        public string Email
        {
            get => email;
            set
            {
                email = value;
                RaisePropertyChanged("email");
            }
        }

       
    }

}
