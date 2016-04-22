using MyShuttle.Client.Desktop.Infrastructure;
using System.Windows.Controls;

namespace MyShuttle.Client.Desktop.Controls
{
    public partial class Details : UserControl
    {
        public Details()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationHelper.NavigateToEditDriverPage();
        }
    }
}
