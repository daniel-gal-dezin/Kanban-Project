using Frontend.Model;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
   class MainViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }
        public MainViewModel()
        {
            this.Controller = new BackendController();
          
        }


        private string email;
        
        public string Email
        {
            get => email; 
            set 
             {
                this.email = value;
                RaisePropertyChanged("email");
            }

        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                this.password = value;
                RaisePropertyChanged("email");
            }

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
        /// <summary>
        /// data binding of login
        /// </summary>
        /// <returns></returns>
        public UserModel Login()
        {
            Message = "";
            try
            {
                return Controller.login(Email, Password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }
        /// <summary>
        /// data binding of register
        /// </summary>
        public void Register()
        {
            Message = "";
            try
            {
                Controller.Register(Email, Password);
                Message = "Registered successfully";
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }
    }
}
