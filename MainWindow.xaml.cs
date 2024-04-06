using csharp_final.Models;
using csharp_final.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace csharp_final
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var closeResult = MessageBox.Show("Are you sure you want to exit?", "Exit", 
                                              MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (closeResult == MessageBoxResult.Yes)
            {
                var saveResult = MessageBox.Show("Do you want to save current list?", "Save Data", 
                                                 MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (saveResult == MessageBoxResult.Yes)
                {
                    var viewModel = listView.DataContext as PersonListViewModel;
                    await PersonSerializer.SavePersonListAsync(viewModel!.People);
                }
            }
            else
            {
                e.Cancel = true;                
            }
        }
    }
}