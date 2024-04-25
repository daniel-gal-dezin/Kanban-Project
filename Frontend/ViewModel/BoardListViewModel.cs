using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    public class BoardListViewModel : NotifiableObject
    {
        private UserModel user;

        public BackendController Controller { get; private set; }

        public BoardListViewModel(UserModel user, BackendController controller)
        {
            this.user = user;
            Controller = controller;
        }

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }

        private bool _enableForward = false;
        private BoardModel _selectedBoard;

        public BoardModel SelectedBoard { 
            get { return _selectedBoard; }
            set
            {
                _selectedBoard = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedBoard");
            }
        }

        public bool EnableForward
        {
            get { return _enableForward; }  
            private set { 
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }


        public UserModel User
        {
            get => user;
            set
            {
                user = value;
                RaisePropertyChanged("User");
            }
        }


        /// <summary>
        /// data binding of login
        /// </summary>
        /// <returns></returns>
        public void LogOut()
        {
            Message = "";
            try
            {
                Controller.LogOut(user.Email);
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }

        
    }
}
