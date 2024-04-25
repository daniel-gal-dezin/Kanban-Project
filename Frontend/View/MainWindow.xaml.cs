using Frontend.Model;
using Frontend.View;
using Frontend.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Frontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            this.viewModel = new MainViewModel();
            this.DataContext = viewModel;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserModel user = viewModel.Login();
            if(user != null)
            {
                BoardListWindow boardList = new BoardListWindow(user, viewModel.Controller);
                boardList.Show();
                this.Close();
            }

        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Register();

        }
    }
}
