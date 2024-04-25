using Frontend.Model;
using Frontend.ViewModel;
using IntroSE.Kanban.Backend.BuisnessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for BoardListWindow.xaml
    /// </summary>
    public partial class BoardListWindow : Window
    {
        private BoardListViewModel viewModel;
        private string email;
        public BoardListWindow(UserModel u, BackendController controller)
        {
            InitializeComponent();
            viewModel = new BoardListViewModel(u,controller);
            this.DataContext = viewModel;
            this.email = u.Email;
        }
        public void Board_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            BoardModel board = (BoardModel)b.DataContext;
            string email = this.email;
            if (board != null)
            {
                viewModel.SelectedBoard = board;
                BoardWindow boardView = new BoardWindow(email,board, viewModel.Controller);
                boardView.Show();
                this.Close();
            }




        }

        public void Logout_Click(object sender, RoutedEventArgs e)
        {
            viewModel.LogOut();
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();

        }
    }
}
