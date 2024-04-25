using Frontend.Model;
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
using System.Windows.Shapes;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for Board.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        private BoardViewModel viewModel;
        private string email;
        public BoardWindow(string email,BoardModel b, BackendController controller)
        {
            InitializeComponent();
            viewModel = new BoardViewModel(b, controller);
            this.DataContext = viewModel;
            this.email = email;
        }
        
    }
}
